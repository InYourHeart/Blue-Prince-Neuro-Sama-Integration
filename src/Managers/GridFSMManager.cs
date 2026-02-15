using Il2Cpp;
using MelonLoader;
using NeuroSDKCsharp.Messages.Outgoing;
using UnityEngine;
using Room = Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms.Room;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers
{
    public class GridFSMManager
    {
        private static int MAX_RANK = 9;
        private static int MIN_RANK = 1;
        private static int MAX_TILE = 5;
        private static int MID_TILE = 3;
        private static int MIN_TILE = 1;

        private static int MIN_GARAGE_RANK = 3;

        private static int ENTRANCE_HALL_RANK = 1;
        private static int ENTRANCE_HALL_TILE = 3;

        private static int ANTECHAMBER_RANK = 9;
        private static int ANTECHAMBER_TILE = 3;

        public static int NORTH_ANGLE = 0;
        public static int EAST_ANGLE = 90;
        public static int SOUTH_ANGLE = 180;
        public static int WEST_ANGLE = 270;

        private static PlayMakerFSM gridFSM;

        private static Room[,] HouseLayout = new Room[9, 5];

        public static void Initialize()
        {
            GameObject gridObject = GameObject.Find("THE GRID");
            gridFSM = gridObject.GetComponent<PlayMakerFSM>();
        }

        public static void Set(int rank, int tile, Room room)
        {
            Context.Send("A " + room.name + " has been drafted into Rank " + rank + ", Tile" + tile, false);

            HouseLayout[rank - 1, tile - 1] = room;
        }

        private static Room Get(int rank, int tile)
        {
            if (rank <  MIN_RANK || rank > MAX_RANK || tile < MIN_TILE || tile > MAX_TILE) { return null;}

            return HouseLayout[rank - 1, tile - 1];
        }

        public static Room[] GetCurrentNeighbours()
        {
            int rank = CurrentRank() == null ? ENTRANCE_HALL_RANK : (int) CurrentRank();
            int tile = CurrentTile() == null ? ENTRANCE_HALL_TILE : (int) CurrentTile();

            return GetNeighbours(rank, tile);
        }

        public static Room[] GetTargetNeighbours() {
            int rank = TargetRank() == null ? -1 : (int) TargetRank();
            int tile = TargetTile() == null ? -1 : (int) TargetTile();

            if (rank == -1 || tile == -1) {
                Melon<Core>.Logger.Error("Could not get target's rank or tile!");
                return new Room[3];
            }

            return GetNeighbours(rank, tile);
        } 

        //Returns rooms neighbouring the tile in a North-East-South-West order
        private static Room[] GetNeighbours(int rank, int tile)
        {
            Room northRoom = null;
            Room eastRoom = null;
            Room southRoom = null;
            Room westRoom = null;

            //Northern room
            //No northern rooms at rank 9 outside of room 46.
            if (rank == ANTECHAMBER_RANK && tile == ANTECHAMBER_TILE)
            {
                //Might be spoilers?
                northRoom = new Room("ROOM 46");
            }
            else if (rank < MAX_RANK)
            {
                northRoom = Get(rank + 1, tile);
            }

            //Eastern room
            if (tile < MAX_TILE)
            {
                eastRoom = Get(rank, tile + 1);
            }

            //Southern room
            //Connection to the grounds from the Entrance Hall
            if (rank == MIN_RANK && tile == MID_TILE)
            {
                //TODO Figure out how to include the grounds
            }
            else if (rank > MIN_RANK)
            {
                southRoom = Get(rank - 1, tile);
            }

            //Western room
            //Garage
            if (rank > MIN_GARAGE_RANK && rank < MAX_RANK && tile == MIN_TILE)
            {
                //TODO Figure out how to include the west path
            }
            else if (tile > MIN_TILE)
            {
                westRoom = Get(rank, tile - 1);
            }

            return new Room[] { northRoom, eastRoom, southRoom, westRoom };
        }

        private static int? GetFsmInt(string name)
        {
            try
            {
                return gridFSM.FsmVariables.GetFsmInt(name).Value;
            } catch (NullReferenceException)
            {
                return null;
            }
        }

        private static bool? GetFsmBool(string name)
        {
            try
            {
                return gridFSM.FsmVariables.GetFsmBool(name).Value;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        private static string GetFsmString(string name)
        {
            try
            {
                return gridFSM.FsmVariables.GetFsmString(name).Value;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static int? CurrentDraftDirection()
        {
            int currentRank = CurrentRankHandlingNull();
            int currentTile = CurrentTileHandlingNull();
            int? currentTargetRank = TargetRank();
            int? currentTargetTile = TargetTile();

            if (currentTargetTile == null || currentTargetRank == null) return null;

            if (currentTargetRank > currentRank) return NORTH_ANGLE;

            if (currentRank > currentTargetRank) return SOUTH_ANGLE;

            if (currentTargetTile > currentTile) return EAST_ANGLE;

            if (currentTile > currentTargetTile) return WEST_ANGLE;

            return null;
        }

        public static Room GetCurrentNorth()
        {
            return HouseLayout[CurrentRankHandlingNull(), CurrentTileHandlingNull() - 1];
        }

        public static Room GetCurrentSouth()
        {
            return HouseLayout[CurrentRankHandlingNull() - 2, CurrentTileHandlingNull() - 1];
        }

        public static Room GetCurrentEast()
        {
            return HouseLayout[CurrentRankHandlingNull() - 1, CurrentTileHandlingNull()];
        }

        public static Room GetCurrentWest()
        {
            return HouseLayout[CurrentRankHandlingNull() - 1, CurrentTileHandlingNull() - 2];
        }

        private static int CurrentRankHandlingNull()
        {
            return CurrentRank() == null ? 1 : (int)CurrentRank();
        }

        private static int CurrentTileHandlingNull()
        {
            return CurrentTile() == null ? 3 : (int)CurrentTile();
        }

        public static int? CurrentRank()
        {
            return GetFsmInt("Current Rank");
        }

        public static int? CurrentTile()
        {
            int? tile = GetFsmInt("Current Tile");

            return AbsoluteToRelativeTile(tile);
        }

        private static int? AbsoluteToRelativeTile(int? tile)
        {
            if (tile == null) return null;

            if (tile <= 5) return tile;

            decimal d = (decimal)tile / 5;

            decimal decimalPart = d - Math.Truncate(d);

            if (decimalPart == 0)
            {
                return 5;
            }
            else
            {
                return (int?) (decimalPart * 5);
            }
        }

        public static int? TargetRank() {
            //This typo is the Blue Prince dev's. Lmao.
            return GetFsmInt("Taret Rank");
        }

        public static int? TargetTile()
        {
            int? tile = GetFsmInt("Target Tile");

            return AbsoluteToRelativeTile(tile);
        }

        public static bool? NorthSouth()
        {
            return GetFsmBool("NorthSouth");
        }

        public static string CurrentRoom()
        {
            return GetFsmString("CURRENT ROOM");
        }

        public static string EastDoor()
        {
            return GetFsmString("EastDoor");
        }

        public static string NorthDoor()
        {
            return GetFsmString("NorthDoor");
        }

        public static string SouthDoor()
        {
            return GetFsmString("SouthDoor");
        }

        public static string WestDoor()
        {
            return GetFsmString("WestDoor");
        }
    }
}
