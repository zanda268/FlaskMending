using Il2Cpp;
using MelonLoader;
using ModSettings;
using UnityEngine;

namespace FlaskMending
{
    internal class FlaskMendingSettings : JsonModSettings
    {        
        [Section("General")]

		[Name("Restore duration multipler")]
		[Description("Multiplier changing how long it takes to repair or restore a flask. Default of 1 would take 60 minutes if using High Quality Tools.")]
		[Slider(0f, 3f, 16, NumberFormat = "{0:0.#}x")]
		public float restoreDurationMultiplier = 1f;

		[Name("Restore material multipler")]
		[Description("Changes how many materials it takes to repair or restore a flask. Default is 2x of scrap metal.")]
		[Slider(0, 4, 1, NumberFormat = "{0:0}x")]
		public int restoreMaterialMultiplier = 1;

		[Name("Restore chance debuff")]
		[Description("Flat debuff to fail repairing or restoring a flask. Default of 0 will give you a 90% chance to repair if using High Quality tools.")]
		[Slider(0f, 100f, 101)]
		public float restoreFailureDebuff = 0f;


		protected override void OnConfirm()
        {
			base.OnConfirm();
        }
    }

    internal static class Settings
    {
        public static FlaskMendingSettings options;

        public static void OnLoad()
        {
            options = new FlaskMendingSettings();
            options.AddToModSettings("Flask Mending");
        }
    }
}
