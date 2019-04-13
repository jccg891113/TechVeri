using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BabelTime.GD.UI.BattleUISub.FogAbout
{
	public class FogMaskFixGroupX
	{
		#region Pool
		private static Queue<FogMaskFixGroupX> _poolX = new Queue<FogMaskFixGroupX> ();
		public static FogMaskFixGroupX Get (int x)
		{
			if (_poolX.Count > 0) {
				FogMaskFixGroupX tmp = _poolX.Dequeue ();
				tmp.SetX (x);
				return tmp;
			} else {
				return new FogMaskFixGroupX (x);
			}
		}
		public static void Recover (FogMaskFixGroupX item)
		{
			item.Clear ();
			_poolX.Enqueue (item);
		}
		#endregion

		public int X;
		public List<FogMaskFixGroupY> fogMaskFixGroupY;

		private FogMaskFixGroupX (int x)
		{
			X = x;
			fogMaskFixGroupY = new List<FogMaskFixGroupY> ();
		}

		private void SetX (int x)
		{
			this.X = x;
		}

		public void Add (int y, Color color)
		{
			bool addSuccess = false;
			FogMaskFixGroupY ptr = null;
			for (int i = 0, imax = fogMaskFixGroupY.Count; i < imax; i++) {
				ptr = fogMaskFixGroupY [i];
				if (ptr.endY + 1 == y) {
					ptr.endY += 1;
					ptr.AddAfter (color);
					addSuccess = true;
					break;
				}
			}
			if (!addSuccess) {
				ptr = FogMaskFixGroupY.Get (y, color);
				fogMaskFixGroupY.Add (ptr);
			}
		}

		public void Combo ()
		{
			if (fogMaskFixGroupY.Count >= 2) {
				fogMaskFixGroupY.Sort ((l, r) => {
					return l.beginY.CompareTo (r.beginY);
				});
				FogMaskFixGroupY beforePtr = null;
				FogMaskFixGroupY afterPtr = null;
				for (int i = fogMaskFixGroupY.Count - 1; i > 0; i--) {
					beforePtr = fogMaskFixGroupY [i - 1];
					afterPtr = fogMaskFixGroupY [i];
					/// 收尾相连则合并之
					if (beforePtr.endY + 1 == afterPtr.beginY) {
						/// 扩展before的数组
						beforePtr.colorArray.AddRange (afterPtr.colorArray);
						beforePtr.endY = afterPtr.endY;
						/// 删除after
						FogMaskFixGroupY.Recover (afterPtr);
						fogMaskFixGroupY.RemoveAt (i);
					}
				}
			}
		}

		public void Clear ()
		{
			for (int i = fogMaskFixGroupY.Count - 1; i >= 0; i--) {
				FogMaskFixGroupY.Recover (fogMaskFixGroupY [i]);
			}
			fogMaskFixGroupY.Clear ();
		}
	}
}