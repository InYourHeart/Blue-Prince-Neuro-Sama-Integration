using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
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
                        GameObject gameObject = GameObject.Find(actionToTake);
                        PlayMakerFSM fsm = gameObject.GetComponent<PlayMakerFSM>();

                        fsm.SendEvent("click");
                        actionToTake = "";
                    }

                    UpdatePlayerLocationContext();

                    if (DraftManager.currentRoomSlot == 3)
                    {
                        GameObject gameObject = GameObject.Find("DRAFT UI");
                        PlayMakerFSM fsm = gameObject.GetComponent<PlayMakerFSM>();

                        foreach (FsmState state in fsm.FsmStates)
                        {
                            if (state != null && state.active && state.name.Equals("Animate In 2"))
                            {
                                //Checking for the third room plan's slide in animation to finish before sending the drafting context.
                                //
                                //Allows humans to see what is going on and prevents some race conditions that might
                                // lock the player cam if the room plan was picked at the first possible moment
                                if (!state.Actions[20].Active)
                                {
                                    DraftManager.SendDraftingContext();
                                }
                            }
                        }
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

        [HarmonyPatch(typeof(RoomDraftContext), "PickRoomFromSlot", new Type[] { typeof(int),typeof(RoomCostType),typeof(bool),typeof(bool),typeof(CardFilterDelegate) })]
        private static class NormalDraftPatch
        {
            private static void Postfix(ref RoomCard __result)
            {
                DraftManager.AddPickedRoom(__result);
            }
        }

        [HarmonyPatch(typeof(OuterDraftManager), "PickCurrentRoom")]
        private static class OuterDraftPatch
        {
            private static void Postfix(ref GameObject __result)
            {
                DraftManager.AddPickedRoom(__result);
            }
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