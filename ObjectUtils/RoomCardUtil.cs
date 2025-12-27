using Il2Cpp;
using Il2CppHutongGames.PlayMaker;
using MelonLoader;
using UnityEngine;
using static Il2CppRewired.Demos.CustomPlatform.MyPlatformControllerExtension;
using static Il2CppRewired.UI.ControlMapper.ControlMapper;
using static Il2CppSystem.Threading.Tasks.TaskReplicator;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.ObjectWrappers
{
    public class RoomCardUtil
    {
        public static string GetDraftingContext(RoomCard newRoom)
        {
            //TODO Various effects that change costs and resources that can be spent
            //TODO This way of getting the Room Engine PlayMakerFSM is kinda ass. No hard guarantees of names matching
            PlayMakerFSM prefabFsm = GameObject.Find(newRoom.Template.Prefab.GetComponent<PlayMakerFSM>().name.ToUpper()).GetComponent<PlayMakerFSM>();

            string roomName = GetRoomName(prefabFsm);
            //TODO Move this to room transitions and such instead string roomDescription = GetRoomDescription(prefabFsm);
            string roomEffect = GetRoomEffect(prefabFsm);
            string roomTypes = GetRoomTypes(prefabFsm);
            int roomCost = GetCost(prefabFsm);

            return roomName + ". " + roomEffect + ". It " + roomTypes + ". It costs " + roomCost + " gems to draft.";
        }

        private static string GetRoomName(PlayMakerFSM prefabFsm) {
            return prefabFsm.FsmVariables.GetFsmString("ROOM NAME").value;
        }

        private static string GetRoomEffect(PlayMakerFSM prefabFsm)
        {
            //Because there is no string specifying the room effects (the text on their icons is part of the image), we gotta do it manually. This is so ass lmao
            switch (prefabFsm.name)
            {
                case "THE FOUNDATION":
                    return "Does not reset each day";
                case "SERVANT'S SPARE QUARTERS":
                case "SERVANT'S QUARTERS":
                    return "Gives 1 key for each Bedroom in your house";
                case "HER LADYSHIP'S SPARE ROOM":
                case "HER LADYSHIP'S CHAMBER":
                    return "Makes it so the next time you enter a Boudoir you gain 10 steps and the next time you enter a Walk-in Closet you gain +3 Gems";
                case "SPARE MASTER BEDROOM":
                case "MASTER BEDROOM":
                    return "Gives 1 step for each room in your house.";
                case "SPARE VERANDA":
                case "VERANDA":
                    return "Makes it so there is a greater chance of finding items in Green Rooms.";
                case "SPARE TERRACE":
                case "TERRACE":
                    return "Makes it so Green Rooms do not cost gems to draft";
                case "SPARE PATIO":
                case "PATIO":
                    return "Spreads gems in each Green Room";
                case "SPARE FOYER":
                case "FOYER":
                    return "Makes it so Hallway Doors are always unlocked";
                case "SPARE SECRET PASSAGE":
                case "SECRET PASSAGE":
                    return "Leads to a room of a color of your choice";
                case "SPARE GREAT HALL":
                case "GREAT HALL":
                    return "Has 7 locked doors";
                case "ROTUNDA":
                    return "Can rotate";
                case "PARLOR":
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("3gems"))
                    {
                        return "Has a 3 gem prize";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("windup"))
                    {
                        return "Has 2 Wind-up keys";
                    }

                    return "";
                case "FUNERAL PARLOR":
                    return "The gem prize is equal to the amount of Red Rooms in your house, but if you open an empty box you lose 30 steps";
                case "SPEAKEASY":
                    return "Basic Addition";
                case "BREAK ROOM":
                    return "If you call it a day in a Break Room, tomorrow you will begin the day with a staff keycard";
                case "POOL HALL":
                    return "Adds a Great Hall, Foyer and Secret Passage to today's Draft Pool";
                case "CLOSET":
                    return "Has 2 items.";
                case "HALLWAY CLOSET":
                    return "Has 2 items, and 1 extra item if drafted adjoined to a Hallway";
                case "BEDROOM CLOSET":
                    return "Has 2 items, and 2 extra items if drafted adjoined to a Bedroom";
                case "EMPTY CLOSET":
                    return "Has 0 items, but 4 extra items if drafted adjoined to a Red Room";
                case "WALK-IN CLOSET":
                    return "Has 4 items";
                case "ATTIC":
                    return "Has 8 items";
                case "STOREROOM":
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("gold"))
                    {
                        return "Has 1 key, 1 gem and 10 gold";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("gem"))
                    {
                        return "Has 1 key, 2 gems and 1 gold";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("key"))
                    {
                        return "Has 1 keys, 1 gem and 1 gold";
                    }
                    return "Has 1 key, 1 gem and 1 gold";
                case "NOOK":
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("keys"))
                    {
                        return "Has 2 keys";
                    }
                    return "Has 1 key";
                case "BREAKFAST NOOK":
                    return "Has 1 key and Bacon and Eggs";
                case "READING NOOK":
                    return "Has 1 key and you will always draw a Library while drafting in this room";
                case "GARAGE":
                    return "Has 3 keys";
                case "MUSIC ROOM":
                    return "Has 1 major key and 1 minor key";
                case "LOCKER ROOM":
                    return "Spreads keys throughout the house";
                case "DEN":
                    return "Has 1 gem";
                case "WINE CELLAR":
                    return "Has 3 gems";
                case "TROPHY ROOM":
                    return "Has 8 gems";
                case "BALLROOM":
                    return "Whenever you enter, your gem count is set to 2";
                case "PANTRY":
                    return "Has 4 gold";
                case "RUMPUS ROOM":
                    return "Has 8 gold";
                case "VAULT":
                    return "Has 40 gold";
                case "OFFICE":
                    return "Gives an opportunity to earn and spread gold";
                case "DRAWING ROOM":
                    return "You may draw new Floor Plans while drafting in this room";
                case "STUDY":
                    return "While drafting in this room, you may spend gems to redraw rooms up to 8 times";
                case "LIBRARY":
                    return "While drafting in this room, you can draw less common rooms";
                case "CHAMBER OF MIRRORS":
                    return "Makes it so you can draft second copies of rooms you already have in your house";
                case "THE POOL":
                    return "Adds a Locker Room, Sauna and Pump Room to today's Draft Pool";
                case "DRAFTING STUDIO":
                    return "Allows you to select a new floorplan to permanently add to your estate's draft pool";
                case "UTILITY CLOSET":
                    return "Has a Breaker Box";
                case "BOILER ROOM":
                    return "Has a Power Source";
                case "PUMP ROOM":
                    return "You can control the water flow throughout the Estate in this room";
                case "SECURITY":
                    return "You can view the inventory of all items currently in the house in this room";
                case "WORKSHOP":
                    return "You can combine inventory to create new items in this room";
                case "LABORATORY":
                    return "You can do Experimental House Features in this room";
                case "SAUNA":
                    return "Makes it so Tomorrow you will start the day with 20 extra steps";
                case "COAT CHECK":
                    return "You can check one item and retrieve it on another day in this room";
                case "MAIL ROOM":
                    return "A package will be delivered here the day after drafting this room";
                case "FREEZER":
                    return "Freezes your accounts, gold and gem amounts will not reset at the end of the day and they cannot be adjusted or used until tomorrow";
                case "DINING ROOM":
                    return "Each day, a meal is served in this room after Rank 8 has been reached";
                case "OBSERVATORY":
                    return "Gives 1 Star.";
                case "CONFERENCE ROOM":
                    return "Whenever items would be spread throughout the house, they are placed in this room instead";
                case "AQUARIUM":
                    return "Is every color of room";
                case "GOLDFISH AQUARIUM":
                    return "Is every color of room and gives 10 gold";
                case "STARFISH AQUARIUM":
                    return "Is every color of room and gives 1 Star";
                case "ELETRIC EEL AQUARIUM":
                    return "Is every color of room and has a Power Source";
                default:
                    return "";

                    //TODO Continue from Bedroom section https://blueprince.wiki.gg/wiki/Room_Directory#Show_all_spoilers-1
            }
        }

        private static string GetRoomDescription(PlayMakerFSM prefabFSM) {
            string description = prefabFSM.FsmVariables.GetFsmString("DESCRIPTION").value;

            description = description.Replace("<i>", "").Replace("</i>", "");
            description = description.Replace("<size=3.2>", "");

            return description;
        }

        private static string GetRoomTypes(PlayMakerFSM prefabFsm) {

            //TODO Upgrades and other room qualities that might not be caught by the Split()
            //TODO Better parsing in general, this shit is slapstick comedy
            //TODO Sometimes there's a space between a </color> and the next <color>

            string types = "is ";
            string fullTypeString = prefabFsm.FsmVariables.GetFsmString("TYPE").value;

            Melon<Core>.Logger.Msg($"Full types string: {prefabFsm.FsmVariables.GetFsmString("TYPE").value}");

            if (fullTypeString.Contains("Permanent"))
            {
                types += "Permanent,";
            }

            if (fullTypeString.Contains("Mechanical"))
            {
                types += "a Mechanical room,";
            }

            if (fullTypeString.Contains("Spread"))
            {
                types += "a Spread room,";
            }

            if (fullTypeString.Contains("Puzzle"))
            {
                types += "a Puzzle room,";
            }

            if (fullTypeString.Contains("Entry"))
            {
                types += "an Entry room,";
            }

            if (fullTypeString.Contains("Blueprint"))
            {
                types += "a Blueprint,";
            }

            if (fullTypeString.Contains("Hallway"))
            {
                types += "a Hallway,";
            }

            if (fullTypeString.Contains("Bedroom"))
            {
                types += "a Bedroom,";
            }

            if (fullTypeString.Contains("Green Room"))
            {
                types += "a Green Room,";
            }

            if (fullTypeString.Contains("Red Room"))
            {
                types += "a Red Room,";
            }

            if (fullTypeString.Contains("Shop"))
            {
                types += "a Shop,";
            }

            if (fullTypeString.Contains("Blackprint"))
            {
                types += "a Blackprint,";
            }

            if (fullTypeString.Contains("Dead End"))
            {
                types += "a Dead End,";
            }

            if (fullTypeString.Contains("Upgrade"))
            {
                types += "an Upgrade,";
            }

            if ((fullTypeString).Contains("Addition"))
            {
                types += "an Addition,";
            }

            if (fullTypeString.Contains("Tomorrow"))
            {
                types += "a Tomorrow room,";
            }

            if (fullTypeString.Contains("Drafting"))
            {
                types += "a Drafting room,";
            }

            if (fullTypeString.Contains("Objective"))
            {
                types += "an Objective,";
            }

            int commaCount = types.Split(",").Length - 1;

            if (commaCount == 0)
            {
                //Should not happen but just in case
                return "";
            }

            //Replace the last comma with a full stop.
            types = types.Remove(types.LastIndexOf(","), types.Length).Insert(types.LastIndexOf(","), ".");

            //Replace the penultimate comma with an "and"
            if (commaCount > 1)
            {
                types = types.Remove(types.LastIndexOf(","), types.Length).Insert(types.LastIndexOf(","), " and ");
            }

            return types;
        }

        private static int GetCost(PlayMakerFSM prefabFsm)
        {
            return prefabFsm.FsmVariables.GetFsmInt("GEM COST").value;
        }
    }
}
