namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class PickBerryAction : DraftingAbilityAction
	{
		public override string GAME_OBJECT_NAME => "PICK BERRY";

		public override string Name => "pick_berry";

		protected override string Description => "Draft a random floor plan disregarding rarity using the Blessing of the Berry Picker. It costs 1 wild berry.";
	}
}
