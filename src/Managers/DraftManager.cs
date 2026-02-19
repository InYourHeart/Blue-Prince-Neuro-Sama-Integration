using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;
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
        public static bool pickDraftActionActive = false;

        public static void SendDraftingContext()
        {
            isDrafting = true;

            string draftingContext = "";

            draftingContext += UpdateDraftingContext("1");
            draftingContext += UpdateDraftingContext("2");
            draftingContext += UpdateDraftingContext("3");

            Context.Send(draftingContext, false);
            NeuroActionHandler.RegisterActions(new ChooseRoomAction());

            pickDraftActionActive = true;
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
            } catch (Exception)
            {
                Melon<Core>.Logger.Error($"Could not retrieve the Room object for slot " + slot + "!");
                return null;
            }
        }

        public static bool IsHovelActive()
        {
            return FsmUtil.GetFsmBool("DRAFT UI","hovel") == null ? false : (bool) FsmUtil.GetFsmBool("DRAFT UI", "hovel");
        }

        public static bool IsArchived(string slot)
        {
            return FsmUtil.GetFsmInt("PLAN MANAGEMENT", "ArchivedPick") == int.Parse(slot);
        }

        private static string StartingDraftContext(string slot)
        {
            if (GridFSMManager.TargetRank() == null || GridFSMManager.TargetTile() == null)
            {
                Melon<Core>.Logger.Error($"Could not obtain the draft's target rank or tile while building the starting draft text!");
                return "";
            }

            int targetRank = (int)GridFSMManager.TargetRank();
            int targetTile = (int)GridFSMManager.TargetTile();

            return "A draft for Rank " + targetRank + ", Tile " + targetTile + " has begun.\n" +
                    "The following three floor plans have been pulled from the draft pool and may chosen from:\n";
        }

        private static string SlotNumberContext(string slot)
        {
            return slot + ". ";
        }
        private static string ArchivedFloorPlanContext(Room roomInSlot)
        {
            return "An archived floor plan\n";
        }

        private static string FloorPlanContext(Room roomInSlot)
        {
            string floorPlanText = "";

            floorPlanText += FloorPlanNameContext(roomInSlot);

            floorPlanText += FloorPlanRarityContext(roomInSlot);

            if (roomInSlot.effect != "")
            {
                floorPlanText += FloorPlanEffectContext(roomInSlot);
            }

            floorPlanText += FloorPlanTypesContext(roomInSlot);

            //Don't look for Outer Room's door layout
            if (!roomInSlot.isOuter)
            {
                floorPlanText += "\t* Unblocked doors: " + roomInSlot.doorLayout.GetDraftingContext() + "\n";
            }

            return floorPlanText;
        }

        private static string FloorPlanNameContext(Room roomInSlot)
        {
            return roomInSlot.name + "\n";
        }

        private static string FloorPlanRarityContext(Room roomInSlot)
        {
            return "\t* Rarity: " + roomInSlot.rarity + ";\n";
        }

        private static string FloorPlanEffectContext(Room roomInSlot)
        {
            return "\t* Effect: " + roomInSlot.effect + ";\n";
        }

        private static string FloorPlanTypesContext(Room roomInSlot)
        {
            return "\t* Type:" + roomInSlot.types + ";\n";
        }

        private static string FloorPlanCostContext(Room roomInSlot)
        {
            if (roomInSlot.cost <= 0) return "";

            int cost = (IsHovelActive() ? roomInSlot.cost * 3 : roomInSlot.cost);
            string resource = (IsHovelActive() ? "steps" : "gems");

            return "\t* Cost: " + cost + " " + resource + ";\n";
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
                draftingContext += StartingDraftContext(slot);
            }

            draftingContext += SlotNumberContext(slot);

            if (IsArchived(slot))
            {
                draftingContext += ArchivedFloorPlanContext(roomInSlot);
            } else
            {
                draftingContext += FloorPlanContext(roomInSlot);
            }

            draftingContext += FloorPlanCostContext(roomInSlot);


            if (slot.Equals("3"))
            {
                draftingContext += InventoryManager.GetInventoryContext();
            }

            if (draftingContext.LastIndexOf(";") != -1) {
                draftingContext = draftingContext.Remove(draftingContext.LastIndexOf(";"), 1).Insert(draftingContext.LastIndexOf(";"), ".");
            }

            return draftingContext;
        }
    }
}
