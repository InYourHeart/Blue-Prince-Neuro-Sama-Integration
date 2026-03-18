using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Drafting_Abilities.Misc;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class CronographAction : MiscAction
	{
		public override string GAME_OBJECT_NAME => "CHRONOGRAPH";

		public override string Name => "cronograph";

		protected override string Description => "Reverse the last redraw using the Chronograph's drafting ability.";
	}
}
