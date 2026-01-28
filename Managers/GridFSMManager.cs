using Blue_Prince_Neuro_Sama_Integration_Mod.Rooms;
using Il2Cpp;
using Il2CppHutongGames.PlayMaker;
using Il2CppToolBuddy.ThirdParty.VectorGraphics;
using MelonLoader;
using UnityEngine;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.Managers
{
    public class GridFSMManager
    {
        private static PlayMakerFSM gridFSM;

        public static void Initialize()
        {
            GameObject gridObject = GameObject.Find("THE GRID");
            gridFSM = gridObject.GetComponent<PlayMakerFSM>();

            //Melon<Core>.Logger.Msg($"Grid");
            foreach (FsmGameObject nv in gridFSM.FsmVariables.GameObjectVariables)
            {
                string name;
                string value;

                if (nv == null || nv.Name == null)
                {
                    name = "None";
                    value = "None";
                }
                else
                {
                    name = nv.Name;

                    if (nv.Value == null)
                    {
                        value = "None";
                    } else
                    {
                        value = nv.Value.ToString();
                    }
                }

                //Melon<Core>.Logger.Msg($"Variable - {name}||{value}");
            }
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

        private static string? GetFsmString(string name)
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

        public static int? CurrentRank()
        {
            return GetFsmInt("Current Rank");
        }

        public static int? CurrentWing()
        {
            int? tile = GetFsmInt("Current Tile");

            if (tile == null) return null;

            if (tile <= 5) return tile;


            decimal d = (decimal)tile / 5;

            decimal decimalPart = d - Math.Truncate(d);

            if (decimalPart == 0)
            {
                return 5;
            } else
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
            return GetFsmInt("Target Tile");
        }

        public static bool? NorthSouth()
        {
            return GetFsmBool("NorthSouth");
        }

        public static string? CurrentRoom()
        {
            return GetFsmString("CURRENT ROOM");
        }

        public static string? EastDoor()
        {
            return GetFsmString("EastDoor");
        }

        public static string? NorthDoor()
        {
            return GetFsmString("NorthDoor");
        }

        public static string? SouthDoor()
        {
            return GetFsmString("SouthDoor");
        }

        public static string? WestDoor()
        {
            return GetFsmString("WestDoor");
        }
    }
}
