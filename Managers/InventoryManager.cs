using Il2CppTMPro;
using UnityEngine;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.Managers
{
    public class InventoryManager
    {
        public static string goldAmount = "0";
        public static string keyAmount = "0";
        public static string gemAmount = "0";
        public static string stepAmount = "0";

        public static string GetInventoryContext()
        {
            GameObject goldGameObject = GameObject.Find("Gold #");
            if (goldGameObject != null)
            {
                goldAmount = goldGameObject.GetComponent<TextMeshPro>().text;
            }

            GameObject keyGameObject = GameObject.Find("Key #");
            if (keyGameObject != null)
            {
                keyAmount = keyGameObject.GetComponent<TextMeshPro>().text;
            }

            GameObject gemGameObject = GameObject.Find("Gem #");
            if (gemGameObject != null)
            {
                gemAmount = gemGameObject.GetComponent<TextMeshPro>().text;
            }

            GameObject stepsGameObject = GameObject.Find("Steps #");
            if (stepsGameObject != null)
            {
                stepAmount = stepsGameObject.GetComponent<TextMeshPro>().text;
            }

            return "You currently have " + goldAmount + " gold, " + keyAmount + " key(s), " + gemAmount + " gem(s) and " + stepAmount + " steps.";
        }
    }
}
