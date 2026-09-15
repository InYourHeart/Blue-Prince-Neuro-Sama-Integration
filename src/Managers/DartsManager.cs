using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions.Darts;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;
using Il2Cpp;
using Il2CppHutongGames.PlayMaker;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Messages.Outgoing;
using UnityEngine;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers
{
	public class DartsManager
	{
		public static string dartState = "";
		public static string aPrev = "";
		public static string ePrev = "";

		public static bool isPlayingDarts = false;

		private static ActionWindow dartsActionWindow;

		public static void CheckState()
		{
			FsmState dartsState = FsmUtil.GetActiveFsmState("Dartboard System");
			if (dartsState != null && dartState != dartsState.name)
			{
				dartState = dartsState.name;
				Melon<Core>.Logger.Msg("Darts - " + dartState);

				if (dartState.Contains("Switch "))
				{
					//Correct number picked
				} else if (dartState.Contains("Holding"))
				{
					//Animation finished, ready for next solution
					RegisterActions("The answer was correct, starting next stage.");
				} else
				{
					switch (dartState)
					{
						case "State 1":
							if (isPlayingDarts)
							{
								//Can act after a failure. Also the first active state when the room gets drafted
								RegisterActions("The answer was wrong, starting from the first stage.");
							}
							break;
						case "THE BIG RESET":
							//Wrong number picked
							break;
					}
				}
			}

			FsmState a = FsmUtil.GetActiveFsmState(FsmUtil.GetChildPlayMakerFSM("Dartboard System", "Darts - First Collider"));
			if (a != null && aPrev != a.name)
			{
				aPrev = a.name;

				Melon<Core>.Logger.Msg("Enter - " + aPrev);

				switch (aPrev)
				{
					case "Camera Move 2": isPlayingDarts = true; break;
					case "Wait for Exit 2":
						//Send first action
						RegisterActions("A new Dartboard puzzle has begun.");
						break;
					case "State 3":
						//Manually exited darts
						Exit();
						break;
					case "State 1":
						//Completed puzzle
						Context.Send("Dartboard completed!");
						Exit();
						break;
				}
			}

			FsmState e = FsmUtil.GetActiveFsmState(GetExitFSM());
			if (e != null && ePrev != e.name)
			{
				ePrev = e.name;
				Melon<Core>.Logger.Msg("Exit - " + ePrev);
			}
		}

		public static PlayMakerFSM GetExitFSM()
		{
			return FsmUtil.GetPlayMakerFSM("Darts - Exit Collider");
		}

		public static string GetCurrentStage()
		{
			GameObject dartboard = FsmUtil.GetChildGameObject("Billiard Room(Clone)001", "_GAMEPLAY|Dartboard|Dartboard System|Dartboard New");

			string bullseyeDescription = "";
			string[] middleRingsDescription = ["","","",""];
			string externalRingDescription = "";

			GameObject bullseyeColorSection = FsmUtil.GetChildGameObject(dartboard, "BULLSEYE|COLORS");
			for (int i = 0; i < bullseyeColorSection.transform.childCount; i++)
			{
				GameObject bullseyeColor = bullseyeColorSection.transform.GetChild(i).gameObject;

				if (!bullseyeColor.active) continue;

				if (bullseyeColor.name == "B") continue;

				switch (bullseyeColor.name)
				{
					case "A":
						bullseyeDescription += "Blue";
						break;
					case "D":
						bullseyeDescription += "Purple";
						break;
					case "M":
						bullseyeDescription += "Pink";
						break;
					case "S":
						bullseyeDescription += "Yellow";
						break;
				}
				break;
			}

			GameObject bullseyeTypesSection = FsmUtil.GetChildGameObject(dartboard, "BULLSEYE|TYPES");
			for (int i = 0; i < bullseyeTypesSection.transform.childCount; i++)
			{
				GameObject bullseyeTypes = bullseyeTypesSection.transform.GetChild(i).gameObject;

				if (!bullseyeTypes.active) continue;

				if (bullseyeTypes.name == "E") continue;

				bullseyeDescription += ", ";

				switch (bullseyeTypes.name)
				{
					case "S":
						bullseyeDescription += "Square";
						break;
					case "V":
						bullseyeDescription += "Diamond";
						break;
					case "O":
						bullseyeDescription += "Single Wave";
						break;
					case "T":
						bullseyeDescription += "Double Wave";
						break;
					case "H":
						bullseyeDescription += "Triple Wave";
						break;
					case "D":
						bullseyeDescription += "Double Square";
						break;
					case "Q":
						bullseyeDescription += "Big Square, Small Diamond";
						break;
					case "W":
						bullseyeDescription += "Big Diamond, Small Square";
						break;
					case "A":
						bullseyeDescription += "Big Single Wave, Small Square";
						break;
					case "B":
						bullseyeDescription += "Big Double Wave, Small Square";
						break;
					case "C":
						bullseyeDescription += "Big Triple Wave, Small Square";
						break;
					case "1":
						bullseyeDescription += "Big Square, Small Single Wave";
						break;
					case "2":
						bullseyeDescription += "Big Square, Small Double Wave";
						break;
					case "3":
						bullseyeDescription += "Big Square, Small Triple Wave";
						break;
					case "4":
						bullseyeDescription += "Big Single Wave, Small Diamond";
						break;
					case "5":
						bullseyeDescription += "Big Double Wave, Small Diamond";
						break;
					case "6":
						bullseyeDescription += "Big Triple Wave, Small Diamond";
						break;
					case "7":
						bullseyeDescription += "Big Diamond, Small Single Wave";
						break;
					case "8":
						bullseyeDescription += "Big Diamond, Small Double Wave";
						break;
					case "9":
						bullseyeDescription += "Big Diamond, Small Triple Wave";
						break;
				}

				bullseyeDescription += ";";

				break;
			}

			for (int number = 1; number <= 20; number++)
			{
				Transform numberSection = dartboard.transform.GetChild(number + 2);

				//External Ring
				Transform externalRing = numberSection.GetChild(4);
				if (externalRing.GetChild(0).gameObject.active) {
					externalRingDescription += "diagonal line on " + number + ", ";
				} else if (externalRing.GetChild(1).gameObject.active)
				{
					externalRingDescription += "two dots on " + number + ", ";
				} else if (externalRing.GetChild(2).gameObject.active)
				{
					externalRingDescription += "diamond symbol on " + number + ", ";
				} else if (externalRing.GetChild(3).gameObject.active)
				{
					externalRingDescription += "cross on " + number + ", ";
				} else if (externalRing.GetChild(4).gameObject.active)
				{
					externalRingDescription += "diamond with a slash on " + number + ", ";
				}

				for (int i = 0; i < 4; i++)
				{
					Transform section = numberSection.GetChild(i);

					if (section.GetChild(2).gameObject.active) continue; //Section not active

					if (section.GetChild(1).gameObject.active)
					{
						middleRingsDescription[i] += "thin line of ";
					}

					if (section.GetChild(0).gameObject.active)
					{
						middleRingsDescription[i] += "blue on " + number + ", ";
					}
					else if (section.GetChild(3).gameObject.active)
					{
						middleRingsDescription[i] += "purple on " + number + ", ";
					}
					else if (section.GetChild(4).gameObject.active)
					{
						middleRingsDescription[i] += "pink on " + number + ", ";
					}
					else if (section.GetChild(5).gameObject.active)
					{
						middleRingsDescription[i] += "yellow on " + number + ", ";
					}
				}
			}

			string stageContext = "\n## Dartboard layout";
			stageContext += (bullseyeDescription != "") ? "\n* Bullseye: " + bullseyeDescription : "";
			stageContext += (middleRingsDescription[0] != "") ? "\n* First Ring: " + middleRingsDescription[0] : "";
			stageContext += (middleRingsDescription[1] != "") ? "\n* Second Ring: " + middleRingsDescription[1] : "";
			stageContext += (middleRingsDescription[2] != "") ? "\n* Third Ring: " + middleRingsDescription[2] : "";
			stageContext += (middleRingsDescription[3] != "") ? "\n* Fourth Ring: " + middleRingsDescription[3] : "";
			stageContext += (externalRingDescription != "") ? "\n* External Ring: " + externalRingDescription : "";

			return stageContext;
		}

		public static void PickAnswer(int answer)
		{
			string answerObjectName = "" + answer;
			if (answer == 5 || answer == 6 || answer == 7 || answer == 8)
			{
				answerObjectName += " ";
			}
			answerObjectName += "holder";

			PlayMakerFSM holder = FsmUtil.GetChildPlayMakerFSM("Billiard Room(Clone)001","_GAMEPLAY|Dartboard|Dartboard System|" + answerObjectName + "|" + answer);

			FsmState activeState = FsmUtil.GetActiveFsmState(holder);

			if (activeState.Name == "Prestate")
			{
				holder.SendEvent("hov");
			}

			holder.SendEvent("click");
		}

		public static void RegisterActions(string context)
		{
			dartsActionWindow = ActionWindow.Create();
			dartsActionWindow.SetContext(context + GetCurrentStage());
			dartsActionWindow.AddAction(new ChooseDartboardStageAnswerAction());
			dartsActionWindow.AddAction(new ExitDartboardAction());
			dartsActionWindow.Register();
		}

		public static void Exit()
		{
			isPlayingDarts = false;
		}
	}
}
