using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Drafting_Abilities.Redraw;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class InkwellAction : RedrawAction
	{
		public override string GAME_OBJECT_NAME => "INKWELL";

		public override string Name => "redraw_inkwell";

		protected override string Description => "Redraw the three floor plans for the draft using the Inkwell's power. It cost 1 star.";
	}
}
