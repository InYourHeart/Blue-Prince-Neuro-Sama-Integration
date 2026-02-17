using Il2Cpp;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils
{
    public class FsmUtil
    {
        public static string GetTextMeshProText(string gameObjectName, string defaultResponse)
        {
            try
            {
                return GameObject.Find(gameObjectName).GetComponent<TextMeshPro>().text;
            }
            catch (Exception) {
                return defaultResponse;
            }
        }

        public static string GetFsmString(string gameObjectName, string variableName, string defaultResponse)
        {
            try
            {
                return GetPlayMakerFSM(gameObjectName).FsmVariables.GetFsmString(variableName).value;
            }
            catch (Exception)
            {
                return defaultResponse;
            }
        }

        public static int? GetFsmInt(string gameObjectName, string variableName) {
            try
            {
                return GetPlayMakerFSM(gameObjectName).FsmVariables.GetFsmInt(variableName).value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool? GetFsmBool(string gameObjectName, string variableName)
        {
            try
            {
                return GetPlayMakerFSM(gameObjectName).FsmVariables.GetFsmBool(variableName).value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static GameObject GetGameObject(string gameObjectName, string variableName)
        {
            try
            {
                return GetPlayMakerFSM(gameObjectName).FsmVariables.GetFsmGameObject(variableName).value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static PlayMakerFSM GetPlayMakerFSM(string gameObjectName) {
            try
            {
                return GameObject.Find(gameObjectName).GetComponent<PlayMakerFSM>();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
