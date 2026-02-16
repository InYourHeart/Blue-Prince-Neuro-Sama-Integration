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
    }
}
