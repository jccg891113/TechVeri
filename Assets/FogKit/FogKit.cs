using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BabelTime.GD.UI.BattleUISub.FogAbout
{
	public class FogKit
	{
		FogBaseData baseData;
		FogViewBaseData viewData2;
		FogMaskData maskData;

		public FogKit (int world_w, int world_h, int view_w, int view_h, int mask_w, int mask_h)
		{
			baseData = new FogBaseData (world_w, world_h);
			viewData2 = new FogViewBaseData (view_w, view_h);
			maskData = new FogMaskData (viewData2, mask_w, mask_h);
		}

		public void UpdateRange (int x, int y, int r)
		{
			baseData.UpdateTransparent (x, y, r);
		}

		public void UpdateView (Material viewImageMaterial, float view_lt_x, float view_lt_y)
		{
			viewData2.AfterResetVO (baseData, view_lt_x, view_lt_y);
			viewImageMaterial.SetColor ("_SizeInfo", new Color (viewData2.view_delta_x, viewData2.view_delta_y, viewData2.view_size_x, viewData2.view_size_y));
		}

		public void RefreshView ()
		{
			maskData.AfterViewDataChange (viewData2);
		}

		public void RefreshMaskTexture2D (Texture2D maskPic)
		{
			foreach (var item in maskData.fixGroup.fogMaskFixGroupX) {
				foreach (var groupX in item.Value.fogMaskFixGroupY) {
					maskPic.SetPixels (item.Key, groupX.beginY, 1, groupX.colorArray.Count, groupX.colorArray.ToArray ());
				}
			}
			maskPic.Apply ();
		}

		public void CleanData ()
		{
			baseData.CleanFixList ();
			viewData2.CleanFixList ();
			maskData.CleanFixList ();
		}
	}
}