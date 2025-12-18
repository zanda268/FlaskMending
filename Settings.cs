using UnityEngine;
using ModSettings;
using MelonLoader;

namespace RuinedMending
{
    internal class RuinedMendingSettings : JsonModSettings
    {        
        [Section("General")]
     
        [Name("Enabled")]
        [Description("Enable/disable complete mod functionality")]
        public bool modEnabled = true;

		[Name("Restore duration multipler")]
		[Description("Changes how long it takes to restore a ruined item based on the normal mending time. Default is 2x normal mend time.")]
		[Slider(0f, 3f, 16)]
		public float restoreDurationMultiplier = 2f;

		[Name("Restore material multipler")]
		[Description("Changes how many materials it takes to restore a ruined item based on the normal mending amount. Default is 2x normal mending materials.")]
		[Slider(0, 4, 1)]
		public int restoreMaterialMultiplier = 2;

		[Name("Restore chance debuff")]
		[Description("Flat debuff to fail restoring a ruined item on top of normal mending failure chance. Default is a 30% debuff. 0% will not increase failure chance at all.")]
		[Slider(0f, 100f, 101)]
		public float restoreFailureDebuff = 30f;

		[Name("Restored Item Condition")]
		[Description("Item's condition if the restore succeeds. Default is 10%")]
		[Slider(1f, 100f, 100)]
		public float restoredItemCondition = 10f;


		protected override void OnConfirm()
        {
			base.OnConfirm();
        }
    }

    internal static class Settings
    {
        public static RuinedMendingSettings options;

        public static void OnLoad()
        {
            options = new RuinedMendingSettings();
            options.AddToModSettings("Ruined Mending");
        }
    }
}
