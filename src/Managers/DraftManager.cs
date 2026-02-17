using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;
using Il2Cpp;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Messages.Outgoing;
using UnityEngine;
using Room = Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms.Room;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers
{
    public class DraftManager
    {
        public static Room[] draftedRooms = new Room[3];
        public static bool isDrafting = false;

        public static void SendDraftingContext()
        {
            isDrafting = true;

            string draftingContext = "";

            draftingContext += UpdateDraftingContext("1");
            draftingContext += UpdateDraftingContext("2");
            draftingContext += UpdateDraftingContext("3");

            Context.Send(draftingContext, false);
            NeuroActionHandler.RegisterActions(new ChooseRoomAction());
        }

        private static Room GetDraftedRoom(string slot)
        {
            try
            {
                Room room = new Room("PLAN" + slot + " - ENGINE");

                int? roomRotation = FsmUtil.GetFsmInt("PLAN MANAGEMENT", "PLAN" + slot + " - ROTATION AMOUNT");
                int rotation = roomRotation == null ? 0 : (int) roomRotation / 90;

                room.doorLayout = DoorLayout.GetDoorLayout(room.name, rotation);

                draftedRooms[int.Parse(slot) - 1] = room;

                return room;
            } catch (Exception e)
            {
                Melon<Core>.Logger.Error($"Could not retrieve the Room object for slot " + slot + "!");
                return null;
            }
        }

        public static bool IsHovelActive()
        {
            return FsmUtil.GetFsmBool("DRAFT UI","hovel") == null ? false : (bool) FsmUtil.GetFsmBool("DRAFT UI", "hovel");
        }

        private static string UpdateDraftingContext(string slot)
        {
            Room roomInSlot = GetDraftedRoom(slot);

            if (roomInSlot == null) {
                return "";
            }

            string draftingContext = "";

            if (slot.Equals("1"))
            {
                if (GridFSMManager.TargetRank() == null || GridFSMManager.TargetTile() == null)
                {
                    Melon<Core>.Logger.Error($"Could not obtain the draft's target rank or tile while building the information for slot " + slot + "!");
                    return "";
                }

                int targetRank = (int) GridFSMManager.TargetRank();
                int targetTile = (int) GridFSMManager.TargetTile();

                draftingContext += "A draft for Rank " + targetRank + ", Tile " + targetTile + " has begun.\n";
                draftingContext += "The following three rooms have been pulled from the draft pool and may chosen from:\n";
            }

            draftingContext += slot + ". " + roomInSlot.name + ". " + roomInSlot.rarity + " rarity.";
            if (roomInSlot.effect != "")
            {
                draftingContext += " It has the following effect: " + roomInSlot.effect + ".";
            }
            draftingContext += " It is" + roomInSlot.types + ".";
            if (roomInSlot.cost > 0)
            {
                int cost = (IsHovelActive() ? roomInSlot.cost * 3 : roomInSlot.cost);
                string resource = (IsHovelActive() ? "steps" : "gems");

                draftingContext += " It costs " + cost + " " + resource + " to draft.";
            }
            
            //Don't look for Outer Room's door layout
            if (!roomInSlot.isOuter){
                draftingContext += roomInSlot.doorLayout.GetDraftingContext();
            }

            draftingContext += "\n";

            if (slot.Equals("3"))
            {
                draftingContext += InventoryManager.GetInventoryContext();
            }

            return draftingContext;
        }
    }
}
