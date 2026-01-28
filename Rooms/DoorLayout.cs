using MelonLoader;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.Rooms
{
    public class DoorLayout
    {
        public Door north;
        public Door east;
        public Door south;
        public Door west;

        //Rotation increments by 1 for every 90 degrees of clockwise rotation
        private DoorLayout(bool northDefault, bool eastDefault, bool westDefault, int rotation)
        {
            this.north = northDefault;
            this.east = eastDefault;
            this.south = true; //Plans always have a south door
            this.west = westDefault;

            this.Rotate(rotation);
        }

        private void Rotate(int rotations)
        {
            if (rotations == 0)
            {
                return;
            }

            bool tempNorth = north;

            north = west;
            west = south;
            south = east;
            east = tempNorth;

            Rotate(rotations - 1);
        }

        public string GetDraftingContext()
        {
            if (!north && !east && !west) {
                return " It does not have any doors.";
            }

            string draftingContext = " It has a door to the";

            if (north) {
                draftingContext += " north,";
            }

            if (east) {
                draftingContext += " east,";
            }

            if (west) {
                draftingContext += " west,";
            }

            if (south){
                draftingContext += " south,";
            }

            int commaCount = draftingContext.Split(",").Length - 1;

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
                case "HALLWAY":
                    //North and west
                    return new DoorLayout(true, false, true, rotation);
            }

            return null;
        }
    }
}
