using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Json;
using NeuroSDKCsharp.Websocket;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
	public abstract class DraftingAbilityAction : NeuroAction<string>
	{
		public abstract string DRAFTING_ABILITY_TYPE { get; }
		public abstract string GAME_OBJECT_NAME { get; }

		protected override JsonSchema Schema => null;

		protected override Task Execute(string resultData)
		{
			Core.draftingAbilityToTake = this;

			return Task.CompletedTask;
		}

		protected override ExecutionResult Validate(ActionData actionData, out string resultData)
		{
			DraftManager.UnregisterActions();

			resultData = "";

			return ExecutionResult.Success();
		}
	}
}
