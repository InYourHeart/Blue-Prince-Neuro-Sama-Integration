using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using Il2CppSystem.Runtime.Remoting.Messaging;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Json;
using NeuroSDKCsharp.Websocket;
using Newtonsoft.Json.Linq;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Darts
{
	public class ChooseDartboardStageAnswerAction : NeuroAction<int?>
	{
		public override string Name => "choose_dartboard_stage_answer";

		protected override string Description => "Pick a number from 1 to 20 as the answer for the current stage of the dartboard puzzle.";

		protected override JsonSchema Schema => new()
		{
			Type = JsonSchemaType.Object,
			Required = ["answer"],
			Properties = new Dictionary<string, JsonSchema>()
			{
				["answer"] = QJS.Enum([1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20])
			}
		};

		protected override Task Execute(int? resultData)
		{
			Core.actionToTake = Name;
			Core.dartsAnswer = resultData.Value;

			return Task.CompletedTask;
		}

		protected override ExecutionResult Validate(ActionData actionData, out int? answer)
		{
			answer = actionData.Data?["answer"]?.Value<int>();

			if (answer == null || answer < 1 || answer > 20)
			{
				return ExecutionResult.Failure("You did not send a valid answer. Try picking a number from 1 to 20.");
			}

			return ExecutionResult.Success();
		}
	}
}
