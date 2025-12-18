using MelonLoader;
using UnityEngine;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection;
using System.Collections;
using Il2Cpp;
using Il2CppNewtonsoft.Json.Linq;
using HarmonyLib;


namespace RuinedMending
{
	//Lets us grab a copy of an existing button to copy later.
	[HarmonyPatch(typeof(Panel_Inventory), nameof(Panel_Inventory.Initialize))]
	internal class RuinedMendingInitialization
	{
		private static void Postfix(Panel_Inventory __instance)
		{
			RuinedMending.Log("Patches.RuinedMendingIntialization : Initializing RM panel");
			RMButtons.InitializeRM(__instance.m_ItemDescriptionPage);
		}
	}

	//Updates selected GearItem so we know which item we are restoring.
	[HarmonyPatch(typeof(ItemDescriptionPage), nameof(ItemDescriptionPage.UpdateGearItemDescription))]
	internal class UpdateRestoreItemButton
	{
		private static void Postfix(ItemDescriptionPage __instance, GearItem gi)
		{
			if (__instance != InterfaceManager.GetPanel<Panel_Inventory>()?.m_ItemDescriptionPage) return;

			if (RMUtils.IsValidItem(gi))
			{
				RMButtons.SetRestoreItemButtonActive(true);
				RMUtils.restoreItem = gi;

				if (RMUtils.CanRestore(gi))
				{
					RMButtons.SetRestoreItemButtonErrored(false);
				} else
				{
					RMButtons.SetRestoreItemButtonErrored(true);
				}
			}
			else
			{
				RMButtons.SetRestoreItemButtonActive(false);
			}
		}
	}

	//DEBUG
	[HarmonyPatch(typeof(ConsoleManager), nameof(ConsoleManager.Initialize))]
	internal class ConsoleManagerPatches_Initialize
	{
		private static void Postfix()
		{
			uConsole.RegisterCommand("rmdebug", new Action(ConsoleCommands.Console_OnCommand));
		}
	}


	internal class ConsoleCommands
	{
		internal static void Console_OnCommand()
		{
			string[] gearList = { "GEAR_DeerSkinPants", "GEAR_SewingKit" };

			foreach (string gearName in gearList)
			{
				GearItem gi = GameManager.GetInventoryComponent().GetHighestConditionGearThatMatchesName(gearName);

				if (gi != null)
				{
					gi.m_CurrentHP = 0;
					gi.ForceWornOut();
				}
			}
		}
	}
}


