using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;
using HarmonyLib;
using Il2Cpp;
using Il2CppBluePrince;
using Il2CppHutongGames.PlayMaker;
using Il2CppHutongGames.PlayMaker.Actions;
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
                if (__instance != null) {
                    if (actionToTake.Contains("DRAFT PLAN"))
                    {
                        PlayMakerFSM fsm = FsmUtil.GetPlayMakerFSM(actionToTake);

                        fsm.SendEvent("click");
                        actionToTake = "";
					} else
					{
						switch (actionToTake)
						{
							case "REDRAW_IVORY_DICE":
								PlayMakerFSM fsm = FsmUtil.GetChildPlayMakerFSM("DICE (slot x)", "CLICK fsm");

								fsm.SendEvent("click");
								actionToTake = "";
								break;
							default:
								break;
						}
					}

					UpdatePlayerLocationContext();

					//Checking for the third room plan's slide in animation to finish before sending the drafting context.
					//
					//Allows humans to see what is going on and prevents some race conditions that might
					// lock the player cam if the room plan was picked at the first possible moment
					FsmState draftingStartState = FsmUtil.GetFsmState("DRAFT UI", 7);
					if (draftingStartState != null && draftingStartState.active && !DraftManager.isDrafting && !draftingStartState.Actions[20].Active) {
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
                }
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