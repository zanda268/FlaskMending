using Il2Cpp;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Delegate = Il2CppSystem.Delegate;

namespace FlaskMending
{
	internal class FMUtils
	{
		//Reference for the most recently selected item
		internal static GearItem restoreItem;

		//Returns an array of GearItems required to restore an item
		public static GearItem[] LookupRequiredRestoreGear(GearItem gi)
		{
			return new GearItem[] { GearItem.LoadGearItemPrefab("GEAR_ScrapMetal") };
		}

		//Returns an array of ints for how many GearItems are required to restore an item
		public static int[] LookupRequiredRestoreGearAmounts(GearItem gi)
		{
			int[] requiredAmount = { 2 };

			for (int i = 0; i < requiredAmount.Length; i++)
			{
				//TODO Add setting to modify how many materials are required.
				requiredAmount[i] *= Settings.options.restoreMaterialMultiplier;
			}

			return requiredAmount;
		}

		//Returns how long the restore should take
		public static float LookupRestoreDuration(GearItem gi)
		{
			return 60 * (IsUsingSimpleTools() ? 2f : 1f) * Settings.options.restoreDurationMultiplier; //TODO get repair skill modifier here?
		}

		//Rolls restore chance. Takes the base chance, adds the 20% boost, and then subtracts the debuff from the settings. Generates a random number based on that and clamps it between 0.2 and 1.0.
		public static float RollRestoreChance(GearItem gi)
		{
			float chanceToSucceed = (50 + 20 - Settings.options.restoreFailureDebuff + (IsUsingSimpleTools() ? 0 : 20)) / 100;
			return System.Math.Clamp(UnityEngine.Random.RandomRange(chanceToSucceed, 1 + chanceToSucceed), 0.2f, 1f);
		}

		//Checks if the GearItem is a valid restore target
		public static bool IsValidItem(GearItem gi)
		{
			if (gi == null) return false; //GearItem is null

			if (!gi.name.Contains("InsulatedFlask_")) return false; //GearItem not a flask.

			if (gi.CurrentHP > 495) return false; //Item is almost full hp.

			return true; //GearItem is valid
		}

		//Checks if player can restore the GearItem. Warns the player if they can't and "silent" is false.
		internal static bool CanRestore(GearItem gi, bool silent = true)
		{
			if (gi == null) return false; //GearItem is null

			//Check if the player has all the required restore materials in their inventory.
			GearItem[] requiredMaterials = LookupRequiredRestoreGear(gi);
			int[] numRequiredMaterials = LookupRequiredRestoreGearAmounts(gi);
			for (int i = 0; i < requiredMaterials.Length; i++) 
			{
				if (GameManager.GetInventoryComponent().NumGearInInventory(requiredMaterials[i].name, true) < numRequiredMaterials[i])
				{
					//Enable HintLabel telling player what materials they need to restore the GearItem
					if (!silent) FMButtons.UpdateHintLabel(GetTextFriendlyMissingMaterialsString(gi), true);

					return false;
				}
			}

			if (GameManager.GetInventoryComponent().NumGearInInventory("GEAR_SimpleTools", true) == 0 && GameManager.GetInventoryComponent().NumGearInInventory("GEAR_HighQualityTools", true) == 0)
			{
				//Enable HintLabel telling player what tools they need to restore the GearItem
				if (!silent) FMButtons.UpdateHintLabel("You need a Simple or High Quality Tools to repair this!", true);

				return false;
			}

			return true;
		}

		//Checks if a player is using a Fishing Tackle to restore an item
		internal static bool IsUsingSimpleTools()
		{
			return (GameManager.GetInventoryComponent().NumGearInInventory("GEAR_HighQualityTools", true) == 0);
		}

		//Returns a nicely formatted string of required repair materials
		internal static string GetTextFriendlyMissingMaterialsString(GearItem gi)
		{
			string s_requiredMaterials = "";

			GearItem[] requiredMaterials = LookupRequiredRestoreGear(gi);
			int[] numRequiredMaterials = LookupRequiredRestoreGearAmounts(gi);

			if (requiredMaterials.Length == 0)
			{
				//What?
				FlaskMending.Log("GetTextFriendlyMissingMaterialsString() : Required restore materials is empty.", ComplexLogger.FlaggedLoggingLevel.Error);
			}
			else if (requiredMaterials.Length == 1)
			{
				s_requiredMaterials = $"{gi.DisplayName} requires {numRequiredMaterials[0]} {requiredMaterials[0].DisplayName} to repair!";
			} 
			else if (requiredMaterials.Length == 2)
			{
				s_requiredMaterials = $"{gi.DisplayName} requires {numRequiredMaterials[0]} {requiredMaterials[0].DisplayName} and {numRequiredMaterials[1]} {requiredMaterials[1].DisplayName} to repair!";
			}
			else
			{
				s_requiredMaterials = $"{gi.DisplayName} requires ";

				for (int i = 0; i < requiredMaterials.Length - 1; i++)
				{
					s_requiredMaterials += $"{numRequiredMaterials[i]} {requiredMaterials[i].DisplayName},";
				}

				s_requiredMaterials += $", and {numRequiredMaterials[1]} {requiredMaterials[1].DisplayName} to repair!";
			}

			return s_requiredMaterials;
		}

		internal class LabelCoroutine : MonoBehaviour
		{
			internal System.Collections.IEnumerator DisplayHintLabel(float waitTime)
			{
				//TODO Add nice fade out for HintLabel

				yield return new WaitForSeconds(waitTime);

				FMButtons.UpdateHintLabel("",false);
			}
		}
	}
}
