namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class RedrawStudyAction : RedrawAction
	{
		public override string GAME_OBJECT_NAME => "REDRAW STUDY";

		public override string Name => "redraw_study";

		protected override string Description => "Redraw the three floor plans for the draft using the Study's effect. It costs 1 gem.";
	}
}
