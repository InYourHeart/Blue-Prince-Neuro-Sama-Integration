using Blue_Prince_Neuro_Sama_Integration_Mod.src;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Json;
using NeuroSDKCsharp.Websocket;
using Newtonsoft.Json.Linq;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions
{
    public class ChooseRoomAction : NeuroAction<string>
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
            //TODO Validate Gems and what not?

            string choice = actionData.Data?["choice"]?.Value<string>();

            switch (choice)
            {
                case "1":
                case "2":
                case "3":
                    Room chosenRoom = DraftManager.draftedRooms[int.Parse(choice) - 1];

                    if (!choice.Equals("1"))
                    {
						bool canAffordRoomCost = true;
						string errorMessage = "";

                        if (DraftManager.IsHovelActive())
                        {
                            if ((chosenRoom.cost * 3) > int.Parse(InventoryManager.GetSteps()))
							{
								canAffordRoomCost = false;
								errorMessage = $"You do not have enough Steps to draft the {chosenRoom.name}. Try to pick a different option.";
                            }
                        }
                        else if (chosenRoom.cost > int.Parse(InventoryManager.GetGems()))
						{
							canAffordRoomCost = false;
							errorMessage = $"You do not have enough Gems to draft the {chosenRoom.name}. Try to pick a different option.";
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
                        Melon<Core>.Logger.Error($"Could not obtain the draft's target rank or tile while adding the picked room!");
                    } else if (!chosenRoom.isOuter)
                    {
                        GridFSMManager.Set((int)GridFSMManager.TargetRank(), (int)GridFSMManager.TargetTile(), chosenRoom);
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
