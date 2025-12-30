using Blue_Prince_Neuro_Sama_Integration_Mod.Rooms;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.Managers
{
    public class HouseManager
    {
        private static Room[][] HouseLayout;

        public static Room Get(int rank, int wing)
        {
            return HouseLayout[rank - 1][wing - 1];
        }
    
        //Returns rooms neighbouring the tile in a North-East-South-West order
        public static Room[] GetNeighbours(int rank, int wing)
        {
            Room northRoom = null;
            Room eastRoom = null;
            Room southRoom = null;
            Room westRoom = null;

            //Northern room
            //No northern rooms at rank 9 outside of room 46.
            if (rank == 8 && wing == 2)
            {
                //Might be spoilers?
                northRoom = new Room("ROOM 46");
            } else if (rank < 8)
            {
                northRoom = HouseLayout[rank + 1][wing];
            }

            //Eastern room
            if (wing < 4)
            {
                eastRoom = HouseLayout[rank][wing + 1];
            }

            //Southern room
            //Connection to the grounds from the Entrance Hall
            if (rank == 0 && wing == 2)
            {
                //TODO Figure out how to include the grounds
            } else if (rank > 0)
            {
                southRoom = HouseLayout[rank - 1][wing];
            }

            //Western room
            //Garage
            if (rank > 3 && rank < 8 && wing == 0)
            {
                //TODO Figure out how to include the west path
            } else if (wing > 0)
            {
                westRoom = HouseLayout[rank][wing - 1];
            }

            return new Room[] { northRoom, eastRoom, southRoom, westRoom };
        }
    }
}
