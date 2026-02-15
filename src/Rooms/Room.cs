using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms
{
    public class Room
    {
        public string name { get; set; }
        public string description { get; set; }
        public string rarity { get; set; }
        public string effect { get; set; }
        public string types { get; set; }
        public int cost { get; set; }

        public bool isOuter { get; set; }

        public DoorLayout doorLayout { get; set; }

        public Room(string roomName)
        {
            PlayMakerFSM fsm = GameObject.Find(roomName).GetComponent<PlayMakerFSM>();
            Initialize(fsm);
        }

        public Room(PlayMakerFSM fsm) {
            Initialize(fsm);
        }

        private void Initialize(PlayMakerFSM fsm)
        {
            name = GetRoomName(fsm);
            description = GetRoomDescription(fsm);
            rarity = GetRoomRarity(fsm);
            effect = GetRoomEffect(fsm);
            types = GetRoomTypes(fsm);
            cost = GetCost(fsm);
            isOuter = types.IndexOf("Outer Room") != -1;
        }

        private static string GetRoomName(PlayMakerFSM prefabFsm) {
            return prefabFsm.FsmVariables.GetFsmString("ROOM NAME").value;
        }

        private static string GetRoomRarity(PlayMakerFSM prefabFsm)
        {
            string rarityValue = prefabFsm.FsmVariables.GetFsmString("RARITY").value;

            switch (rarityValue)
            {
                case "1":
                    return "Commonplace";
                case "2":
                    return "Standard";
                case "3":
                    return "Unusual";
                case "4":
                    return "Rare";
                case "<size=7>Rumored</size>":
                    return "Rumored";
                default:
                    return "n/a";
            }
        }

        private static string GetRoomEffect(PlayMakerFSM prefabFsm)
        {
            //Because there is no string specifying the room effects (the text on their icons is part of the image), we gotta do it manually.
            switch (prefabFsm.name)
            {
                case "THE FOUNDATION":
                    return "Does not reset each day";
                case "SERVANT'S SPARE QUARTERS":
                case "SERVANT'S QUARTERS":
                    return "+1 key for each Bedroom in your house";
                case "HER LADYSHIP'S SPARE ROOM":
                case "HER LADYSHIP'S CHAMBER":
                    return "Makes it so the next time you enter a Boudoir you gain 10 steps and the next time you enter a Walk-in Closet you gain +3 Gems";
                case "SPARE MASTER BEDROOM":
                case "MASTER BEDROOM":
                    return "Gives 1 step for each room in your house";
                case "SPARE VERANDA":
                case "VERANDA":
                    return "Makes it so there is a greater chance of finding items in Green Rooms";
                case "SPARE TERRACE":
                case "TERRACE":
                    return "Green Rooms do not cost gems to draft";
                case "SPARE PATIO":
                case "PATIO":
                    return "Spread gems in each Green Room";
                case "SPARE FOYER":
                case "FOYER":
                    return "Hallway Doors are always unlocked";
                case "SPARE SECRET PASSAGE":
                case "SECRET PASSAGE":
                    return "Leads to a room of a color of your choice";
                case "SPARE GREAT HALL":
                case "GREAT HALL":
                    return "7 Locked Doors";
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
                    return "Has 2 items";
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
                    return "Gives 1 Star";
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
                case "BEDROOM":
                    return "Whenever you enter this room, gain 2 steps";
                case "BOUDOIR":
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("walk"))
                    {
                        return "+1 gem";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("par"))
                    {
                        return "+2 dice";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("obs"))
                    {
                        return "+3 apples";
                    }
                    return "";
                case "GUEST BEDROOM":
                    return "+10 steps";
                case "GUESS BEDROOM":
                    return "Hidden effect of a random BEDROOM in your draft pool?";
                case "QUEST BEDROOM":
                    return "+10 steps, if you enter ANTECHAMBER today, add 2 gold to your allowance";
                case "GEIST BEDROOM":
                    return "+2 dice, if you have TOMB on the estate today, gain an additional 4 dice";
                case "NURSERY":
                    return "Whenever you draft a Bedroom, gain 5 steps";
                case "NURSERY10":
                    return "Whenever you draft a Bedroom, gain 8 steps";
                case "NURSES STATION":
                    return "If you have less than 10 steps when you enter this room, set your steps to 20";
                case "INDOOR NURSERY":
                    return "Whenever you draft another GREEN ROOM, 2 gems will sprout in this room";
                case "BUNK ROOM":
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("gold"))
                    {
                        return "This room counts as 2 BEDROOMS, if you have exactly 2 SHOPS when drafting this room, DOUBLE your gold";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("gem"))
                    {
                        return "This room counts as 2 BEDROOMS, if you have exactly 2 GREEN ROOMS when drafting this room, DOUBLE your gems";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("key"))
                    {
                        return "This room counts as 2 BEDROOMS, if you have exactly 2 HALLWAYS when drafting this room, DOUBLE your keys";
                    }
                    return "This room counts as 2 BEDROOMS";
                case "HALLWAY":
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("1key"))
                    {
                        return "+1 key";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("2keys"))
                    {
                        return "+1 locked trunks";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("4keys"))
                    {
                        return "Add an extra HALLWAY to tomorrow's draft pool";
                    }
                    return "";
                case "CORRIDOR":
                    return "CORRIDOR is always left unlocked";
                case "COURTYARD":
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("gems"))
                    {
                        return "+2 gems";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("digs"))
                    {
                        return "5 digspots";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("corriyard"))
                    {
                        return "CORRIYARD is always left unlocked";
                    }
                    return "";
                case "CLOISTER":
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("dauja"))
                    {
                        return "Gain 2 stars for each room with an animal you draft from this CLOISTER";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("draxus"))
                    {
                        return "Gain 4 dice for each DEAD-END room you WILL draft from this CLOISTER";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("joya"))
                    {
                        return "Permanently add an extra 5 steps to the MAIN COURSE for each KITCHEN, PANTRY, or FURNACE you draft from this CLOISTER";
                    }
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("lydia"))
                    {
                        return "Add 2 gold to your allowance for each SHOP you draft from this CLOISTER";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("mila"))
                    {
                        return "Find an extra item in each BEDROOM you draft from this CLOISTER";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("orinda"))
                    {
                        return "Open a random door of the ANTECHAMBER for each BLACKPRINT you draft from this CLOISTER";
                    }
                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("rynna"))
                    {
                        return "Raise your LUCK with each GREEN ROOM you draft from this CLOISTER";
                    }

                    if (prefabFsm.FsmVariables.GetFsmTexture("EAST").Value.name.Contains("veia"))
                    {
                        return "Find 8 dirt piles in each room with a fireplace you draft from this CLOISTER";
                    }
                    return "";
                case "GREENHOUSE":
                    return "You are more likely to draw GREEN ROOMS while drafting";
                case "MORNING ROOM":
                    return "+2 gems, tomorrow you will start with 2 gems";
                case "SECRET GARDEN":
                    return "Spread Fruit throughout the House";
                case "COMMISSARY":
                    return "Items for Sale";
                case "KITCHEN":
                    return "Food for Sale";
                case "LOCKSMITH":
                    return "Keys for Sale";
                case "SHOWROOM":
                    return "Luxury Items for Sale";
                case "LAUNDRY ROOM":
                    return "Launder Currency";
                case "BOOKSHOP":
                    return "Books for sale";
                case "THE ARMORY":
                    return "Weapons & Armor for sale";
                case "GIFT SHOP":
                    return "Souvenirs for Sale";
                case "CHAPEL":
                    return "Whenever you enter CHAPEL, lose 1 gold";
                case "MAID'S CHAMBER":
                    return "Less likely to find items laying around in your house";
                case "ARCHIVES":
                    return "While drafting, you will no longer be able to see all 3 of your potential Floor Plans";
                case "GYMNASIUM":
                    return "Whenever you enter GYMNASIUM, lose 2 steps";
                case "DARKROOM":
                    return "You cannot see the Floor Plans while drafting in this room";
                case "WEIGHT ROOM":
                    return "Lose half your steps";
                case "FURNACE":
                    return "You are more likely to draw Red Rooms while drafting";
                case "DOVECOTE":
                    return "Whenever you draw DOVECOTE while drafting, you may rotate floorplans";
                case "THE KENNEL":
                    return "Each time you dig in a room, unlock all doors in that room";
                case "CLOCK TOWER":
                    return "Tomorrow, you will start with 1 key for each tomorrow room you draft today";
                case "CLASSROOM":
                    return "While drafting in CLASSROOM, you may draw new Floorplans one time for each Drafting room in your house";
                case "SOLARIUM":
                    return "You are more likely to draw Unusual and Rare rooms while drafting";
                case "DORMITORY":
                    return "Each time you enter DORMITORY after drafting a Drafting room, gain 10 steps";
                case "VESTIBULE":
                    return "Whenever you enter VESTIBULE, 3 of the doors in this room unlock and the fourth one becomes locked";
                case "CASINO":
                    return "Games of Chance";
                case "PLANETARIUM":
                    return "If you call it a day in PLANETARIUM, gain 2 stars";
                case "MECHANARIUM":
                    return "The MECHANARIUM has one door for each Mechanical room in your house";
                case "TREASURE TROVE":
                    return "+5 gold for each time you have drafted TREASURE TROVE";
                case "THRONE ROOM":
                    return "Reclaim the crown";
                case "TUNNEL":
                    return "Always draw TUNNEL while drafting in this room";
                case "CONSERVATORY":
                    return "Adjust the rarity of the Floorplans in your house";
                case "LOST AND FOUND":
                    return "2 rare items, Whenever you enter this room, you will lose one random inventory item";
                case "CLOSED EXHIBIT":
                    return "The doors in this room are security locked";
                case "TOOLSHED":
                    return "2 special items";
                case "BOMB SHELTER":
                    return "SHELTER protects you from the negative effects of the next 3 RED ROOMS drafted";
                case "SCHOOLHOUSE":
                    return "Adds 8 CLASSROOMS to today's Draft Pool";
                case "SHRINE":
                    return "Make an offering, Receive a blessing";
                case "ROOT CELLAR":
                    return "Rooms in your house will be more likely to contain dig spots today";
                case "HOVEL":
                    return "Special Floorplans now cost additional steps instead of gems";
                case "TRADING POST":
                    return "Items for Trade";
                case "TOMB":
                    return "Each time you draft a Dead End in your house, spread 5 gold in the TOMB";
                default:
                    return "";
            }
        }

        private static string GetRoomDescription(PlayMakerFSM prefabFSM) {
            string description = prefabFSM.FsmVariables.GetFsmString("DESCRIPTION").value;

            description = description.Replace("<i>", "").Replace("</i>", "");
            description = description.Replace("<size=3.2>", "");

            return description;
        }

        private static string GetRoomTypes(PlayMakerFSM prefabFsm) {
            string types = "";
            string fullTypeString = prefabFsm.FsmVariables.GetFsmString("TYPE").value;

            if (fullTypeString.Contains("Permanent"))
            {
                types += " Permanent,";
            }

            if (fullTypeString.Contains("Mechanical"))
            {
                types += " a Mechanical room,";
            }

            if (fullTypeString.Contains("Spread"))
            {
                types += " a Spread room,";
            }

            if (fullTypeString.Contains("Puzzle"))
            {
                types += " a Puzzle room,";
            }

            if (fullTypeString.Contains("Entry"))
            {
                types += " an Entry room,";
            }

            if (fullTypeString.Contains("Blueprint"))
            {
                types += " a Blueprint,";
            }

            if (fullTypeString.Contains("Hallway"))
            {
                types += " a Hallway,";
            }

            if (fullTypeString.Contains("Bedroom"))
            {
                types += " a Bedroom,";
            }

            if (fullTypeString.Contains("Green Room"))
            {
                types += " a Green Room,";
            }

            if (fullTypeString.Contains("Red Room"))
            {
                types += " a Red Room,";
            }

            if (fullTypeString.Contains("Shop"))
            {
                types += " a Shop,";
            }

            if (fullTypeString.Contains("Blackprint"))
            {
                types += " a Blackprint,";
            }

            if (fullTypeString.Contains("Outer Room"))
            {
                types += " an Outer Room,";
            }

            if (fullTypeString.Contains("Dead End"))
            {
                types += " a Dead End,";
            }

            if (fullTypeString.Contains("Upgrade"))
            {
                types += " an Upgrade,";
            }

            if (fullTypeString.Contains("Addition"))
            {
                types += " an Addition,";
            }

            if (fullTypeString.Contains("Tomorrow"))
            {
                types += " a Tomorrow room,";
            }

            if (fullTypeString.Contains("Drafting"))
            {
                types += " a Drafting room,";
            }

            if (fullTypeString.Contains("Objective"))
            {
                types += " an Objective,";
            }

            int commaCount = types.Split(",").Length - 1;

            if (commaCount == 0)
            {
                //Should not happen but just in case
                return "";
            }

            //Remove the last comma
            types = types.Remove(types.LastIndexOf(","), 1).Insert(types.LastIndexOf(","), "");

            //Replace the penultimate comma with an "and"
            if (commaCount > 1)
            {
                types = types.Remove(types.LastIndexOf(","), 1).Insert(types.LastIndexOf(","), " and");
            }

            return types;
        }

        private static int GetCost(PlayMakerFSM prefabFsm)
        {
            return prefabFsm.FsmVariables.GetFsmInt("GEM COST").value;
        }
    }
}
