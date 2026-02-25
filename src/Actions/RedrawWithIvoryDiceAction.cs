namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class RedrawWithIvoryDiceAction : RedrawAction
	{
		public override string GAME_OBJECT_NAME => "DICE (slot x)";

		public override string Name => "redraw_ivory_dice";

		protected override string Description => "Redraw the three floor plans for the draft at the cost of 1 ivory dice.";
	}
}
