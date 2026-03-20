using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Json;
using NeuroSDKCsharp.Websocket;
using Newtonsoft.Json.Linq;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
    public class ChooseFloorPlanAction : NeuroAction<string>
    {
        public override string Name => "pick_draft_option";
		protected override string Description => "Choose one of the three floor plans to draft.";

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
			DraftManager.EndDraft();

			Core.actionToTake = draftPlanObjectName;

            return Task.CompletedTask;
        }

        protected override ExecutionResult Validate(ActionData actionData, out string draftPlanObjectName)
        {
			string choice = actionData.Data?["choice"]?.Value<string>();

            switch (choice)
            {
                case "1":
                case "2":
                case "3":
                    FloorPlan chosenFloorPlan = DraftManager.draftedFloorPlans[int.Parse(choice) - 1];

                    if (!choice.Equals("1"))
                    {
						bool canAffordRoomCost = true;
						string errorMessage = "";

                        if (DraftManager.IsHovelActive())
                        {
                            if ((chosenFloorPlan.cost * 3) > int.Parse(InventoryManager.GetSteps()))
							{
								canAffordRoomCost = false;
								errorMessage = $"You do not have enough Steps to draft the {chosenFloorPlan.name}. Try to pick a different option.";
                            }
                        }
                        else if (chosenFloorPlan.cost > int.Parse(InventoryManager.GetGems()))
						{
							canAffordRoomCost = false;
							errorMessage = $"You do not have enough Gems to draft the {chosenFloorPlan.name}. Try to pick a different option.";
                        }

						if (!canAffordRoomCost)
						{
							Core.actionToTake = "DRAFT PLAN " + choice; //Click the floor plan for the visual effect
							draftPlanObjectName = "";
							return ExecutionResult.Failure(errorMessage);
						}
					}

                    draftPlanObjectName = "DRAFT PLAN " + choice;

                    if (GridFSMManager.TargetRank() == null || GridFSMManager.TargetTile() == null)
                    {
                        Melon<Core>.Logger.Error($"Could not obtain the draft's target rank or tile while adding the picked floor plan!");
                    } else if (!chosenFloorPlan.isOuter)
                    {
                        GridFSMManager.Set((int)GridFSMManager.TargetRank(), (int)GridFSMManager.TargetTile(), chosenFloorPlan);
                    }
                        
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
