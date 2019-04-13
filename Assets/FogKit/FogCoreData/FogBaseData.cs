using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BabelTime.GD.UI.BattleUISub.FogAbout
{
	/// <summary>
	/// 全场迷雾数据
	/// 
	/// 通过ulong类型存储8*8方格的alpha数据
	/// 通过ulong的二维数组记录全场迷雾数据
	/// 
	/// 具体小格的数据运算通过移位获取，求逻辑与获取alpha结果
	/// </summary>
	public class FogBaseData
	{
		const ulong ALPHABASE = 0x0000000000000001;
		const ulong TRANSPARENT = 0xffffffffffffffff;

		public const int AlphaArraySize = 8;

		public ulong [,] totalAlphaArray;

		public int w;
		public int h;

		public int array_w;
		public int array_h;

		Dictionary<int, FogTransparentRange> fogRangeDic;

		public FogDataFixList fixList;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:BabelTime.GD.UI.BattleUISub.FogAbout.FogBaseData"/> class.
		/// </summary>
		/// <param name="w">世界地图数据存储宽度.</param>
		/// <param name="h">世界地图数据存储高度.</param>
		public FogBaseData (int w, int h)
		{
			this.w = w;
			this.h = h;

			array_w = (int) System.Math.Ceiling ((double) w / AlphaArraySize);
			array_h = (int) System.Math.Ceiling ((double) h / AlphaArraySize);

			totalAlphaArray = new ulong [array_w, array_h];
			fogRangeDic = new Dictionary<int, FogTransparentRange> ();
			fixList = new FogDataFixList ();
		}

		private ulong _AlphaPos (int x, int y)
		{
			return ALPHABASE << (x * 8 + y);
		}

		#region Set Data Methods

		public void SetTransparent (int x, int y)
		{
			int subX = x % AlphaArraySize;
			int subY = y % AlphaArraySize;
			SetTransparent (x / AlphaArraySize, y / AlphaArraySize, subX, subY);
			fixList.Add (x, y, true);
		}

		private void SetTransparent (int arrayX, int arrayY, int x, int y)
		{
			totalAlphaArray [arrayX, arrayY] |= _AlphaPos (x, y);
		}

		public void SetOpaque (int x, int y)
		{
			int subX = x % AlphaArraySize;
			int subY = y % AlphaArraySize;
			SetOpaque (x / AlphaArraySize, y / AlphaArraySize, subX, subY);
			fixList.Add (x, y, false);
		}

		private void SetOpaque (int arrayX, int arrayY, int x, int y)
		{
			totalAlphaArray [arrayX, arrayY] &= ~_AlphaPos (x, y);
		}

		#endregion

		#region Check Methods

		public bool IsTransparent (int x, int y)
		{
			int subX = x % AlphaArraySize;
			int subY = y % AlphaArraySize;
			return IsTransparent (x / AlphaArraySize, y / AlphaArraySize, subX, subY);
		}

		public bool IsTransparent (int arrayX, int arrayY, int subX, int subY)
		{
			return (totalAlphaArray [arrayX, arrayY] & _AlphaPos (subX, subY)) > 0;
		}

		#endregion

		#region Update Range Methods

		public void UpdateTransparent (int x, int y, int r)
		{
			FogTransparentRange rangeData = null;
			if (!fogRangeDic.ContainsKey (r)) {
				rangeData = new FogTransparentRange (r);
				fogRangeDic [r] = rangeData;
			} else {
				rangeData = fogRangeDic [r];
			}
			/// 先求取视野范围内的有效坐标
			/// 即根据视点坐标求出视野范围内有效用于战场的尺寸范围
			/// 有效坐标奉行小闭大开原则
			//if (x - r >= 0) {
			//	vl = 0;
			//} else {
			//	vl = r - x;
			//}
			int view_l = (x - r >= 0) ? 0 : (r - x);
			//if (x + r < w) {
			//	vr = 2 * r + 1;
			//} else {
			//	vr = r - x + w;
			//}
			int view_r = (x + r < w) ? (2 * r + 1) : (r - x + w);
			//if (y - r >= 0) {
			//	vt = 0;
			//} else {
			//	vt = r - y;
			//}
			int view_t = (y - r >= 0) ? 0 : (r - y);
			//if (y + r < h) {
			//	vb = 2 * r + 1;
			//} else {
			//	vb = r - y + h;
			//}
			int view_b = (y + r < h) ? (2 * r + 1) : (r - y + h);
			int view_lt_x = x - r;
			int view_lt_y = y - r;


			/// 操作九宫格四边 上（y值为0开始）
			_UpdateTransparentTop (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);
			/// 操作九宫格四边 左（x值为0开始）
			_UpdateTransparentLeft (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);

			/// 操作九宫格中心全部为透明区域
			_UpdateTransparentCenter (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);

			/// 操作九宫格四边 右
			_UpdateTransparentRight (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);
			/// 操作九宫格四边 下
			_UpdateTransparentBottom (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);

			/// 忽略九宫格四角
		}

		private void _UpdateTransparentCenter (FogTransparentRange rangeData,
											   int view_lt_x, int view_lt_y,
											   int view_left, int view_right, int view_top, int view_bottom)
		{
			int xMin = Max (view_left, rangeData.l);
			int xMax = Min (view_right, rangeData.r + 1);
			int yMin = Max (view_top, rangeData.t);
			int yMax = Min (view_bottom, rangeData.b + 1);
			if (xMin < xMax && yMin < yMax) {
				for (int i = xMin, j = 0; i < xMax; i++) {
					for (j = yMin; j < yMax; j++) {
						SetTransparent (view_lt_x + i, view_lt_y + j);
					}
				}
			}
		}

		private void _UpdateTransparentTop (FogTransparentRange rangeData,
											int view_lt_x, int view_lt_y,
											int view_left, int view_right, int view_top, int view_bottom)
		{
			int xMin = Max (view_left, rangeData.l);
			int xMax = Min (view_right, rangeData.r + 1);
			int yMin = Max (view_top, 0);
			int yMax = Min (view_bottom, rangeData.t + 1);
			if (xMin < xMax && yMin < yMax) {
				for (int i = xMin, j = 0; i < xMax; i++) {
					for (j = yMin; j < yMax; j++) {
						if (rangeData.array [i, j]) {
							SetTransparent (view_lt_x + i, view_lt_y + j);
						}
					}
				}
			}
		}

		private void _UpdateTransparentBottom (FogTransparentRange rangeData,
											   int view_lt_x, int view_lt_y,
											   int view_left, int view_right, int view_top, int view_bottom)
		{
			int xMin = Max (view_left, rangeData.l);
			int xMax = Min (view_right, rangeData.r + 1);
			int yMin = Max (view_top, rangeData.b);
			int yMax = Min (view_bottom, rangeData.size);
			if (xMin < xMax && yMin < yMax) {
				for (int i = xMin, j = 0; i < xMax; i++) {
					for (j = yMin; j < yMax; j++) {
						if (rangeData.array [i, j]) {
							SetTransparent (view_lt_x + i, view_lt_y + j);
						}
					}
				}
			}
		}

		private void _UpdateTransparentLeft (FogTransparentRange rangeData,
											 int view_lt_x, int view_lt_y,
											 int view_left, int view_right, int view_top, int view_bottom)
		{
			int xMin = Max (view_left, 0);
			int xMax = Min (view_right, rangeData.l + 1);
			int yMin = Max (view_top, rangeData.t);
			int yMax = Min (view_bottom, rangeData.b + 1);
			if (xMin < xMax && yMin < yMax) {
				for (int i = xMin, j = 0; i < xMax; i++) {
					for (j = yMin; j < yMax; j++) {
						if (rangeData.array [i, j]) {
							SetTransparent (view_lt_x + i, view_lt_y + j);
						}
					}
				}
			}
		}

		private void _UpdateTransparentRight (FogTransparentRange rangeData,
											  int view_lt_x, int view_lt_y,
											  int view_left, int view_right, int view_top, int view_bottom)
		{
			int xMin = Max (view_left, rangeData.r);
			int xMax = Min (view_right, rangeData.size);
			int yMin = Max (view_top, rangeData.t);
			int yMax = Min (view_bottom, rangeData.b + 1);
			if (xMin < xMax && yMin < yMax) {
				for (int i = xMin, j = 0; i < xMax; i++) {
					for (j = yMin; j < yMax; j++) {
						if (rangeData.array [i, j]) {
							SetTransparent (view_lt_x + i, view_lt_y + j);
						}
					}
				}
			}
		}

		private int Max (int val1, int val2)
		{
			return val1 > val2 ? val1 : val2;
		}

		private int Min (int val1, int val2)
		{
			return val1 > val2 ? val2 : val1;
		}

		#endregion

		public void CleanFixList ()
		{
			fixList.Clear ();
		}
	}
}