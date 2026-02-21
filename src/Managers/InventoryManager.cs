using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers
{
    public class InventoryManager
    {
        public static string GetGold()
        {
            return FsmUtil.GetTextMeshProText("Gold #","0");
        }

        public static string GetKeys()
        {
            return FsmUtil.GetTextMeshProText("Key #","0");
        }

        public static string GetGems()
        {
            return FsmUtil.GetTextMeshProText("Gem #","0");
        }

        public static string GetSteps()
        {
            return FsmUtil.GetTextMeshProText("Steps #","0");
        }
        
        public static string GetInventoryContext()
        {
            return "You currently have " + GetGold() + " gold, " + GetKeys() + " key(s), " + GetGems() + " gem(s) and " + GetSteps() + " steps.";
        }

		public static string GetItemByID(int id)
		{
			//list taken from here https://www.reddit.com/r/BluePrince69/comments/1mv4i9h/savecrafting_catalogue/

			switch (id)
			{
				case 0: return "Basement Key";
				case 1: return "Battery Pack";
				case 2: return "Broken Lever";
				case 3: return "Burning Glass";
				case 4: return "Car Keys";
				case 5: return "Chronograph";
				case 6: return "Coin Purse";
				case 7: return "Compass";
				case 8: return "Coupon Book";
				case 9: return "Detector Shovel";
				case 10: return "Dowsing Rod";
				case 11: return "Emerald Bracelet";
				case 12: return "Ivory Dice"; //(does nothing since dice are normally a tally rather than an inventory item)
				case 13: return "Jack Hammer";
				case 14: return "Sanctum Key";
				case 15: return "Keycard";
				case 16: return "Lockpick";
				case 17: return "Lucky Purse";
				case 18: return "Lucky Rabbit's Foot";
				case 19: return "Magnifying Glass";
				case 20: return "Master Key";
				case 21: return "Metal Detector";
				case 22: return "Moon Pendant";
				case 23: return "Ornate Compass";
				case 24: return "Pick Sound Amplifier";
				case 25: return "Power Hammer";
				case 26: return "Electromagnet"; //(note: formerly Powered Electromagnet)
				case 27: return "Key 8"; //(note: probably formerly Room 8 Key)
				case 28: return "Running Shoes";
				case 29: return "Salt Shaker";
				case 30: return "Secret Garden Key";
				case 31: return "Shovel";
				case 32: return "Silver Key";
				case 33: return "Silver Spoon";
				case 34: return "Sledgehammer";
				case 35: return "Sleeping Mask";
				case 36: return "Treasure Map";
				case 37: return "Vault Key 149";
				case 38: return "Vault Key 233";
				case 39: return "Vault Key 304";
				case 40: return "Vault Key 370";
				case 41: return "Wind case Up Key";
				case 42: return "Upgrade Disk";
				case 43: return "Stop Watch";
				case 44: return "Hallpass";
				case 45: return "Repellent";
				case 46: return "Gear Wrench";
				case 47: return "Telescope";
				case 48: return "Watering Can";
				case 49: return "Morning Star";
				case 50: return "Self Igniting Torch";
				case 51: return "Knight's Shield";
				case 52: return "The Axe";
				case 53: return "Micro Chip";
				case 54: return "Micro Chip";
				case 55: return "Micro Chip";
				case 56: return "Diary Key";
				case 57: return "Lunch Box";
				case 58: return "Cursed Effigy";
				case 59: return "File Cabinet Key";
				case 60: return "File Cabinet Key";
				case 61: return "Paper Crown";
				case 62: return "Crown of the Blueprints";
				case 63: return "Royal Scepter";
				case 64: return "Prism Key";
				case 65: return "Key of Aries";
				case 1000:
				default: return "";
			}
		}
    }
}
