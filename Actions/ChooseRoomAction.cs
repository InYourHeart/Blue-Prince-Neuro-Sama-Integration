using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Json;
using NeuroSDKCsharp.Websocket;
using Newtonsoft.Json.Linq;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.Actions
{
    public class ChooseRoomAction : NeuroAction<string>
    {
        public override string Name => "pick_draft_option";
        protected override string Description => "Choose one of the three rooms to draft.";

        protected override JsonSchema Schema => new()
        {
            Type = JsonSchemaType.Object,
            Required = ["choice"],
            Properties = new Dictionary<string, JsonSchema>()
            {
                ["choice"] = QJS.Enum(["1","2","3"])
            }
        };

        protected override Task Execute(string draftPlanObjectName)
        {
            Core.actionToTake = draftPlanObjectName;

            return Task.CompletedTask;
        }

        protected override ExecutionResult Validate(ActionData actionData, out string draftPlanObjectName)
        {
            //TODO Validate Gems and what not?

            string choice = actionData.Data?["choice"]?.Value<string>();

            switch (choice)
            {
                case "1":
                case "2":
                case "3":
                    draftPlanObjectName = "DRAFT PLAN " + choice; 
                    break;
                case null:
                    draftPlanObjectName = "";
                    return ExecutionResult.Failure("Action failed. Missing required parameter 'choice'.");
                default:
                    draftPlanObjectName = "";
                    return ExecutionResult.Failure("Action failed. Invalid parameter 'choice'.");

            }

            return ExecutionResult.Success();
        }
    }
}
