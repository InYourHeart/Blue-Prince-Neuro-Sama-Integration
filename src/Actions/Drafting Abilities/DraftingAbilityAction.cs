using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Json;
using NeuroSDKCsharp.Websocket;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public abstract class DraftingAbilityAction : NeuroAction<string>
	{
		public static string DRAFTING_ABILITY_FLAG = "DRAFTING_ABILITY_ACTION";
		public abstract string GAME_OBJECT_NAME { get; }

		protected override JsonSchema Schema => null;

		protected override Task Execute(string resultData)
		{
			Core.actionToTake = DRAFTING_ABILITY_FLAG + GAME_OBJECT_NAME;

			return Task.CompletedTask;
		}

		protected override ExecutionResult Validate(ActionData actionData, out string resultData)
		{
			resultData = "";

			return ExecutionResult.Success();
		}
	}
}
