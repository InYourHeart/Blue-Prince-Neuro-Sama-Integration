using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Drafting_Abilities.Rotation;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class DancerAction : RotationAction
	{
		public override string GAME_OBJECT_NAME => "DANCER";

		public override string Name => "dancer";

		protected override string Description => "Rotate the floorplans clockwise using the Dancer's power. It costs 1 gem.";
	}
}
