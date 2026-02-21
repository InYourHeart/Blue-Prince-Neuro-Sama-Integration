using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers
{
	public class CoatCheckManager
	{
		private static string CLAIM_TICKET_FSM = "Claim Ticket";
		private static string ITEM_INDEX_INT = "IndexNumber";

		private static int? CheckedInItemId(string slot)
		{
			return FsmUtil.GetFsmInt(CLAIM_TICKET_FSM + " " + slot, ITEM_INDEX_INT);
		}

		public static string GetCoatCheckContext(string slot)
		{
			string coatCheckContext = "\t* Checked in item: ";

			int coatCheckItemID = CheckedInItemId(slot) == null ? -1 : (int) CheckedInItemId(slot);

			if (coatCheckItemID != -1 && !InventoryManager.GetItemByID(coatCheckItemID).Equals(""))
			{
				coatCheckContext += InventoryManager.GetItemByID(coatCheckItemID);
			} else
			{
				coatCheckContext += "None";
			}

			return coatCheckContext + ";\n";
		}
	}
}
