using ComplexLogger;
using Il2Cpp;
using Il2CppInterop;
using Il2CppInterop.Runtime.Injection; 
using MelonLoader;
using System.Collections;
using UnityEngine;

namespace FlaskMending
{
	public class FlaskMending : MelonMod
	{
		internal static ComplexLogger<FlaskMending> Logger = new();


		public override void OnInitializeMelon()
		{
            Settings.OnLoad();
		}

		internal static void Log(String str, ComplexLogger.FlaggedLoggingLevel level = FlaggedLoggingLevel.Always)
		{
			Logger.Log(str, level);
		}
	}
}