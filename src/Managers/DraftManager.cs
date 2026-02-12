using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms;
using Il2Cpp;
using Il2CppHutongGames.PlayMaker;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Messages.Outgoing;
using UnityEngine;
using Room = Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms.Room;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers
{
    public class DraftManager
    {
        public static int currentRoomSlot = 0;
        private volatile static string draftingContext = "";

        public static Room[] pickedRooms = new Room[3];

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
            pickedRooms[currentRoomSlot] = new Room(newRoom);

            currentRoomSlot++;
        }

        public static void SendDraftingContext()
        {
            UpdateDraftingContext(0);
            UpdateDraftingContext(1);
            UpdateDraftingContext(2);

            Context.Send(draftingContext, false);
            NeuroActionHandler.RegisterActions(new ChooseRoomAction());

            draftingContext = "";
            currentRoomSlot = 0;
        }

        private static void UpdateDraftingContext(int slot)
        {
            if (slot == 0)
            {
                if (GridFSMManager.TargetRank() == null || GridFSMManager.TargetTile() == null)
                {
                    Melon<Core>.Logger.Error($"Could not obtain the draft's target rank or tile while building the information for slot " + slot + "!");
                    return;
                }

                int targetRank = (int) GridFSMManager.TargetRank();
                int targetTile = (int) GridFSMManager.TargetTile();

                draftingContext += "A draft for Rank " + targetRank + ", Tile " + targetTile + " has begun.\n";
                draftingContext += "The following three rooms have been pulled from the draft pool and may chosen from:\n";
            }

            draftingContext += slot + 1 + ". " + pickedRooms[slot].name + ". " + pickedRooms[slot].rarity + " rarity.";
            if (pickedRooms[slot].effect != "")
            {
                draftingContext += " It has the following effect: " + pickedRooms[slot].effect + ".";
            }
            draftingContext += " It is " + pickedRooms[slot].types + ".";
            if (pickedRooms[slot].cost > 0)
            {
                draftingContext += "It costs " + pickedRooms[slot].cost + " gems to draft.";
            }

            FsmInt roomRotation = GameObject.Find("PLAN MANAGEMENT").GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("PLAN" + (slot + 1) + " - ROTATION AMOUNT");
            int rotation = roomRotation.value / 90;

            pickedRooms[slot].doorLayout = DoorLayout.GetDoorLayout(pickedRooms[slot].name, rotation);
            
            draftingContext += pickedRooms[slot].doorLayout.GetDraftingContext();

            draftingContext += "\n";

            if (slot == 2)
            {
                draftingContext += InventoryManager.GetInventoryContext();
            }
        }
    }
}
