using Blue_Prince_Neuro_Sama_Integration_Mod.Actions;
using Blue_Prince_Neuro_Sama_Integration_Mod.ObjectWrappers;
using Il2Cpp;
using Il2CppHutongGames.PlayMaker;
using Il2CppTMPro;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Messages.Outgoing;
using UnityEngine;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.Managers
{
    public class DraftManager
    {
        private static int pickedRoomSlots = 0;
        private volatile static string draftingContext = "";


        public static void AddPickedRoom(RoomCard newRoom)
        {
            if (newRoom == null)
            {
                return;
            }

            pickedRoomSlots++;

            if (pickedRoomSlots == 1)
            {
                draftingContext += "The following three rooms can now be drafted:\n";
            }

            draftingContext += pickedRoomSlots + ". " + RoomCardUtil.GetDraftingContext(newRoom);

            draftingContext += "\n";
            

            if (pickedRoomSlots == 3)
            {
                Melon<Core>.Logger.Msg($"Draft context: {draftingContext}");

                draftingContext += GetInventory();

                new Thread(DraftManager.RegisterDraftActionWindow).Start();

                pickedRoomSlots = 0;
            }
        }

        public static string GetInventory() {
            string goldAmount = "0";
            string keyAmount = "0";
            string gemAmount = "0";
            string stepAmount = "0";

            GameObject goldGameObject = GameObject.Find("Gold #");
            if (goldGameObject != null)
            {
                goldAmount = goldGameObject.GetComponent<TextMeshPro>().text;
            }

            GameObject keyGameObject = GameObject.Find("Key #");
            if (keyGameObject != null)
            {
                keyAmount = keyGameObject.GetComponent<TextMeshPro>().text;
            }

            GameObject gemGameObject = GameObject.Find("Gem #");
            if (gemGameObject != null)
            {
                gemAmount = gemGameObject.GetComponent<TextMeshPro>().text;
            }

            GameObject stepsGameObject = GameObject.Find("Steps #");
            if (stepsGameObject != null)
            {
                stepAmount = stepsGameObject.GetComponent<TextMeshPro>().text;
            }


            return "You currently have " + goldAmount + " gold, " + keyAmount + " key(s), " + gemAmount + " gem(s) and " + stepAmount + " steps.";
        }

        public static void RegisterDraftActionWindow()
        {
            GameObject draftUIGameObject = GameObject.Find("DRAFT UI");
            PlayMakerFSM draftUIfsm = draftUIGameObject.GetComponent<PlayMakerFSM>();

            FsmState state;

            do
            {
                state = draftUIfsm.fsm.activeState;
                FsmStateAction lastAnimationAction = state.Actions[20];

                if (!lastAnimationAction.Active)
                {
                    break;
                }

                Thread.Sleep(20);
            } while (true);

            ActionWindow.Create()
                .SetForce(5, "Three rooms have been drafted, please choose one.", "", true, ForcePriority.Low)
                .AddAction(new ChooseRoomAction())
                .SetContext(draftingContext, false)
                .Register();

            draftingContext = "";
        }
    }
}
