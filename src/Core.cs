using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;
using HarmonyLib;
using Il2Cpp;
using Il2CppBluePrince;
using Il2CppHutongGames.PlayMaker;
using Il2CppTMPro;
using MelonLoader;
using NeuroSDKCsharp;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Messages.Outgoing;
using UnityEngine;

[assembly: MelonInfo(typeof(Blue_Prince_Neuro_Sama_Integration_Mod.Core), "Blue Prince Neuro-Sama Integration Mod", "1.0.0", "InYourHeart", null)]
[assembly: MelonGame("Dogubomb", "BLUE PRINCE")]

namespace Blue_Prince_Neuro_Sama_Integration_Mod
{
    public class Core : MelonMod
    {
        public volatile static string actionToTake = "";
		public volatile static DraftingAbilityAction draftingAbilityToTake = null;
		public static string currentRoom = "None";

        public override void OnEarlyInitializeMelon()
        {
            SdkSetup.Initialize("Blue Prince", "");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            Melon<Core>.Logger.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");

            if (sceneName.Contains("Mount Holly")){
                Time.timeScale = 1;
                GridFSMManager.Initialize();
				//Send a modified transcript of the intro cutscene as the inital context because that's fun. We can actually explain mechanics later
				/*Context.Send("You received a phonograph recording in the mail which contained the following:" +
					"I, Herbert S. 987 of the Mount Holly estate at Reddington, do publish, and declare this instrument, my last will and testament, and hereby revoke all wills and codicils heretofore made by me." +
					"I give and bequeath to my grand niece Neuro S. 987, daughter of my dear nephew Vedal 987, all of my right, title and interest in and to the house and land which I own near Mount Holly." +
					"" +
					"The above provision and request is contingent on my aforementioned grand-niece discovering the location of the 46th room of my forty-five room estate." +
					"The location of the room has been kept a secret from all the staff and servants of the manor, but I am confident that any heiress worthy of the 987 legacy should have no trouble uncovering its whereabouts within a timely manner." +
					"Should my grandniece fail to uncover this room or provide proof of his discovery to the executors of my will, then this gift shall lapse." +
					"" +
					"In witness whereof, I have hereunto set my hand this 18th day of March, 1993." +
					"" +
					"Herbert S. 987");*/

				//Start of day instructions
				//TODO Include context on the initial state of the grid (Foundation means it can't be just a static string)
				Context.Send("A new day in the Mount Holly Estate has begun. " +
					"Your goal is to reach Room 46, hidden somewhere inside the Estate's House, within the timeframe of a single day." +
					"The House is composed of a 9 (South to North) by 5 (West to East) grid of Rooms. It starts off mostly empty, containing only the Antechamber on Rank 9, Tile 3, and the Entrance Hall (where you start each day) on Rank 1, Tile 3. It resets at the end of the day, though you will be able to try again starting the next morning." +
					"You can add new Rooms to the House through the Draft, which may be initialized by using one of the unblocked doors of the Room you are currently in." +
					"During a Draft you will be able to choose one of three Floor Plans, each corresponding to a specific Room. Various effects and abilities exist that may change the layout of the Floor Plans, replace them with new ones, or change the way the Draft works. Not all of these are necessarily beneficial, however." +
					"Furthermore, some Rooms and abilities have costs associated with them. You can obtain the necessary resources by picking up and using various items that will be scattered throughout the House. Certain Rooms provide better access to resources, while others are better at providing Doors that will open new paths in order to go deeper into the House." +
					"Finally, you have a Step counter that defines how far you can go. Every time you enter a Room, you will lose one step. If you reach 0 steps, you will be too tired to continue and will have to call it a day.");
            } else
            {
                Time.timeScale = 5;
            }
        }

        [HarmonyPatch(typeof(BluePrinceManager), "Update")]
        private static class OnUpdatePatch
        {
            private static void Postfix(BluePrinceManager __instance)
            {
				try
				{
					if (__instance == null) return;

					if (actionToTake != "") Melon<Core>.Logger.Msg("Action to take: " + actionToTake);

					if (actionToTake.Contains("DRAFT PLAN"))
					{
						PlayMakerFSM fsm = FsmUtil.GetPlayMakerFSM(actionToTake);

						fsm.SendEvent("click");

						actionToTake = "";
					}

					if (draftingAbilityToTake != null)
					{
						PlayMakerFSM fsm = FsmUtil.GetChildPlayMakerFSM(draftingAbilityToTake.GAME_OBJECT_NAME, "CLICK fsm");

						fsm.SendEvent("click");

						if (draftingAbilityToTake.DRAFTING_ABILITY_TYPE.Equals(new ClassAction().DRAFTING_ABILITY_TYPE)) { 
							//Idk if anything needs to be here
						}
						else if (draftingAbilityToTake.DRAFTING_ABILITY_TYPE.Equals(new DancerAction().DRAFTING_ABILITY_TYPE))
						{
							DraftManager.StartRotation();
						}

						draftingAbilityToTake = null;
					}

					UpdatePlayerLocationContext();

					//Checking for the third room plan's slide in animation to finish before sending the drafting context.
					//
					//Allows humans to see what is going on and prevents some race conditions that might
					// lock the player cam if the room plan was picked at the first possible moment
					FsmState draftingStartState = FsmUtil.GetFsmState("DRAFT UI", 7);
					if (draftingStartState != null && draftingStartState.active && !DraftManager.isDrafting && !draftingStartState.Actions[20].Active)
					{
						DraftManager.StartDraft();
					}

					FsmState draftingEndState = FsmUtil.GetFsmState("DRAFT UI", 0);
					if (draftingEndState != null && draftingEndState.active && DraftManager.isDrafting)
					{
						DraftManager.EndDraft();
					}

					FsmState redrawStartState = FsmUtil.GetFsmState("Draw New Floor Plans", 2);
					if (redrawStartState != null && redrawStartState.active && !DraftManager.isRedrawing)
					{
						DraftManager.StartRedraw();
					}

					FsmState redrawEndState = FsmUtil.GetFsmState("Draw New Floor Plans", 3);
					if (redrawEndState != null && redrawEndState.active && DraftManager.isRedrawing)
					{
						DraftManager.EndRedraw();
					}

					FsmState draftControllerState = FsmUtil.GetActiveFsmState("Draft Controller Selector");
					GameObject rotationErrorGameObject = GameObject.Find("ROTATION ERROR POPUP");
					if (DraftManager.isRotating)
					{
						if (!DraftManager.isFailedRotation && rotationErrorGameObject != null)
						{
							DraftManager.StartFailRotation();
						} else if (DraftManager.isFailedRotation && rotationErrorGameObject == null)
						{
							DraftManager.EndFailRotation();
						} else if (!DraftManager.isFailedRotation && draftControllerState.name.Contains("Enable inputs"))
						{
							DraftManager.EndRotation();
						}
					}
				}
				catch (Exception e)
				{
					Melon<Core>.Logger.Error(e.Message);
				}
            }
        }

		[HarmonyPatch(typeof(GameObject), "SetActive")]
		class Patch_SetActive
		{
			static void Postfix(GameObject __instance, bool value)
			{
				if (!value)
				{
					return;
				}

				GameObject parent = __instance;

				do
				{
					try
					{
						parent = parent.transform.parent.gameObject;

						if (parent != null && parent.name.Equals("DOCUMENTS"))
						{
							DocumentManager.SetOpenPageContents(__instance);
							break;
						}
					} catch (Exception)
					{
						return;
					}
				} while (true);
			}
		}

		private static void UpdatePlayerLocationContext()
        {
            GameObject roomTextObject = GameObject.Find("Room Text");
            if (roomTextObject == null) return;

            string contextMessage = "";
            TextMeshPro roomTextMesh = roomTextObject.GetComponent<TextMeshPro>();
            MeshRenderer roomTextRenderer = roomTextObject.GetComponent<MeshRenderer>();

            if (roomTextMesh == null 
                || roomTextMesh.text == null
                || roomTextMesh.text == currentRoom
                || (roomTextMesh.text == "" && currentRoom != "None")
                || (roomTextMesh.text != "" && currentRoom == "None")
                || !roomTextMesh.enabled) return;

            if (roomTextMesh.text == "" && currentRoom == "None")
            {
                contextMessage += "You have entered Entrance Hall (Rank 1, Tile 3)";
                currentRoom = "Entrance Hall";
            } else
            {
                contextMessage += "You have entered " + roomTextMesh.text;

                //Handle possible changes in the rank text (also encompasses the "The Grounds" subtitle and such)
                GameObject rankTextObject = GameObject.Find("Rank Text");
                if (rankTextObject != null)
                {
                    TextMeshPro rankTextMesh = rankTextObject.GetComponent<TextMeshPro>();

                    if (rankTextMesh != null && rankTextMesh.text != null && rankTextMesh.text != " ")
                    {
                        if (rankTextMesh.text.Contains("Rank"))
                        {
                            if (GridFSMManager.CurrentRank() == null || GridFSMManager.CurrentTile() == null)
                            {
                                contextMessage += " (Rank 1, Tile 3)";
                            }
                            else
                            {
                                contextMessage += " (Rank " + GridFSMManager.CurrentRank() + ", Tile " + GridFSMManager.CurrentTile() + ")";
                            }
                        }
                    }
                }

                contextMessage += " from " + currentRoom;

                currentRoom = roomTextMesh.text;
            }


            Context.Send(contextMessage);
        }

        /// <summary>
        /// Speeding stuff
        /// </summary>
        public override void OnDeinitializeMelon()
        {
            NeuroActionHandler.OnApplicationQuit();
        }
    }
}