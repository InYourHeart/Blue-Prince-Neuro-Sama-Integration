using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Drafting_Abilities.Redraw;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class DrawingAction : RedrawAction
	{
		public override string GAME_OBJECT_NAME => "REDRAW DRAWING";

		public override string Name => "redraw_drawing";

		protected override string Description => "Redraw the three floor plans for the draft using the Drawing Room's drafting ability.";
	}
}
