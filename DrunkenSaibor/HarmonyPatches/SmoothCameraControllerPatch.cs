using HarmonyLib;
using System;

namespace DrunkenSaibor.HarmonyPatches
{
    [HarmonyPatch(typeof(SmoothCameraController), "Start")]
    class SmoothCameraControllerPatch
    {
        public static event Action<SmoothCameraController, SmoothCamera> OnPostSmoothCameraControllerInit;

        static bool isInited = false;
        static void Postfix(SmoothCameraController __instance, SmoothCamera ____smoothCamera)
        {
            if (isInited) return;
            isInited = true;
            OnPostSmoothCameraControllerInit?.Invoke(__instance, ____smoothCamera);
        }
    }
}
