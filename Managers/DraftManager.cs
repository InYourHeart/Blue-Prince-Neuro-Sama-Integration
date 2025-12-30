using Blue_Prince_Neuro_Sama_Integration_Mod.Actions;
using Il2Cpp;
using Il2CppHutongGames.PlayMaker;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Messages.Outgoing;
using UnityEngine;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.Managers
{
    public class DraftManager
    {
        private static int pickedRoomSlots = 0;
        private volatile static string draftingContext = "";

        public static Rooms.Room firstRoom;
        public static Rooms.Room secondRoom;
        public static Rooms.Room thirdRoom;

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

            Rooms.Room newRoomObject = new Rooms.Room(newRoom);

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

            UpdateDraftingContext(newRoomObject);

            if (pickedRoomSlots == 3)
            {
                new Thread(DraftManager.RegisterDraftActionWindow).Start();

                pickedRoomSlots = 0;
            }
        }

        private static void UpdateDraftingContext(Rooms.Room newRoomObject)
        {
            if (pickedRoomSlots == 1)
            {
                draftingContext += "The following three rooms have been pulled from the draft pool and may chosen from:\n";
            }

            draftingContext += pickedRoomSlots + ". " + newRoomObject.name + ". " + newRoomObject.rarity + " rarity.";
            if (newRoomObject.effect != "")
            {
                draftingContext += " It has the following effect: " + newRoomObject.effect + ".";
            }
            draftingContext += " It is " + newRoomObject.types + ".";
            if (newRoomObject.cost > 0)
            {
                draftingContext += "It costs " + newRoomObject.cost + " gems to draft.";
            }

            draftingContext += "\n";

            if (pickedRoomSlots == 3)
            {
                draftingContext += InventoryManager.GetInventoryContext();
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
