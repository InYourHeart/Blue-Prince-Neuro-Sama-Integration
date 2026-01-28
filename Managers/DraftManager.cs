using Blue_Prince_Neuro_Sama_Integration_Mod.Actions;
using Blue_Prince_Neuro_Sama_Integration_Mod.Rooms;
using Il2Cpp;
using Il2CppHutongGames.PlayMaker;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Messages.Outgoing;
using UnityEngine;
using Room = Blue_Prince_Neuro_Sama_Integration_Mod.Rooms.Room;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.Managers
{
    public class DraftManager
    {
        public static int pickedRoomSlots = 0;
        private volatile static string draftingContext = "";

        public static Room firstRoom;
        public static Room secondRoom;
        public static Room thirdRoom;

        public static int currentDraftRank;
        public static int currentDraftWing;

        public static void AddPickedRoom(GameObject newRoom)
        {
            if (newRoom == null)
            {
                return;
            }

            PlayMakerFSM prefabFsm = newRoom.GetComponent<PlayMakerFSM>();
            AddPickedRoom(prefabFsm);
        }

        public static void AddPickedRoom(RoomCard newRoom)
        {
            if (newRoom == null)
            {
                return;
            }

            PlayMakerFSM prefabFsm = GameObject.Find(newRoom.Template.Prefab.GetComponent<PlayMakerFSM>().name.ToUpper()).GetComponent<PlayMakerFSM>();
            AddPickedRoom(prefabFsm);
        }

        public static void AddPickedRoom(PlayMakerFSM newRoom)
        {
            pickedRoomSlots++;

            Room newRoomObject = new Room(newRoom);

            switch (pickedRoomSlots)
            {
                case 1:
                    firstRoom = newRoomObject;
                    break;
                case 2:
                    secondRoom = newRoomObject;
                    break;
                case 3:
                    thirdRoom = newRoomObject;
                    break;
            }
        }

        public static void SendDraftingContext()
        {
            UpdateDraftingContext(1);
            UpdateDraftingContext(2);
            UpdateDraftingContext(3);

            ActionWindow.Create()
                .SetForce(5, "Three rooms have been picked for the draft, please choose one.", "", true, ForcePriority.Low)
                .AddAction(new ChooseRoomAction())
                .SetContext(draftingContext, false)
                .Register();

            draftingContext = "";
            pickedRoomSlots = 0;
        }

        private static void UpdateDraftingContext(int slot)
        {
            Room newRoomObject;
            switch (slot)
            {
                case 1:
                    newRoomObject = firstRoom;
                    break;
                case 2:
                    newRoomObject = secondRoom;
                    break;
                case 3:
                    newRoomObject = thirdRoom;
                    break;
                default:
                    return;
            }

            if (slot == 1)
            {
                draftingContext += "The following three rooms have been pulled from the draft pool and may chosen from:\n";
            }

            draftingContext += slot + ". " + newRoomObject.name + ". " + newRoomObject.rarity + " rarity.";
            if (newRoomObject.effect != "")
            {
                draftingContext += " It has the following effect: " + newRoomObject.effect + ".";
            }
            draftingContext += " It is " + newRoomObject.types + ".";
            if (newRoomObject.cost > 0)
            {
                draftingContext += "It costs " + newRoomObject.cost + " gems to draft.";
            }

            FsmInt roomRotation = GameObject.Find("PLAN MANAGEMENT").GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("PLAN" + slot + " - ROTATION AMOUNT");
            int rotation = roomRotation.value / 90;

            Melon<Core>.Logger.Msg($"{roomRotation.value}");

            draftingContext += DoorLayout.GetDoorLayout(newRoomObject.name, rotation).GetDraftingContext();

            draftingContext += "\n";

            if (slot == 3)
            {
                draftingContext += InventoryManager.GetInventoryContext();

                GridFSMManager.Initialize();
            }
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
