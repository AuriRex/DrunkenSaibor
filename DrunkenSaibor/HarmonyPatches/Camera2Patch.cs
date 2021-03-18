using DrunkenSaibor.Managers;
using HarmonyLib;
using IPA.Loader;
using System.Reflection;
using UnityEngine;

namespace DrunkenSaibor.HarmonyPatches
{

    [HarmonyPatch]
    internal class Camera2Patch
    {
        private static MethodBase TargetMethod()
        {
            PluginMetadata cameraTwo = PluginManager.GetPluginFromId("Camera2");
            return cameraTwo?.Assembly.GetType("Camera2.Behaviours.Cam2").GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);
        }

        private static void Postfix(MonoBehaviour __instance)
        {
            //UCamera
            // Cam2 created
            Logger.log.Notice($"Camera2 behaviour inited! {__instance.GetType()}");
            var cam = __instance.GetType().GetProperty("UCamera", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(__instance, null) as Camera;
            if(cam == null)
            {
                Logger.log.Error("UCamera is null!!!");
                return;
            }
            cam.gameObject.AddComponent<CameraNuisanceController>();
        }
    }
    
}
