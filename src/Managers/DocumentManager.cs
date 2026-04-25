using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers
{
	public class DocumentManager
	{
		public static void SetOpenPageContents(GameObject pageObject)
		{
			string pageKey = pageObject.transform.parent.name + " | " + pageObject.name;
			string pageText;

			Melon<Core>.Logger.Msg("Page Key: " + pageKey);

			switch (pageKey)
			{
				case ("ENTRANCE LETTER - doc | Page 1"):
					pageText = FsmUtil.GetTextMeshProText(pageObject, "Text", "");

					Melon<Core>.Logger.Msg(pageText);

					pageText = pageText.Replace("Simon", "Neuro")
						.Replace("boy","AI")
						.Replace("heir.","heiress.");

					FsmUtil.SetTextMeshProText(pageObject, "Text", pageText);

					break;
				case ("INHERITRANCE LETTER - doc | Page 1"):
					pageText = FsmUtil.GetTextMeshProText(pageObject, "Text", "");

					pageText = pageText.Replace("Baron,", "Baroness,")
						.Replace("Baron of", "Baron and Baroness of")
						.Replace("Baron doesn't", "Baroness doesn't")
						.Replace("He could","She could");

					FsmUtil.SetTextMeshProText(pageObject, "Text", pageText);
					break;
				case ("PAGE TURN | 1"):
					string baseSigPath = "Checkoutcard|cullable card|Checkout Card FLips (1)|Checkout Parent|Checkout Card (1)|";

					FsmUtil.SetTextMeshProText(pageObject, baseSigPath + "ROW 1|Simon Sig (1)","Neuro S. 987");
					FsmUtil.SetTextMeshProText(pageObject, baseSigPath + "ROW 2|Simon Sig (2)","Neuro S. 987");
					FsmUtil.SetTextMeshProText(pageObject, baseSigPath + "ROW 3|Simon Sig (3)","Neuro S. 987");
					FsmUtil.SetTextMeshProText(pageObject, "Checkoutcard|cullable card|title (7)", "Neuro S. 987");
					break;
				case ("DOCUMENTS | ALLOWANCE - doc"):
					pageText = FsmUtil.GetTextMeshProText(pageObject, "Page 1|Text", "");

					pageText = pageText.Replace("Master Simon,", "Mistress Neuro,");

					FsmUtil.SetTextMeshProText(pageObject, "Page 1|Text", pageText);
					break;
			}
		}
	}
}