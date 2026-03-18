using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Drafting_Abilities.Redraw;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class ClassAction : RedrawAction
	{
		public override string GAME_OBJECT_NAME => "REDRAW CLASS";

		public override string Name => "redraw_class";

		protected override string Description => "Redraw the three floor plans using the Classroom's drafting ability.";
	}
}
