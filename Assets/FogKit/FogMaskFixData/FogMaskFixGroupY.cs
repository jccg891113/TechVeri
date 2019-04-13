using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BabelTime.GD.UI.BattleUISub.FogAbout
{
	public class FogMaskFixGroupY
	{
		#region Pool
		private static Queue<FogMaskFixGroupY> _poolY = new Queue<FogMaskFixGroupY> ();
		public static FogMaskFixGroupY Get (int y, Color color)
		{
			if (_poolY.Count > 0) {
				FogMaskFixGroupY tmp = _poolY.Dequeue ();
				tmp.SetYColor (y, color);
				return tmp;
			} else {
				return new FogMaskFixGroupY (y, color);
			}
		}
		public static void Recover (FogMaskFixGroupY item)
		{
			item.colorArray.Clear ();
			_poolY.Enqueue (item);
		}
		#endregion

		public int beginY;
		public int endY;

		public List<Color> colorArray;

		private FogMaskFixGroupY (int y, Color color)
		{
			colorArray = new List<Color> ();
			SetYColor (y, color);
		}

		private void SetYColor (int y, Color color)
		{
			beginY = y;
			endY = y;
			colorArray.Clear ();
			colorArray.Add (color);
		}

		public void AddAfter (Color color)
		{
			colorArray.Add (color);
		}
	}
}