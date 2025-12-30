using static Il2CppRewired.UI.ControlMapper.ControlMapper;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.Rooms
{
    public class DoorLayout
    {
        public bool north;
        public bool east;
        public bool south;
        public bool west;

        //Rotation increments by 1 for every 90 degrees of clockwise rotation
        public DoorLayout(bool northDefault, bool eastDefault, bool westDefault)
        {
            this.north = northDefault;
            this.east = eastDefault;
            this.south = true; //Plans always have a south door
            this.west = westDefault;
        }

        public void Rotate(int rotations)
        {
            if (rotations == 0)
            {
                return;
            }

            bool tempNorth = north;

            north = east;
            east = south;
            south = west;
            west = tempNorth;

            Rotate(rotations--);
        }

        public static DoorLayout GetDoorLayout(string roomName)
        {
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
                case "HALLWAY":
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
                    return new DoorLayout(false, true, true);
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
                    return new DoorLayout(true, true, true);
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
                    return new DoorLayout(true,false,false);
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
                    return new DoorLayout(false, false, false);
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
                    return new DoorLayout(false, false, true);
                case "ROOM 8":
                case "MORNING ROOM":
                    //East only
                    return new DoorLayout(false, true, false);
                case "BOILER ROOM":
                    //North and west
                    return new DoorLayout(true, false, true);
            }

            return null;
        }
    }
}
