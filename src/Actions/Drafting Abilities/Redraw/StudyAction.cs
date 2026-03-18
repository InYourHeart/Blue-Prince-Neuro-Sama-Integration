using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Drafting_Abilities.Redraw;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class StudyAction : RedrawAction
	{
		public override string GAME_OBJECT_NAME => "REDRAW STUDY";

		public override string Name => "redraw_study";

		protected override string Description => "Redraw the three floor plans for the draft using the Study's drafting ability. It costs 1 gem.";
	}
}
