﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BabelTime.GD.UI.BattleUISub.FogAbout
{
	public class FogMaskFixGroup
	{
		public Dictionary<int, FogMaskFixGroupX> fogMaskFixGroupX;

		public FogMaskFixGroupX [] cache;

		const int CACHENUM = 8;

		public FogMaskFixGroup ()
		{
			fogMaskFixGroupX = new Dictionary<int, FogMaskFixGroupX> ();

			cache = new FogMaskFixGroupX [CACHENUM];
		}

		public void Add (int x, int y, Color color)
		{
			int cacheID = x % CACHENUM;
			FogMaskFixGroupX ptr = cache [cacheID];
			if (ptr != null && ptr.X == x) {
				ptr.Add (y, color);
			} else {
				if (!fogMaskFixGroupX.ContainsKey (x)) {
					ptr = FogMaskFixGroupX.Get (x);
					fogMaskFixGroupX [x] = ptr;
				} else {
					ptr = fogMaskFixGroupX [x];
				}
				ptr.Add (y, color);
				cache [cacheID] = ptr;
			}
		}

		public void DataCombo ()
		{
			foreach (var item in fogMaskFixGroupX) {
				item.Value.Combo ();
			}
		}

		public void Clear ()
		{
			foreach (var item in fogMaskFixGroupX) {
				FogMaskFixGroupX.Recover (item.Value);
			}
			fogMaskFixGroupX.Clear ();
			for (int i = 0; i < CACHENUM; i++) {
				cache [i] = null;
			}
		}
	}
}