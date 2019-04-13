using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BabelTime.GD.UI.BattleUISub.FogAbout;

public class Test : MonoBehaviour
{
	public Image image;
	public GameObject point;
	FogKit kit;
	Texture2D mask;

	/// <summary>
	/// 不透明颜色
	/// </summary>
	Color black = new Color (1, 0, 0);
	/// <summary>
	/// 透明颜色
	/// </summary>
	Color clear = new Color (0, 0, 0);

	int sizeHalfW = 250;
	int sizeHalfH = 200;

	private void Start ()
	{
		int mask_w = (int) (250 * 4);
		int mask_h = (int) (200 * 4);
		kit = new FogKit (1334, 750, sizeHalfW * 2, sizeHalfH * 2, mask_w, mask_h);
		mask = new Texture2D (mask_w, mask_h, TextureFormat.RGB24, false, true);
		for (int i = 0, j = 0; i < mask_w; i++) {
			for (j = 0; j < mask_h; j++) {
				mask.SetPixel (i, j, black);
			}
		}
		mask.Apply ();
		image.material.SetTexture ("_FogTex", mask);
	}

	int pos_x_old = int.MaxValue;
	int pos_y_old = int.MaxValue;
	int pos_view_x_old = int.MaxValue;
	int pos_view_y_old = int.MaxValue;

	private void Update ()
	{
		//int pos_x = (int) (point.transform.localPosition.x) + 667;
		//int pos_y = (int) (point.transform.localPosition.y) + 375;
		//int view_lt_x = (int) (image.rectTransform.localPosition.x) + 667 - sizeHalfW;
		//int view_lt_y = (int) (image.rectTransform.localPosition.y) + 375 - sizeHalfH;
		//if ((pos_x_old == pos_x && pos_y_old == pos_y) &&
		//	(pos_view_x_old == view_lt_x && pos_view_y_old == view_lt_y)) {
		//	return;
		//} else {
		//	pos_x_old = pos_x;
		//	pos_y_old = pos_y;
		//	pos_view_x_old = view_lt_x;
		//	pos_view_y_old = view_lt_y;
		//}

		///// baseData
		//System.DateTime clock = System.DateTime.Now;
		//baseData.UpdateTransparent (pos_x, pos_y, 15);
		//var baseDataTime = System.DateTime.Now - clock;

		///// viewData
		//clock = System.DateTime.Now;
		////viewData.AfterResetVO (baseData, view_lt_x, view_lt_y);
		//viewData2.AfterResetVO (baseData, view_lt_x, view_lt_y);
		//var viewDataTime = System.DateTime.Now - clock;

		///// maskData
		//clock = System.DateTime.Now;
		////maskData.AfterViewDataChange (viewData);
		//maskData.AfterViewDataChange (viewData2);
		//var maskDataTime = System.DateTime.Now - clock;

		///// for to set
		//clock = System.DateTime.Now;
		////for (int i = 0, imax = maskData._fixList.Count; i < imax; i++) {
		////	mask.SetPixel (maskData._fixList [i].x, maskData._fixList [i].y, maskData._fixList [i].transparent ? clear : black);
		////}
		//foreach (var item in maskData.fixGroup.fogMaskFixGroupX) {
		//	foreach (var groupX in item.Value.fogMaskFixGroupY) {
		//		mask.SetPixels (item.Key, groupX.beginY, 1, groupX.colorArray.Count, groupX.colorArray.ToArray ());
		//	}
		//}
		//var maskApplyForeachTime = System.DateTime.Now - clock;

		///// apply
		//clock = System.DateTime.Now;
		//mask.Apply ();
		//var maskApplyTime = System.DateTime.Now - clock;

		//Debug.LogFormat ("BASE:{0}ms, VIEW:{1}ms, MASK:{2}ms, TEX:{3}ms, TEXFOR:{4}ms",
		//baseDataTime.TotalMilliseconds,
		//viewDataTime.TotalMilliseconds,
		//maskDataTime.TotalMilliseconds,
		//maskApplyTime.TotalMilliseconds,
		//maskApplyForeachTime.TotalMilliseconds);

		//CleanData ();

		//image.material.SetColor ("_SizeInfo", new Color (viewData2.view_delta_x, viewData2.view_delta_y, viewData2.view_size_x, viewData2.view_size_y));
		TestUpdate ();
	}

	private void TestUpdate ()
	{
		int pos_x = (int) (point.transform.localPosition.x) + 667;
		int pos_y = (int) (point.transform.localPosition.y) + 375;
		//int view_lt_x = (int) (image.rectTransform.localPosition.x) + 667 - sizeHalfW;
		//int view_lt_y = (int) (image.rectTransform.localPosition.y) + 375 - sizeHalfH;
		float view_lt_x = (image.rectTransform.localPosition.x) + 667 - sizeHalfW;
		float view_lt_y = (image.rectTransform.localPosition.y) + 375 - sizeHalfH;
		//if ((pos_x_old == pos_x && pos_y_old == pos_y) &&
		//	(pos_view_x_old == view_lt_x && pos_view_y_old == view_lt_y)) {
		//	return;
		//} else {
		//	pos_x_old = pos_x;
		//	pos_y_old = pos_y;
		//	pos_view_x_old = view_lt_x;
		//	pos_view_y_old = view_lt_y;
		//}

		System.DateTime clock = System.DateTime.Now;

		kit.UpdateRange (pos_x, pos_y, 15);
		kit.UpdateView (image.material, view_lt_x, view_lt_y);
		kit.RefreshView ();
		kit.RefreshMaskTexture2D (mask);

		kit.CleanData ();
		var maskApplyTime = System.DateTime.Now - clock;
		Debug.LogFormat ("Cost:{0}ms", maskApplyTime.TotalMilliseconds);
	}
}
