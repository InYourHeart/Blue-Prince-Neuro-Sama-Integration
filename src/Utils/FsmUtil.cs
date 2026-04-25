using Il2Cpp;
using Il2CppHutongGames.PlayMaker;
using Il2CppHutongGames.PlayMaker.Actions;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils
{
    public class FsmUtil
    {
		public static string pathSeparator = "|";

		public static string GetTextMeshProText(GameObject gameObject, string defaultResponse)
		{
			try
			{
				return gameObject.GetComponent<TextMeshPro>().text;
			}
			catch (Exception)
			{
				return defaultResponse;
			}
		}

		public static string GetTextMeshProText(string gameObjectName, string defaultResponse)
        {
            try
            {
                return GetTextMeshProText(GameObject.Find(gameObjectName), defaultResponse);
            }
            catch (Exception) {
                return defaultResponse;
            }
        }

		public static string GetTextMeshProText(GameObject gameObject, string childName, string defaultResponse)
		{
			try
			{
				return GetTextMeshProText(GetChildGameObject(gameObject, childName), defaultResponse);
			}
			catch (Exception)
			{
				return defaultResponse;
			}
		}

		public static string GetTextMeshProText(string gameObjectName, string childName, string defaultResponse)
		{
			try
			{
				return GetTextMeshProText(GameObject.Find(gameObjectName), childName, defaultResponse);
			}
			catch (Exception)
			{
				return defaultResponse;
			}
		}

		public static void SetTextMeshProText(GameObject gameObject, string newText)
		{
			try
			{
				gameObject.GetComponent<TextMeshPro>().text = newText;
			}
			catch (Exception) {
				Melon<Core>.Logger.Msg($"Could not change TextMeshPro value of {gameObject.name} to {newText}");
			}
		}

		public static void SetTextMeshProText(GameObject gameObject, string childName, string newText)
		{
			SetTextMeshProText(GetChildGameObject(gameObject, childName), newText);
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
		public static GameObject GetChildGameObject(GameObject nextChild, string[] childPath)
		{
			for (int i = 0; i < childPath.Length; i++)
			{
				Transform parentTransform = nextChild.transform;

				for (int y = 0; y < parentTransform.childCount; y++)
				{
					nextChild = parentTransform.GetChild(y).gameObject;

					Melon<Core>.Logger.Msg("Child: \"" + childPath[i] + "\" - Next Child: \"" + nextChild.name + "\"");

					if (nextChild.name == childPath[i] && i == childPath.Length - 1) return nextChild;

					if (nextChild.name == childPath[i]) break;
				}
			}

			return null;
		}

		public static GameObject GetChildGameObject(GameObject parentObject, string childName)
		{
			try
			{
				return GetChildGameObject(parentObject, childName.Split(pathSeparator));
			}
			catch {
				Melon<Core>.Logger.Msg($"Did not find child \"{childName}\" of {parentObject.name}");
			}

			return null;
		}

		public static GameObject GetChildGameObject(string parentName, string childName)
		{
			try
			{
				return GetChildGameObject(GameObject.Find(parentName), childName);
			}
			catch
			{
				return null;
			}
		}

		public static PlayMakerFSM GetChildPlayMakerFSM(string parentName, string childName)
		{
			try
			{
				return GetChildGameObject(parentName, childName).GetComponent<PlayMakerFSM>();
			}
			catch {
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

		public static void ListFSMVariables(string gameObjectName)
		{
			PlayMakerFSM fsm = GetPlayMakerFSM(gameObjectName);

			if (fsm == null) {
				Melon<Core>.Logger.Msg($"Could not list FSMVariables for {gameObjectName} because no PlayMakerFSM was found");
				return;
			}
			
			Melon<Core>.Logger.Msg($"Listing all FSMVariables for {gameObjectName}");
			
			ListFSMInts(gameObjectName);
			ListFSMBools(gameObjectName);
			ListFSMStrings(gameObjectName);
			ListFSMGameObjects(gameObjectName);
		}

		public static void ListFSMInts(string gameObjectName)
		{
			PlayMakerFSM fsm = GetPlayMakerFSM(gameObjectName);

			if (fsm == null) return;

			for (int i = 0; i < fsm.FsmVariables.intVariables.Count; i++)
			{
				FsmInt fsmInt = fsm.FsmVariables.intVariables[i];

				if (fsmInt != null) Melon<Core>.Logger.Msg($"{gameObjectName} - FsmInt - {i} - {fsmInt.name} - {fsmInt.value}");
			}
		}

		public static void ListFSMBools(string gameObjectName)
		{
			PlayMakerFSM fsm = GetPlayMakerFSM(gameObjectName);

			if (fsm == null) return;

			for (int i = 0; i < fsm.FsmVariables.boolVariables.Count; i++)
			{
				FsmBool fsmBool = fsm.FsmVariables.boolVariables[i];

				if (fsmBool != null) Melon<Core>.Logger.Msg($"{gameObjectName} - FsmBool - {i} - {fsmBool.name} - {fsmBool.value}");
			}
		}

		public static void ListFSMStrings(string gameObjectName)
		{
			PlayMakerFSM fsm = GetPlayMakerFSM(gameObjectName);

			if (fsm == null) return;

			for (int i = 0; i < fsm.FsmVariables.stringVariables.Count; i++)
			{
				FsmString fsmString = fsm.FsmVariables.stringVariables[i];

				if (fsmString != null) Melon<Core>.Logger.Msg($"{gameObjectName} - FsmString - {i} - {fsmString.name} - {fsmString.value}");
			}
		}

		public static void ListFSMGameObjects(string gameObjectName)
		{
			PlayMakerFSM fsm = GetPlayMakerFSM(gameObjectName);

			if (fsm == null) return;

			for (int i = 0; i < fsm.FsmVariables.stringVariables.Count; i++)
			{
				FsmGameObject fsmGameObject = fsm.FsmVariables.gameObjectVariables[i];

				if (fsmGameObject != null) Melon<Core>.Logger.Msg($"{gameObjectName} - FsmGameObject - {i} - {fsmGameObject.name}");
			}
		}

		public static void ListFSMStates(string gameObjectName) {
			PlayMakerFSM fsm = GetPlayMakerFSM(gameObjectName);

			if (fsm == null) return;

			for (int i = 0; i < fsm.FsmStates.Count; i++)
			{
				FsmState state = fsm.FsmStates[i];

				if (state != null) Melon<Core>.Logger.Msg($"{gameObjectName} - {i} - {state.name}");
			}
		}

		public static FsmState GetFsmState(string gameObjectName, string stateName)
		{
			try
			{
				PlayMakerFSM fsm = GetPlayMakerFSM(gameObjectName);

				if (fsm == null) return null;

				for (int i = 0; i < fsm.FsmStates.Count; i++)
				{
					FsmState state = fsm.FsmStates[i];

					if (state != null && state.name == stateName) {
						return state;
					}
				}
			} catch {}

			return null;
		}

		public static FsmState GetFsmState(string gameObjectName, int stateIndex) {
			PlayMakerFSM fsm = GetPlayMakerFSM(gameObjectName);

			if (fsm == null) return null;

			if (stateIndex >= fsm.FsmStates.Count) return null;

			return fsm.FsmStates[stateIndex];
		}

		public static FsmState GetActiveFsmState(string gameObjectName)
		{
			PlayMakerFSM fsm = GetPlayMakerFSM(gameObjectName);

			return GetActiveFsmState(fsm);
		}

		public static FsmState GetActiveFsmState(PlayMakerFSM fsm)
		{
			if (fsm == null) return null;

			foreach (FsmState state in fsm.FsmStates)
			{
				if (state.active)
				{
					return state;
				}
			}

			return null;
		}
    }
}
