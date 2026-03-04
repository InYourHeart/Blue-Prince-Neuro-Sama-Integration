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

			Melon<Core>.Logger.Msg("c - " + Core.actionToTake);

			return Task.CompletedTask;
		}

		protected override ExecutionResult Validate(ActionData actionData, out string resultData)
		{
			Melon<Core>.Logger.Msg("b");

			resultData = "";

			return ExecutionResult.Success();
		}
	}
}
