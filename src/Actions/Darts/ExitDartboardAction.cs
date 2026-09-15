using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Json;
using NeuroSDKCsharp.Websocket;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Darts
{
	public class ExitDartboardAction : NeuroAction<string>
	{
		public override string Name => "exit_dartboard";

		protected override string Description => "Stop solving the dartboard puzzle for now.";

		protected override JsonSchema Schema => null;

		protected override Task Execute(string resultData)
		{
			Core.actionToTake = Name;

			return Task.CompletedTask;
		}

		protected override ExecutionResult Validate(ActionData actionData, out string resultData)
		{
			resultData = "";

			return ExecutionResult.Success();
		}
	}
}
