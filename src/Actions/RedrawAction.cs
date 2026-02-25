using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Json;
using NeuroSDKCsharp.Websocket;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public abstract class RedrawAction : NeuroAction<string>
	{
		public static string REDRAW_FLAG = "REDRAW_ACTION";
		public abstract string GAME_OBJECT_NAME { get; }

		protected override JsonSchema Schema => null;

		protected override Task Execute(string resultData)
		{
			Core.actionToTake = REDRAW_FLAG + GAME_OBJECT_NAME;

			return Task.CompletedTask;
		}

		protected override ExecutionResult Validate(ActionData actionData, out string resultData)
		{
			resultData = "";

			return ExecutionResult.Success();
		}
	}
}
