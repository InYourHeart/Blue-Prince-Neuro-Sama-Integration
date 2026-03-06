using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Drafting_Abilities.Redraw;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class CrownAction : RedrawAction
	{
		public override string GAME_OBJECT_NAME => "REDRAW CROWN";

		public override string Name => "redraw_crown";

		protected override string Description => "Redraw the three floor plans for the draft using the Paper Crown.";
	}
}
