using Blue_Prince_Neuro_Sama_Integration_Mod.src.Actions;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Rooms;
using Blue_Prince_Neuro_Sama_Integration_Mod.src.Utils;
using MelonLoader;
using NeuroSDKCsharp.Actions;
using NeuroSDKCsharp.Messages.Outgoing;
using UnityEngine;

namespace Blue_Prince_Neuro_Sama_Integration_Mod.src.Managers
{
    public class DraftManager
    {
        public static FloorPlan[] draftedFloorPlans = new FloorPlan[3];
        public static bool isDrafting = false;
		public static bool isRedrawing = false;
		public static bool isRotating = false;
		public static bool isFailedRotation = false;

		private static ActionWindow draftActionWindow;

		public static void StartRotation()
		{
			if (!isRotating)
			{
				Melon<Core>.Logger.Msg($"Rotation started");

				isRotating = true;
			}
		}

		public static void EndRotation()
		{
			Melon<Core>.Logger.Msg($"Rotation ended");

			draftActionWindow = ActionWindow.Create();
			SetDraftingContext();
			RegisterActions();

			isRotating = false;
			isFailedRotation = false;
		}

		public static void StartFailRotation()
		{
			Melon<Core>.Logger.Msg($"Rotation fail start");

			Context.Send("Rotation failed because none of the floor plans can be rotated. The draft remains the same.", false);

			isFailedRotation = true;
		}

		public static void EndFailRotation()
		{
			Melon<Core>.Logger.Msg($"Rotation fail end");

			isFailedRotation = false;

			draftActionWindow = ActionWindow.Create();
			RegisterActions();
		}

		public static void StartRedraw()
		{
			if (!isRedrawing)
			{
				Melon<Core>.Logger.Msg($"Redrawing started");

				isRedrawing = true;
			}
		}

		public static void EndRedraw()
		{
			Melon<Core>.Logger.Msg($"Redrawing ended");

			draftActionWindow = ActionWindow.Create();
			SetDraftingContext();
			RegisterActions();

			isRedrawing = false;
		}

        public static void StartDraft()
        {
			Melon<Core>.Logger.Msg($"Drafting started.");

			isDrafting = true;

			draftActionWindow = ActionWindow.Create();
			SetDraftingContext();
			RegisterActions();
		}

		public static void EndDraft()
		{
			//Redundancy check just in case of a race condition. Call it the fool's semaphore
			if (isDrafting)
			{
				Melon<Core>.Logger.Msg($"Drafting ended.");

				isDrafting = false;
			}
		}

		public static void RegisterActions()
		{
			draftActionWindow.AddAction(new ChooseFloorPlanAction());

			//TODO Crown of the Blueprints is handled differently
			AddDraftingAbility(new PickBerryAction());
			AddDraftingAbility(new OrnateCompassAction());
			AddDraftingAbility(new DovecoteAction());
			AddDraftingAbility(new RotundaAction());
			AddDraftingAbility(new DancerAction());
			AddDraftingAbility(new ClassAction());
			AddDraftingAbility(new CrownAction());
			AddDraftingAbility(new DrawingAction());
			AddDraftingAbility(new InkwellAction());
			AddDraftingAbility(new RookAction());
			AddDraftingAbility(new StudyAction());
			AddDraftingAbility(new CronographAction());
			AddDraftingAbility(new KnightsShieldAction());
			if (!InventoryManager.GetIvoryDice().Equals("0")) draftActionWindow.AddAction(new IvoryDiceAction());

			draftActionWindow.Register();
		}

		public static void SetDraftingContext()
		{
			string draftingContext = "";

			draftingContext += UpdateDraftingContext("1");
			draftingContext += UpdateDraftingContext("2");
			draftingContext += UpdateDraftingContext("3");

			draftActionWindow.SetContext(draftingContext, false);
		}

		private static void AddDraftingAbility(DraftingAbilityAction action)
		{
			GameObject textObject = FsmUtil.GetChildGameObject(action.GAME_OBJECT_NAME, "TEXT");

			if (textObject != null && textObject.gameObject.active) {
				draftActionWindow.AddAction(action);
			}
		}

		private static FloorPlan GetDraftedFloorPlan(string slot)
        {
            try
            {
                FloorPlan floorPlan = new FloorPlan("PLAN" + slot + " - ENGINE");

                int? floorPlanRotation = FsmUtil.GetFsmInt("PLAN MANAGEMENT", "PLAN" + slot + " - ROTATION AMOUNT");
                int rotation = floorPlanRotation == null ? 0 : (int) floorPlanRotation / 90;

                floorPlan.doorLayout = DoorLayout.GetDoorLayout(floorPlan.name, rotation);

                draftedFloorPlans[int.Parse(slot) - 1] = floorPlan;

                return floorPlan;
            } catch (Exception)
            {
                Melon<Core>.Logger.Error($"Could not retrieve the floor plan object for slot " + slot + "!");
                return null;
            }
        }

        public static bool IsHovelActive()
        {
            return FsmUtil.GetFsmBool("DRAFT UI","hovel") == null ? false : (bool) FsmUtil.GetFsmBool("DRAFT UI", "hovel");
        }

        public static bool IsArchived(string slot)
        {
            return FsmUtil.GetFsmInt("PLAN MANAGEMENT", "ArchivedPick") == int.Parse(slot);
        }

		private static string StartingRedrawContext()
		{
			if (GridFSMManager.TargetRank() == null || GridFSMManager.TargetTile() == null)
			{
				Melon<Core>.Logger.Error($"Could not obtain the draft's target rank or tile while building the starting redraw text!");
				return "";
			}

			int targetRank = (int)GridFSMManager.TargetRank();
			int targetTile = (int)GridFSMManager.TargetTile();



			return "A redraw for the draft for Rank " + targetRank + ", Tile " + targetTile + " has begun.\n";
		}


		private static string StartingDraftContext()
        {
            if (GridFSMManager.TargetRank() == null || GridFSMManager.TargetTile() == null)
            {
                Melon<Core>.Logger.Error($"Could not obtain the draft's target rank or tile while building the starting draft text!");
                return "";
            }

            int targetRank = (int)GridFSMManager.TargetRank();
            int targetTile = (int)GridFSMManager.TargetTile();

			return "A draft for Rank " + targetRank + ", Tile " + targetTile + " has begun.\n";
        }

		private static string StartingOuterDraftContext()
		{
			return "A draft for the Outer Room has begun.\n";
		}


		private static string SlotNumberContext(string slot)
        {
            return slot + ". ";
        }
        private static string ArchivedFloorPlanContext(FloorPlan floorPlanInSlot)
        {
            return "An archived floor plan\n";
        }

        private static string FloorPlanContext(FloorPlan floorPlanInSlot)
        {
            string floorPlanText = "";

            floorPlanText += FloorPlanNameContext(floorPlanInSlot);

            floorPlanText += FloorPlanRarityContext(floorPlanInSlot);

            if (floorPlanInSlot.effect != "")
            {
                floorPlanText += FloorPlanEffectContext(floorPlanInSlot);
            }

            floorPlanText += FloorPlanTypesContext(floorPlanInSlot);

            //Don't look for Outer Room's door layout
            if (!floorPlanInSlot.isOuter)
            {
                floorPlanText += "\t* Unblocked doors: " + floorPlanInSlot.doorLayout.GetDraftingContext() + "\n";
            }

            return floorPlanText;
        }

        private static string FloorPlanNameContext(FloorPlan floorPlanInSlot)
        {
            return floorPlanInSlot.name + "\n";
        }

        private static string FloorPlanRarityContext(FloorPlan floorPlanInSlot)
        {
            return "\t* Rarity: " + floorPlanInSlot.rarity + ";\n";
        }

        private static string FloorPlanEffectContext(FloorPlan floorPlanInSlot)
        {
            return "\t* Effect: " + floorPlanInSlot.effect + ";\n";
        }

        private static string FloorPlanTypesContext(FloorPlan floorPlanInSlot)
        {
            return "\t* Type:" + floorPlanInSlot.types + ";\n";
        }

        private static string FloorPlanCostContext(FloorPlan floorPlanInSlot)
        {
            if (floorPlanInSlot.cost <= 0) return "";

            int cost = (IsHovelActive() ? floorPlanInSlot.cost * 3 : floorPlanInSlot.cost);
            string resource = (IsHovelActive() ? "steps" : cost == 1 ? "gem" : "gems");

            return "\t* Cost: " + cost + " " + resource + ";\n";
        }

		private static string UpdateDraftingContext(string slot)
        {
            FloorPlan floorPlanInSlot = GetDraftedFloorPlan(slot);

            if (floorPlanInSlot == null) {
                return "";
            }

            string draftingContext = "";

			if (slot.Equals("1"))
			{
				if (floorPlanInSlot.isOuter)
				{
					draftingContext += StartingOuterDraftContext();
				} else if (isRedrawing)
				{
					draftingContext += StartingRedrawContext();
				} else
				{
					draftingContext += StartingDraftContext();
				}

				draftingContext += "Three floor plans may be chosen from:\n";
			}

            draftingContext += SlotNumberContext(slot);

            if (IsArchived(slot))
            {
                draftingContext += ArchivedFloorPlanContext(floorPlanInSlot);
            } else
            {
                draftingContext += FloorPlanContext(floorPlanInSlot);
            }

			if (floorPlanInSlot.name.ToUpper().Equals("COAT CHECK"))
			{
				draftingContext += CoatCheckManager.GetCoatCheckContext(slot);
			}

            draftingContext += FloorPlanCostContext(floorPlanInSlot);

            if (slot.Equals("3"))
            {
                draftingContext += InventoryManager.GetInventoryContext();
				draftingContext += "Remember that if you don't like the current draft, you may be able to use an action to change it.";
            }

            if (draftingContext.LastIndexOf(";") != -1) {
                draftingContext = draftingContext.Remove(draftingContext.LastIndexOf(";"), 1).Insert(draftingContext.LastIndexOf(";"), ".");
            }

            return draftingContext;
        }
    }
}
