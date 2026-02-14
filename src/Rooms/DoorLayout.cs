using Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers;
using MelonLoader;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms
{
    public class DoorLayout
    {
        public Door northDoor;
        public Door eastDoor;
        public Door southDoor;
        public Door westDoor;

        //Rotation increments by 1 for every 90 degrees of clockwise rotation
        private DoorLayout(bool northDefault, bool eastDefault, bool westDefault, int totalRotation)
        {
            northDoor = new Door(northDefault, null, false, false);
            eastDoor =  new Door(eastDefault, null, false, false);
            southDoor = new Door(true,false,true,true); //Plans always have an entry door, which is south facing before taking rotation in consideration
            westDoor = new Door(westDefault, null, false, false);

            int? draftDirection = GridFSMManager.CurrentDraftDirection();
            if (draftDirection == null)
            {
                Melon<Core>.Logger.Error($"Draft direction was null, draft information might be incorrect!");
            } else
            {
                int draftRotations = (int)draftDirection / 90;
                int planRotations = totalRotation - draftRotations;

                RotatePlan(planRotations);
                RotateDraft(draftRotations);
            }

            Room[] neighbours = GridFSMManager.GetTargetNeighbours();

            northDoor.isBlocked = neighbours[0] == null ? false : !neighbours[0].doorLayout.southDoor.exists;
            eastDoor.isBlocked = neighbours[1] == null ? false : !neighbours[1].doorLayout.westDoor.exists;
            southDoor.isBlocked = neighbours[2] == null ? false : !neighbours[2].doorLayout.northDoor.exists;
            westDoor.isBlocked = neighbours[3] == null ? false : !neighbours[3].doorLayout.eastDoor.exists;
        }

        private void RotatePlan(int planRotations) {
            if (planRotations == 0) return;

            Door tempNorth = northDoor;

            if (planRotations < 0)
            {
                northDoor = eastDoor;
                eastDoor = southDoor;
                southDoor = westDoor;
                westDoor = tempNorth;

                eastDoor.isEntry = false;
                southDoor.isEntry = true;

                RotatePlan(planRotations + 1);
            }

            if (planRotations > 0)
            {
                northDoor = westDoor;
                westDoor = southDoor;
                southDoor = eastDoor;
                eastDoor = tempNorth;

                westDoor.isEntry = false;
                southDoor.isEntry = true;

                RotatePlan(planRotations - 1);
            }
        }

        private void RotateDraft(int draftRotations)
        {
            if (draftRotations == 0) return;

            Door tempNorth = northDoor;

            northDoor = westDoor;
            westDoor = southDoor;
            southDoor = eastDoor;
            eastDoor = tempNorth;

            RotateDraft(draftRotations - 1);
        }

        public string GetDraftingContext()
        {
            string draftingContext = " It has an unblocked door to the";

            if (northDoor.exists && !northDoor.isEntry && !northDoor.isBlocked) {
                draftingContext += " north,";
            }

            if (eastDoor.exists && !eastDoor.isEntry && !eastDoor.isBlocked) {
                draftingContext += " east,";
            }

            if (westDoor.exists && !westDoor.isEntry && !westDoor.isBlocked) {
                draftingContext += " west,";
            }

            if (southDoor.exists && !southDoor.isEntry && !southDoor.isBlocked)
            {
                draftingContext += " south,";
            }
            
            int commaCount = draftingContext.Split(",").Length - 1;

            if (commaCount < 1)
            {
                return " It does not have any doors.";
            }

            //Remove the last comma
            draftingContext = draftingContext.Remove(draftingContext.LastIndexOf(","), 1).Insert(draftingContext.LastIndexOf(","), ".");

            //Replace the penultimate comma with an "and"
            if (commaCount > 1)
            {
                draftingContext = draftingContext.Remove(draftingContext.LastIndexOf(","), 1).Insert(draftingContext.LastIndexOf(","), " and");
            }

            return draftingContext;
        }

        public static DoorLayout GetDoorLayout(string roomName, int rotation)
        {
            roomName = roomName.ToUpper();

            switch (roomName)
            {
                case "THE FOUNDATION":
                case "DEN":
                case "DRAWING ROOM":
                case "THE POOL":
                case "SECURITY":
                case "DINING ROOM":
                case "CONFERENCE ROOM":
                case "AQUARIUM":
                case "GOLDFISH AQUARIUM":
                case "STARFISH AQUARIUM":
                case "ELETRIC EEL AQUARIUM":
                case "WEST WING HALL":
                case "EAST WING HALL":
                case "COURTYARD":
                case "SECRET GARDEN":
                case "GIFT SHOP":
                case "CHAPEL":
                case "GYMNASIUM":
                case "DARKROOM":
                case "THRONE ROOM":
                case "CLOSED EXHIBIT":
                case "HALLWAY":
                    //Side only
                    return new DoorLayout(false, true, true ,rotation);
                case "ENTRANCE HALL":
                case "ANTECHAMBER":
                case "PASSAGEWAY":
                case "GREAT HALL":
                case "CLOISTER":
                case "ARCHIVES":
                case "WEIGHT ROOM":
                case "VESTIBULE":
                case "MECHANARIUM":
                    //All doors
                    return new DoorLayout(true, true, true, rotation);
                case "SPARE ROOM":
                case "SERVANT'S SPARE QUARTERS":
                case "HER LADYSHIP'S SPARE ROOM":
                case "SPARE MASTER BEDROOM":
                case "SPARE VERANDA":
                case "SPARE TERRACE":
                case "SPARE PATIO":
                case "SPARE FOYER":
                case "SPARE SECRET PASSAGE":
                case "SPARE GREAT HALL":
                case "VERANDA":
                case "GALLERY":
                case "LOCKER ROOM":
                case "BALLROOM":
                case "RUMPUS ROOM":
                case "DRAFTING STUDIO":
                case "WORKSHOP":
                case "CORRIDOR":
                case "FOYER":
                case "SHOWROOM":
                case "THE KENNEL":
                case "TUNNEL":
                    //Straight rooms
                    return new DoorLayout(true,false,false, rotation);
                case "SERVANT'S QUARTERS":
                case "HER LADYSHIP'S CHAMBER":
                case "MASTER BEDROOM":
                case "TERRACE":
                case "CLOSET":
                case "HALLWAY CLOSET":
                case "BEDROOM CLOSET":
                case "EMPTY CLOSET":
                case "WALK-IN CLOSET":
                case "UTILITY CLOSET":
                case "ATTIC":
                case "STOREROOM":
                case "WINE CELLAR":
                case "VAULT":
                case "STUDY":
                case "CHAMBER OF MIRRORS":
                case "SAUNA":
                case "COAT CHECK":
                case "MAIL ROOM":
                case "FREEZER":
                case "ROOM 46":
                case "GUEST BEDROOM":
                case "GUESS BEDROOM":
                case "QUEST BEDROOM":
                case "GEIST BEDROOM":
                case "NURSERY":
                case "NURSERY10":
                case "NURSES STATION":
                case "INDOOR NURSERY":
                case "BUNK ROOM":
                case "GREENHOUSE":
                case "LOCKSMITH":
                case "LAUNDRY ROOM":
                case "LAVATORY":
                case "FURNACE":
                case "CLOCK TOWER":
                case "SOLARIUM":
                case "DORMITORY":
                case "PLANETARIUM":
                case "TOOLSHED":
                case "BOMB SHELTER":
                case "SCHOOLHOUSE":
                case "SHRINE":
                case "ROOT CELLAR":
                case "HOVEL":
                case "TRADING POST":
                case "TOMB":
                case "GARAGE":
                case "SECRET PASSAGE":
                    //Dead ends
                    return new DoorLayout(false, false, false, rotation);
                case "ROTUNDA":
                case "PARLOR":
                case "FUNERAL PARLOR":
                case "BILLIARD ROOM":
                case "SPEAKEASY":
                case "BREAK ROOM":
                case "POOL HALL":
                case "NOOK":
                case "BREAKFAST NOOK":
                case "READING NOOK":
                case "MUSIC ROOM":
                case "TROPHY ROOM":
                case "PANTRY":
                case "OFFICE":
                case "LIBRARY":
                case "PUMP ROOM":
                case "LABORATORY":
                case "OBSERVATORY":
                case "BEDROOM":
                case "BOUDOIR":
                case "PATIO":
                case "COMMISSARY":
                case "KITCHEN":
                case "BOOKSHOP":
                case "THE ARMORY":
                case "MAID'S CHAMBER":
                case "DOVECOTE":
                case "CLASSROOM":
                case "CASINO":
                case "TREASURE TROVE":
                case "CONSERVATORY":
                case "LOST AND FOUND":
                    //West only
                    return new DoorLayout(false, false, true, rotation);
                case "ROOM 8":
                case "MORNING ROOM":
                    //East only
                    return new DoorLayout(false, true, false, rotation);
                case "BOILER ROOM":
                    //North and west
                    return new DoorLayout(true, false, true, rotation);
            }

            return null;
        }
    }
}
