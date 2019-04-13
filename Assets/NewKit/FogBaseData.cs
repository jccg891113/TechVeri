/*
 * FogBaseData.cs
 * 
 * Author:
 * 		Jack Wen <sachuan@foxmail.com>
 * 
 * Copyright (c) 2019 a. All Right Reserved
 * 
 * Desc:
 * 		迷雾组件，由显示端迷雾组件精简移植
 * 
 * History:
 * 		Data	|	Version		|	Author	|	Details
 * -------------|---------------|-----------|---------------------------------------------------------------------------
 * 2019-4-11	|0.3.0			|Jack Wen	|Add
 * 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Logic.FogAbout
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
		/// <summary>
		/// 定义mask数组，用于定义8*8分隔为四份时的掩码
		/// </summary>
		private ulong[,] MASK_XMIN_YMIN = new ulong[,] { {
				0x0000000000000001,
				0x0000000000000101,
				0x0000000000010101,
				0x0000000001010101,
				0x0000000101010101,
				0x0000010101010101,
				0x0001010101010101,
				0x0101010101010101
			}, {
				0x0000000000000003,
				0x0000000000000303,
				0x0000000000030303,
				0x0000000003030303,
				0x0000000303030303,
				0x0000030303030303,
				0x0003030303030303,
				0x0303030303030303
			}, { 
				0x0000000000000007,
				0x0000000000000707,
				0x0000000000070707,
				0x0000000007070707,
				0x0000000707070707,
				0x0000070707070707,
				0x0007070707070707,
				0x0707070707070707
			}, {
				0x000000000000000f,
				0x0000000000000f0f,
				0x00000000000f0f0f,
				0x000000000f0f0f0f,
				0x0000000f0f0f0f0f,
				0x00000f0f0f0f0f0f,
				0x000f0f0f0f0f0f0f,
				0x0f0f0f0f0f0f0f0f
			}, {
				0x000000000000001f,
				0x0000000000001f1f,
				0x00000000001f1f1f,
				0x000000001f1f1f1f,
				0x0000001f1f1f1f1f,
				0x00001f1f1f1f1f1f,
				0x001f1f1f1f1f1f1f,
				0x1f1f1f1f1f1f1f1f
			}, {
				0x000000000000003f,
				0x0000000000003f3f,
				0x00000000003f3f3f,
				0x000000003f3f3f3f,
				0x0000003f3f3f3f3f,
				0x00003f3f3f3f3f3f,
				0x003f3f3f3f3f3f3f,
				0x3f3f3f3f3f3f3f3f
			}, {
				0x000000000000007f,
				0x0000000000007f7f,
				0x00000000007f7f7f,
				0x000000007f7f7f7f,
				0x0000007f7f7f7f7f,
				0x00007f7f7f7f7f7f,
				0x007f7f7f7f7f7f7f,
				0x7f7f7f7f7f7f7f7f
			}, {
				0x00000000000000ff,
				0x000000000000ffff,
				0x0000000000ffffff,
				0x00000000ffffffff,
				0x000000ffffffffff,
				0x0000ffffffffffff,
				0x00ffffffffffffff,
				0xffffffffffffffff
			}
		};

		private ulong[,] MASK_XMAX_YMIN = new ulong[,] { {
				0x0000000000000080,
				0x0000000000008080,
				0x0000000000808080,
				0x0000000080808080,
				0x0000008080808080,
				0x0000808080808080,
				0x0080808080808080,
				0x8080808080808080
			}, {
				0x00000000000000c0,
				0x000000000000c0c0,
				0x0000000000c0c0c0,
				0x00000000c0c0c0c0,
				0x000000c0c0c0c0c0,
				0x0000c0c0c0c0c0c0,
				0x00c0c0c0c0c0c0c0,
				0xc0c0c0c0c0c0c0c0
			}, {
				0x00000000000000e0,
				0x000000000000e0e0,
				0x0000000000e0e0e0,
				0x00000000e0e0e0e0,
				0x000000e0e0e0e0e0,
				0x0000e0e0e0e0e0e0,
				0x00e0e0e0e0e0e0e0,
				0xe0e0e0e0e0e0e0e0
			}, {
				0x00000000000000f0,
				0x000000000000f0f0,
				0x0000000000f0f0f0,
				0x00000000f0f0f0f0,
				0x000000f0f0f0f0f0,
				0x0000f0f0f0f0f0f0,
				0x00f0f0f0f0f0f0f0,
				0xf0f0f0f0f0f0f0f0
			}, {
				0x00000000000000f8,
				0x000000000000f8f8,
				0x0000000000f8f8f8,
				0x00000000f8f8f8f8,
				0x000000f8f8f8f8f8,
				0x0000f8f8f8f8f8f8,
				0x00f8f8f8f8f8f8f8,
				0xf8f8f8f8f8f8f8f8
			}, {
				0x00000000000000fc,
				0x000000000000fcfc,
				0x0000000000fcfcfc,
				0x00000000fcfcfcfc,
				0x000000fcfcfcfcfc,
				0x0000fcfcfcfcfcfc,
				0x00fcfcfcfcfcfcfc,
				0xfcfcfcfcfcfcfcfc
			}, {
				0x00000000000000fe,
				0x000000000000fefe,
				0x0000000000fefefe,
				0x00000000fefefefe,
				0x000000fefefefefe,
				0x0000fefefefefefe,
				0x00fefefefefefefe,
				0xfefefefefefefefe
			}, {
				0x00000000000000ff,
				0x000000000000ffff,
				0x0000000000ffffff,
				0x00000000ffffffff,
				0x000000ffffffffff,
				0x0000ffffffffffff,
				0x00ffffffffffffff,
				0xffffffffffffffff
			}
		};

		private ulong[,] MASK_XMIN_YMAX = new ulong[,] { {
				0x0100000000000000,
				0x0101000000000000,
				0x0101010000000000,
				0x0101010100000000,
				0x0101010101000000,
				0x0101010101010000,
				0x0101010101010100,
				0x0101010101010101
			}, {
				0x0300000000000000,
				0x0303000000000000,
				0x0303030000000000,
				0x0303030300000000,
				0x0303030303000000,
				0x0303030303030000,
				0x0303030303030300,
				0x0303030303030303
			}, {
				0x0700000000000000,
				0x0707000000000000,
				0x0707070000000000,
				0x0707070700000000,
				0x0707070707000000,
				0x0707070707070000,
				0x0707070707070700,
				0x0707070707070707
			}, {
				0x0f00000000000000,
				0x0f0f000000000000,
				0x0f0f0f0000000000,
				0x0f0f0f0f00000000,
				0x0f0f0f0f0f000000,
				0x0f0f0f0f0f0f0000,
				0x0f0f0f0f0f0f0f00,
				0x0f0f0f0f0f0f0f0f
			}, {
				0x1f00000000000000,
				0x1f1f000000000000,
				0x1f1f1f0000000000,
				0x1f1f1f1f00000000,
				0x1f1f1f1f1f000000,
				0x1f1f1f1f1f1f0000,
				0x1f1f1f1f1f1f1f00,
				0x1f1f1f1f1f1f1f1f
			}, {
				0x3f00000000000000,
				0x3f3f000000000000,
				0x3f3f3f0000000000,
				0x3f3f3f3f00000000,
				0x3f3f3f3f3f000000,
				0x3f3f3f3f3f3f0000,
				0x3f3f3f3f3f3f3f00,
				0x3f3f3f3f3f3f3f3f
			}, {
				0x7f00000000000000,
				0x7f7f000000000000,
				0x7f7f7f0000000000,
				0x7f7f7f7f00000000,
				0x7f7f7f7f7f000000,
				0x7f7f7f7f7f7f0000,
				0x7f7f7f7f7f7f7f00,
				0x7f7f7f7f7f7f7f7f
			}, {
				0xff00000000000000,
				0xffff000000000000,
				0xffffff0000000000,
				0xffffffff00000000,
				0xffffffffff000000,
				0xffffffffffff0000,
				0xffffffffffffff00,
				0xffffffffffffffff
			}
		};

		private ulong[,] MASK_XMAX_YMAX = new ulong[,] { {
				0x8000000000000000,
				0x8080000000000000,
				0x8080800000000000,
				0x8080808000000000,
				0x8080808080000000,
				0x8080808080800000,
				0x8080808080808000,
				0x8080808080808080
			}, {
				0xc000000000000000,
				0xc0c0000000000000,
				0xc0c0c00000000000,
				0xc0c0c0c000000000,
				0xc0c0c0c0c0000000,
				0xc0c0c0c0c0c00000,
				0xc0c0c0c0c0c0c000,
				0xc0c0c0c0c0c0c0c0
			}, {
				0xe000000000000000,
				0xe0e0000000000000,
				0xe0e0e00000000000,
				0xe0e0e0e000000000,
				0xe0e0e0e0e0000000,
				0xe0e0e0e0e0e00000,
				0xe0e0e0e0e0e0e000,
				0xe0e0e0e0e0e0e0e0
			}, {
				0xf000000000000000,
				0xf0f0000000000000,
				0xf0f0f00000000000,
				0xf0f0f0f000000000,
				0xf0f0f0f0f0000000,
				0xf0f0f0f0f0f00000,
				0xf0f0f0f0f0f0f000,
				0xf0f0f0f0f0f0f0f0
			}, {
				0xf800000000000000,
				0xf8f8000000000000,
				0xf8f8f80000000000,
				0xf8f8f8f800000000,
				0xf8f8f8f8f8000000,
				0xf8f8f8f8f8f80000,
				0xf8f8f8f8f8f8f800,
				0xf8f8f8f8f8f8f8f8
			}, {
				0xfc00000000000000,
				0xfcfc000000000000,
				0xfcfcfc0000000000,
				0xfcfcfcfc00000000,
				0xfcfcfcfcfc000000,
				0xfcfcfcfcfcfc0000,
				0xfcfcfcfcfcfcfc00,
				0xfcfcfcfcfcfcfcfc
			}, {
				0xfe00000000000000,
				0xfefe000000000000,
				0xfefefe0000000000,
				0xfefefefe00000000,
				0xfefefefefe000000,
				0xfefefefefefe0000,
				0xfefefefefefefe00,
				0xfefefefefefefefe
			}, {
				0xff00000000000000,
				0xffff000000000000,
				0xffffff0000000000,
				0xffffffff00000000,
				0xffffffffff000000,
				0xffffffffffff0000,
				0xffffffffffffff00,
				0xffffffffffffffff
			}
		};

		private const ulong ALPHABASE = 0x0000000000000001;
		private const ulong TRANSPARENT = 0xffffffffffffffff;

		private const int AlphaArraySize = 8;
		private const int AlphaArraySizeMask = 0x07;
		private const int AlphaArrayBitSize = 3;

		/// <summary>
		/// 地图压缩数据存储数组
		/// </summary>
		private ulong[,] totalAlphaArray;

		/// <summary>
		/// 地图实际数据宽、高
		/// </summary>
		private int w, h;

		/// <summary>
		/// 地图压缩数据宽、高
		/// </summary>
		private int array_w, array_h;

		/// <summary>
		/// 视野范围数据列表
		/// </summary>
		List<FogTransparentRange> __fogRangeList;
		/// <summary>
		/// 视野范围查询字典
		/// </summary>
		Dictionary<int, int> __fogRangeTable;
		/// <summary>
		/// 视野范围缓存
		/// </summary>
		FogTransparentRange __fogRangeCache;

		/// <summary>
		/// Initializes a new instance of the <see cref="Battle.Logic.FogAbout.FogBaseData"/> class.
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

#if DEBUG && !PROFILER
//			FTools.FogDebug.LogAllocSize ("[BaseData]", default (ulong), array_w * array_h);
#endif

			__fogRangeList = new List<FogTransparentRange> ();
			__fogRangeTable = new Dictionary<int, int> ();
			__fogRangeCache = null;
		}

		private ulong __AlphaPos (int x, int y)
		{
			return ALPHABASE << ((x << AlphaArrayBitSize) + y);
		}

		#region Set Data Methods

		private void __SetTransparent (int x, int y)
		{
			totalAlphaArray [x >> AlphaArrayBitSize, y >> AlphaArrayBitSize] |= __AlphaPos (x & AlphaArraySizeMask, y & AlphaArraySizeMask);
		}

		private void __SetOpaque (int x, int y)
		{
			totalAlphaArray [x >> AlphaArrayBitSize, y >> AlphaArrayBitSize] &= ~__AlphaPos (x & AlphaArraySizeMask, y & AlphaArraySizeMask);
		}

		#endregion

		#region Check Methods

		public bool IsTransparent (int x, int y)
		{
			return IsTransparent (x >> AlphaArrayBitSize, y >> AlphaArrayBitSize, x & AlphaArraySizeMask, y & AlphaArraySizeMask);
		}

		public bool IsTransparent (int arrayX, int arrayY, int subX, int subY)
		{
			if (arrayX >= 0 && arrayX < array_w && arrayY >= 0 && arrayY < array_h) {
				return (totalAlphaArray [arrayX, arrayY] & __AlphaPos (subX, subY)) > 0;
			} else {
#if DEBUG && !PROFILER
//				EditorDebug.Error ("FogBaseData.IsTransparent DATA ERROR!\r\n" +
//				string.Format ("w:{0}, x:{1}, h:{2}, y:{3}, sub_x:{4}, sub_y:{5}", array_w, arrayX, array_h, arrayY, subX, subY));
#endif
				return false;
			}
		}

		#endregion

		#region Update Range Methods

		public void UpdateTransparent (int x, int y, int r)
		{
			#if UNITY_EDITOR
			global::System.Text.StringBuilder sb = new global::System.Text.StringBuilder ();
			global::System.Diagnostics.Stopwatch sw = new global::System.Diagnostics.Stopwatch ();
			#endif

			#if UNITY_EDITOR
			sw.Reset ();
			sw.Start ();
			#endif

			FogTransparentRange rangeData = __GetRangeInfo (r);

			#if UNITY_EDITOR
			sw.Stop ();
			sb.AppendLine (string.Format ("cost {0} ms/{1} ticks", sw.Elapsed.TotalMilliseconds, sw.ElapsedTicks));
			#endif

			#if UNITY_EDITOR
			sw.Reset ();
			sw.Start ();
			#endif

			/// 先求取视野范围内的有效坐标
			/// 即根据视点坐标求出视野范围内有效用于战场的尺寸范围
			/// 有效坐标奉行小闭大开原则
			int view_l = (x - r >= 0) ? 0 : (r - x);
			int view_r = (x + r < w) ? (2 * r + 1) : (r - x + w);
			int view_t = (y - r >= 0) ? 0 : (r - y);
			int view_b = (y + r < h) ? (2 * r + 1) : (r - y + h);
			int view_lt_x = x - r;
			int view_lt_y = y - r;

			#if UNITY_EDITOR
			sw.Stop ();
			sb.AppendLine (string.Format ("cost {0} ms/{1} ticks", sw.Elapsed.TotalMilliseconds, sw.ElapsedTicks));
			#endif

			#if UNITY_EDITOR
			sw.Reset ();
			sw.Start ();
			#endif

			/// 操作九宫格四边 上（y值为0开始）
			__UpdateTransparentTop (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);

			#if UNITY_EDITOR
			sw.Stop ();
			sb.AppendLine (string.Format ("cost {0} ms/{1} ticks", sw.Elapsed.TotalMilliseconds, sw.ElapsedTicks));
			#endif

			#if UNITY_EDITOR
			sw.Reset ();
			sw.Start ();
			#endif

			/// 操作九宫格四边 左（x值为0开始）
			__UpdateTransparentLeft (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);

			#if UNITY_EDITOR
			sw.Stop ();
			sb.AppendLine (string.Format ("cost {0} ms/{1} ticks", sw.Elapsed.TotalMilliseconds, sw.ElapsedTicks));
			#endif

			#if UNITY_EDITOR
			sw.Reset ();
			sw.Start ();
			#endif

			/// 操作九宫格中心全部为透明区域
			__UpdateTransparentCenter (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);

			#if UNITY_EDITOR
			sw.Stop ();
			sb.AppendLine (string.Format ("cost {0} ms/{1} ticks", sw.Elapsed.TotalMilliseconds, sw.ElapsedTicks));
			#endif

			#if UNITY_EDITOR
			sw.Reset ();
			sw.Start ();
			#endif

			/// 操作九宫格四边 右
			__UpdateTransparentRight (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);

			#if UNITY_EDITOR
			sw.Stop ();
			sb.AppendLine (string.Format ("cost {0} ms/{1} ticks", sw.Elapsed.TotalMilliseconds, sw.ElapsedTicks));
			#endif

			#if UNITY_EDITOR
			sw.Reset ();
			sw.Start ();
			#endif

			/// 操作九宫格四边 下
			__UpdateTransparentBottom (rangeData, view_lt_x, view_lt_y, view_l, view_r, view_t, view_b);
			/// 忽略九宫格四角

			#if UNITY_EDITOR
			sw.Stop ();
			sb.AppendLine (string.Format ("cost {0} ms/{1} ticks", sw.Elapsed.TotalMilliseconds, sw.ElapsedTicks));
			#endif

			#if UNITY_EDITOR
			string __log = sb.ToString ();
			if (__log.Length > 0) {
				UnityEngine.Debug.LogError (__log);
			}
			#endif

		}

		private FogTransparentRange __GetRangeInfo (int r)
		{
			if (__fogRangeCache != null && __fogRangeCache.base_r == r) {
				return __fogRangeCache;
			}
			int index;
			if (!__fogRangeTable.TryGetValue (r, out index)) {
				FogTransparentRange rangeData = new FogTransparentRange (r);
				__fogRangeTable.Add (r, __fogRangeList.Count);
				__fogRangeList.Add (rangeData);
				__fogRangeCache = rangeData;
			} else {
				__fogRangeCache = __fogRangeList [index];
			}
			return __fogRangeCache;
		}

		//		private void __UpdateTransparentCenter (FogTransparentRange rangeData,
		//		                                        int view_lt_x, int view_lt_y,
		//		                                        int view_left, int view_right, int view_top, int view_bottom)
		//		{
		//			int xMin = __Max (view_left, rangeData.l) + view_lt_x;
		//			int xMax = __Min (view_right, rangeData.r + 1) + view_lt_x;
		//			int yMin = __Max (view_top, rangeData.t) + view_lt_y;
		//			int yMax = __Min (view_bottom, rangeData.b + 1) + view_lt_y;
		//			if (xMin < xMax && yMin < yMax) {
		//				// 求出方形透明区域内的几个临界值，然后采用for循环与位运算结合求值
		//				int _left_array_x = xMin >> AlphaArrayBitSize;
		//				int _left_bit_x = xMin & AlphaArraySizeMask;
		//				int _right_array_x = (xMax - 1) >> AlphaArrayBitSize;
		//				int _right_bit_x = (xMax - 1) & AlphaArraySizeMask;
		//				int _top_array_y = yMin >> AlphaArrayBitSize;
		//				int _top_bit_y = yMin & AlphaArraySizeMask;
		//				int _bottom_array_y = (yMax - 1) >> AlphaArrayBitSize;
		//				int _bottom_bit_y = (yMax - 1) & AlphaArraySizeMask;
		//
		//				// 刷新求出区域后的中间场景
		//				int _c_array_l, _c_array_r, _c_array_t, _c_array_b;
		//				_c_array_l = (_left_bit_x == 0) ? _left_array_x : (_left_array_x + 1);
		//				_c_array_r = (_right_bit_x == AlphaArraySize - 1) ? _right_array_x : (_right_array_x - 1);
		//				_c_array_t = (_top_bit_y == 0) ? _top_array_y : (_top_array_y + 1);
		//				_c_array_b = (_bottom_bit_y == AlphaArraySize - 1) ? _bottom_array_y : (_bottom_array_y - 1);
		//
		//				// top
		//				for (int i = xMin, j = yMin, jmax = _c_array_t * AlphaArraySize; i < xMax; i++) {
		//					for (j = yMin; j < jmax; j++) {
		//						__SetTransparent (i, j);
		//					}
		//				}
		//
		//				// left
		//				for (int i = xMin, j = 0, jbegin = _c_array_t * AlphaArraySize, imax = _c_array_l * AlphaArraySize, jmax = (_c_array_b + 1) * AlphaArraySize; i < imax; i++) {
		//					for (j = jbegin; j < jmax; j++) {
		//						__SetTransparent (i, j);
		//					}
		//				}
		//
		//				// center
		//				for (int i = _c_array_l, j = _c_array_t, imax = _c_array_r + 1, jmax = _c_array_b + 1; i < imax; i++) {
		//					for (j = _c_array_t; j < jmax; j++) {
		//						totalAlphaArray [i, j] |= TRANSPARENT;
		//					}
		//				}
		//
		//				// right
		//				for (int i = (_c_array_r + 1) * AlphaArraySize, j = 0, jbegin = _c_array_t * AlphaArraySize, jmax = (_c_array_b + 1) * AlphaArraySize; i < xMax; i++) {
		//					for (j = jbegin; j < jmax; j++) {
		//						__SetTransparent (i, j);
		//					}
		//				}
		//
		//				// bottom
		//				for (int i = xMin, j = 0, jBegin = (_c_array_b + 1) * AlphaArraySize; i < xMax; i++) {
		//					for (j = jBegin; j < yMax; j++) {
		//						__SetTransparent (i, j);
		//					}
		//				}
		//			}
		//		}

		private void __UpdateTransparentCenter (FogTransparentRange rangeData,
		                                        int view_lt_x, int view_lt_y,
		                                        int view_left, int view_right, int view_top, int view_bottom)
		{
			int xMin = __Max (view_left, rangeData.l) + view_lt_x;
			int xMax = __Min (view_right, rangeData.r + 1) + view_lt_x;
			int yMin = __Max (view_top, rangeData.t) + view_lt_y;
			int yMax = __Min (view_bottom, rangeData.b + 1) + view_lt_y;
			if (xMin < xMax && yMin < yMax) {
				// 求出方形透明区域内的几个临界值，然后采用for循环与位运算结合求值
				int _left_array_x = xMin >> AlphaArrayBitSize;
				int _left_bit_x = xMin & AlphaArraySizeMask;
				int _right_array_x = (xMax - 1) >> AlphaArrayBitSize;
				int _right_bit_x = (xMax - 1) & AlphaArraySizeMask;
				int _top_array_y = yMin >> AlphaArrayBitSize;
				int _top_bit_y = yMin & AlphaArraySizeMask;
				int _bottom_array_y = (yMax - 1) >> AlphaArrayBitSize;
				int _bottom_bit_y = (yMax - 1) & AlphaArraySizeMask;
		
				// 刷新求出区域后的中间场景
				int _c_array_l = (_left_bit_x == 0) ? _left_array_x : (_left_array_x + 1);
				int _c_array_r = (_right_bit_x == AlphaArraySize - 1) ? _right_array_x : (_right_array_x - 1);
				int _c_array_t = (_top_bit_y == 0) ? _top_array_y : (_top_array_y + 1);
				int _c_array_b = (_bottom_bit_y == AlphaArraySize - 1) ? _bottom_array_y : (_bottom_array_y - 1);
		
				// +---+--------+---+
				// | A |        | B |
				// +---+--------+---+
				// |   |        |   |
				// |   |        |   |
				// |   |        |   |
				// |   |        |   |
				// +---+--------+---+
				// | D |        | C |
				// +---+--------+---+
				int i, j, imax, jmax;
		
				for (i = _c_array_l, imax = _c_array_r + 1, jmax = _c_array_b + 1; i < imax; i++) {
					for (j = _c_array_t; j < jmax; j++) {
						totalAlphaArray [i, j] |= TRANSPARENT;
					}
				}
		
				int __last_bit_left = AlphaArraySizeMask - _left_bit_x;
				int __last_bit_right = _right_bit_x;
				int __last_bit_top = AlphaArraySizeMask - _top_bit_y;
				int __last_bit_bottom = _bottom_bit_y;
		
//				if (_left_bit_x != 0 && _top_bit_y != 0) {
//					totalAlphaArray [_left_array_x, _top_array_y] |= MASK_XMAX_YMAX [__last_bit_left, __last_bit_top];
//				}
//				if (_left_bit_x != 0 && _bottom_bit_y != AlphaArraySizeMask) {
//					totalAlphaArray [_left_array_x, _bottom_array_y] |= MASK_XMAX_YMIN [__last_bit_left, __last_bit_bottom];
//				}
//				if (_right_bit_x != AlphaArraySizeMask && _top_bit_y != 0) {
//					totalAlphaArray [_right_array_x, _top_array_y] |= MASK_XMIN_YMAX [__last_bit_right, __last_bit_top];
//				}
//				if (_right_bit_x != AlphaArraySizeMask && _bottom_bit_y != AlphaArraySizeMask) {
//					totalAlphaArray [_right_array_x, _bottom_array_y] |= MASK_XMIN_YMIN [__last_bit_right, __last_bit_bottom];
//				}
//		
//				for (i = _c_array_l, imax = _c_array_r + 1; i < imax; i++) {
//					totalAlphaArray [i, _top_array_y] |= MASK_XMAX_YMAX [AlphaArraySizeMask, __last_bit_top];
//				}
//				for (i = _c_array_l, imax = _c_array_r + 1; i < imax; i++) {
//					totalAlphaArray [i, _bottom_array_y] |= MASK_XMAX_YMIN [AlphaArraySizeMask, __last_bit_bottom];
//				}
//				for (j = _c_array_t, jmax = _c_array_b + 1; j < jmax; j++) {
//					totalAlphaArray [_left_array_x, j] |= MASK_XMAX_YMIN [__last_bit_left, AlphaArraySizeMask];
//				}
//				for (j = _c_array_t, jmax = _c_array_b + 1; j < jmax; j++) {
//					totalAlphaArray [_right_array_x, j] |= MASK_XMIN_YMIN [__last_bit_right, AlphaArraySizeMask];
//				}

				if (_left_bit_x != 0 && _top_bit_y != 0) {
					totalAlphaArray [_left_array_x, _top_array_y] |= MASK_XMAX_YMAX [__last_bit_left, __last_bit_top];
				}
				if (_left_bit_x != 0 && _bottom_bit_y != AlphaArraySizeMask) {
					totalAlphaArray [_left_array_x, _bottom_array_y] |= MASK_XMAX_YMIN [__last_bit_left, __last_bit_bottom];
				}
				if (_right_bit_x != AlphaArraySizeMask && _top_bit_y != 0) {
					totalAlphaArray [_right_array_x, _top_array_y] |= MASK_XMIN_YMAX [__last_bit_right, __last_bit_top];
				}
				if (_right_bit_x != AlphaArraySizeMask && _bottom_bit_y != AlphaArraySizeMask) {
					totalAlphaArray [_right_array_x, _bottom_array_y] |= MASK_XMIN_YMIN [__last_bit_right, __last_bit_bottom];
				}

				for (i = _c_array_l, imax = _c_array_r + 1; i < imax; i++) {
					totalAlphaArray [i, _top_array_y] |= MASK_XMIN_YMIN [AlphaArraySizeMask, __last_bit_top];
				}
				for (i = _c_array_l, imax = _c_array_r + 1; i < imax; i++) {
					totalAlphaArray [i, _bottom_array_y] |= MASK_XMIN_YMAX [AlphaArraySizeMask, __last_bit_bottom];
				}
				for (j = _c_array_t, jmax = _c_array_b + 1; j < jmax; j++) {
					totalAlphaArray [_left_array_x, j] |= MASK_XMIN_YMAX [__last_bit_left, AlphaArraySizeMask];
				}
				for (j = _c_array_t, jmax = _c_array_b + 1; j < jmax; j++) {
					totalAlphaArray [_right_array_x, j] |= MASK_XMAX_YMAX [__last_bit_right, AlphaArraySizeMask];
				}
			}
		}

		private void __UpdateTransparentTop (FogTransparentRange rangeData,
		                                     int view_lt_x, int view_lt_y,
		                                     int view_left, int view_right, int view_top, int view_bottom)
		{
			int yMin = __Max (view_top, 0);
			int yMax = __Min (view_bottom, rangeData.t + 1);
			if (yMin < yMax) {
				int xMin = __Max (view_left, rangeData.l);
				int xMax = __Min (view_right, rangeData.r + 1);
				for (int i = xMin, j = 0; i < xMax; i++) {
					for (j = yMin; j < yMax; j++) {
						if (rangeData.array [i, j]) {
							__SetTransparent (view_lt_x + i, view_lt_y + j);
						}
					}
				}
			}
		}

		private void __UpdateTransparentBottom (FogTransparentRange rangeData,
		                                        int view_lt_x, int view_lt_y,
		                                        int view_left, int view_right, int view_top, int view_bottom)
		{
			int yMin = __Max (view_top, rangeData.b);
			int yMax = __Min (view_bottom, rangeData.size);
			if (yMin < yMax) {
				int xMin = __Max (view_left, rangeData.l);
				int xMax = __Min (view_right, rangeData.r + 1);
				for (int i = xMin, j = 0; i < xMax; i++) {
					for (j = yMin; j < yMax; j++) {
						if (rangeData.array [i, j]) {
							__SetTransparent (view_lt_x + i, view_lt_y + j);
						}
					}
				}
			}
		}

		private void __UpdateTransparentLeft (FogTransparentRange rangeData,
		                                      int view_lt_x, int view_lt_y,
		                                      int view_left, int view_right, int view_top, int view_bottom)
		{
			int yMin = __Max (view_top, rangeData.t);
			int yMax = __Min (view_bottom, rangeData.b + 1);
			if (yMin < yMax) {
				int xMin = __Max (view_left, 0);
				int xMax = __Min (view_right, rangeData.l + 1);
				for (int i = xMin, j = 0; i < xMax; i++) {
					for (j = yMin; j < yMax; j++) {
						if (rangeData.array [i, j]) {
							__SetTransparent (view_lt_x + i, view_lt_y + j);
						}
					}
				}
			}
		}

		private void __UpdateTransparentRight (FogTransparentRange rangeData,
		                                       int view_lt_x, int view_lt_y,
		                                       int view_left, int view_right, int view_top, int view_bottom)
		{
			int yMin = __Max (view_top, rangeData.t);
			int yMax = __Min (view_bottom, rangeData.b + 1);
			if (yMin < yMax) {
				int xMin = __Max (view_left, rangeData.r);
				int xMax = __Min (view_right, rangeData.size);
				for (int i = xMin, j = 0; i < xMax; i++) {
					for (j = yMin; j < yMax; j++) {
						if (rangeData.array [i, j]) {
							__SetTransparent (view_lt_x + i, view_lt_y + j);
						}
					}
				}
			}
		}

		private int __Max (int val1, int val2)
		{
			return val1 > val2 ? val1 : val2;
		}

		private int __Min (int val1, int val2)
		{
			return val1 > val2 ? val2 : val1;
		}

		#endregion

		public void Release ()
		{
			totalAlphaArray = null;
			for (int i = 0, imax = __fogRangeList.Count; i < imax; i++) {
				__fogRangeList [i].Release ();
			}
			__fogRangeList.Clear ();
			__fogRangeTable.Clear ();
			__fogRangeCache = null;
		}


		#if UNITY_EDITOR

		public UnityEngine.Texture2D SaveTexture ()
		{
			UnityEngine.Texture2D t = new UnityEngine.Texture2D (w, h, UnityEngine.TextureFormat.RGB24, false, true);
			for (int i = 0; i < w; i++) {
				for (int j = 0; j < h; j++) {
					if (IsTransparent (i, j)) {
						t.SetPixel (i, j, UnityEngine.Color.white);
					} else {
						t.SetPixel (i, j, UnityEngine.Color.blue);
					}
				}
			}

			byte[] bytes = t.EncodeToPNG ();
			global::System.IO.File.WriteAllBytes (UnityEngine.Application.dataPath + "/fogMap.png", bytes);
			UnityEngine.GameObject.DestroyImmediate (t);

			return t;
		}

		#endif
	}
}
