using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Json;
using NeuroSDKCsharp.Websocket;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public class RedrawWithIvoryDiceAction : NeuroAction<string>
	{
		public override string Name => "redraw";

		protected override string Description => "Redraw the three floor plans for the draft at the cost of 1 ivory dice.";

		protected override JsonSchema Schema => null;

		protected override Task Execute(string resultData)
		{
			DraftManager.StartRedraw();

			Core.actionToTake = "REDRAW_IVORY_DICE";

			return Task.CompletedTask;
		}

		protected override ExecutionResult Validate(ActionData actionData, out string resultData)
		{
			resultData = "";

			return ExecutionResult.Success();
		}
	}
}
