using Blue_Prince_Neuro_Sama_Integration_Mod.Managers;
using HarmonyLib;
using Il2Cpp;
using Il2CppBluePrince;
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
        private static KeyCode freezeToggleKey;

        private static bool speeding;
        private static float baseTimeScale;

        public volatile static string actionToTake = "";

        public override void OnEarlyInitializeMelon()
        {
            SdkSetup.Initialize("Blue Prince", "");

            freezeToggleKey = KeyCode.Space;
            //TODO Register actions
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
        private static class PickRoomFromSlotPatch
        {
            private static void Postfix(ref RoomCard __result)
            {
                DraftManager.AddPickedRoom(__result);
            }
        }

        /// <summary>
        /// Speeding stuff
        /// </summary>

        public override void OnLateUpdate()
        {
            if (Input.GetKeyDown(freezeToggleKey))
            {
                ToggleFreeze();
            }
        }

        public static void DrawSpeedingText()
        {
            GUI.Label(new Rect(20, 20, 1000, 200), "<b><color=cyan><size=100>Speeding</size></color></b>");
        }

        private static void ToggleFreeze()
        {
            speeding = !speeding;

            if (speeding)
            {
                Melon<Core>.Logger.Msg("Speeding");

                MelonEvents.OnGUI.Subscribe(DrawSpeedingText, 100); // Register the 'Speeding' label
                baseTimeScale = Time.timeScale; // Save the original time scale before freezing
                Time.timeScale = 5;
            }
            else
            {
                Melon<Core>.Logger.Msg("Unspeeding");

                MelonEvents.OnGUI.Unsubscribe(DrawSpeedingText); // Unregister the 'Speeding' label
                Time.timeScale = baseTimeScale; // Reset the time scale to what it was before we froze the time
            }
        }

        public override void OnDeinitializeMelon()
        {
            if (speeding)
            {
                ToggleFreeze(); // Unfreeze the game in case the melon gets unregistered
            }

            NeuroActionHandler.OnApplicationQuitAsync();
        }
    }
}