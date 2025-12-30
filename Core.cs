using Blue_Prince_Neuro_Sama_Integration_Mod.Managers;
using HarmonyLib;
using Il2Cpp;
using Il2CppBluePrince;
using Il2CppToolBuddy.ThirdParty.VectorGraphics;
using MelonLoader;
using NeuroSDKCsharp;
using NeuroSDKCsharp.Actions;
using UnityEngine;

[assembly: MelonInfo(typeof(Blue_Prince_Neuro_Sama_Integration_Mod.Core), "Blue Prince Neuro-Sama Integration Mod", "1.0.0", "InYourHeart", null)]
[assembly: MelonGame("Dogubomb", "BLUE PRINCE")]

namespace Blue_Prince_Neuro_Sama_Integration_Mod
{
    public class Core : MelonMod
    {
        public volatile static string actionToTake = "";

        public override void OnEarlyInitializeMelon()
        {
            SdkSetup.Initialize("Blue Prince", "");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            Melon<Core>.Logger.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
            //TODO Transitions from Loading Screen -> Main Menu -> Mount Holly Estate -> Other Scenes
        }

        [HarmonyPatch(typeof(BluePrinceManager), "Update")]
        private static class OnUpdatePatch
        {
            private static void Postfix(BluePrinceManager __instance)
            {
                if (__instance != null) {
                    switch (actionToTake) {
                        case "":
                            break;
                        case "DRAFT PLAN 3":
                        case "DRAFT PLAN 2":
                        case "DRAFT PLAN 1":
                            GameObject gameObject = GameObject.Find(actionToTake);
                            PlayMakerFSM fsm = gameObject.GetComponent<PlayMakerFSM>();

                            fsm.SendEvent("click");
                            actionToTake = "";
                            break;
                    }
                }
            }
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