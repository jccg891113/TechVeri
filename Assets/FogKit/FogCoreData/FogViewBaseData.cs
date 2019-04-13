using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BabelTime.GD.UI.BattleUISub.FogAbout
{
	public class FogViewBaseData
	{
		const bool TRANSPARENT = true;
		const bool OPAQUE = false;

		public int real_w;
		public int real_h;

		public int array_w;
		public int array_h;

		public int array_real_w;
		public int array_real_h;

		public ulong [,] array;

		public float view_delta_x;
		public float view_size_x;
		public float view_delta_y;
		public float view_size_y;

		public FogDataFixList fixList;

		public FogViewBaseData (int w, int h)
		{
			this.real_w = w;
			this.real_h = h;
			this.array_w = (w % FogBaseData.AlphaArraySize <= 1) ? (w / FogBaseData.AlphaArraySize) + 1 : (w / FogBaseData.AlphaArraySize) + 2;
			this.array_h = (h % FogBaseData.AlphaArraySize <= 1) ? (h / FogBaseData.AlphaArraySize) + 1 : (h / FogBaseData.AlphaArraySize) + 2;

			array_real_w = this.array_w * FogBaseData.AlphaArraySize;
			array_real_h = this.array_h * FogBaseData.AlphaArraySize;

			view_size_x = (float) real_w / array_real_w;
			view_size_y = (float) real_h / array_real_h;

			this.array = new ulong [array_w, array_h];

			fixList = new FogDataFixList ();
		}

		public void AfterResetVO (FogBaseData fogBaseData, float world_x, float world_y)
		{
			_AfterResetVO_2 (fogBaseData, world_x, world_y);
		}

		private void _AfterResetVO_2 (FogBaseData fogBaseData, float world_x, float world_y)
		{
			if (world_x < 0 || world_y < 0 || world_x >= (fogBaseData.w - real_w) || world_y >= (fogBaseData.h - real_h)) {
				return;
			}

			int _tmp_world_x = (int) world_x;
			int _tmp_world_y = (int) world_y;

			int world_begin_x = _tmp_world_x / FogBaseData.AlphaArraySize;
			int world_begin_y = _tmp_world_y / FogBaseData.AlphaArraySize;
			//int world_begin_delta_x = _tmp_world_x % FogBaseData.AlphaArraySize;
			//int world_begin_delta_y = _tmp_world_y % FogBaseData.AlphaArraySize;

			float world_begin_delta_x_f = world_x - world_begin_x * FogBaseData.AlphaArraySize;
			float world_begin_delta_y_f = world_y - world_begin_y * FogBaseData.AlphaArraySize;

			ulong _base_0x01 = 0x01;

			ulong _tmp_world_8x8, _tmp_view_8x8, _tmp_xor;
			int _index;
			int baseX, baseY;

			for (int i = 0; i < array_w; i++) {
				for (int j = 0; j < array_h; j++) {
					_tmp_world_8x8 = fogBaseData.totalAlphaArray [world_begin_x + i, world_begin_y + j];
					_tmp_view_8x8 = array [i, j];
					_tmp_xor = _tmp_world_8x8 ^ _tmp_view_8x8;
					if (_tmp_xor > 0) {
						_index = 0;
						baseX = i * FogBaseData.AlphaArraySize;
						baseY = j * FogBaseData.AlphaArraySize;
						while (_index < 64) {
							if ((_tmp_xor & _base_0x01) > 0) {
								fixList.Add (baseX + _index / FogBaseData.AlphaArraySize, baseY + _index % FogBaseData.AlphaArraySize, (_tmp_world_8x8 & (_base_0x01 << _index)) > 0 ? TRANSPARENT : OPAQUE);
							}
							_tmp_xor = _tmp_xor >> 1;
							_index++;
						}

					}
					array [i, j] = _tmp_world_8x8;
				}
			}

			//view_delta_x = (float) world_begin_delta_x / array_real_w;
			//view_delta_y = (float) world_begin_delta_y / array_real_h;
			view_delta_x = world_begin_delta_x_f / array_real_w;
			view_delta_y = world_begin_delta_y_f / array_real_h;

		}

		public void CleanFixList ()
		{
			fixList.Clear ();
		}
	}
}