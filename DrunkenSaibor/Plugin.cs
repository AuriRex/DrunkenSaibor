using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine.SceneManagement;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using DrunkenSaibor.Configuration;
using DrunkenSaibor.Installers;
using HarmonyLib;
using System.Reflection;
using DrunkenSaibor.HarmonyPatches;
using DrunkenSaibor.Managers;

namespace DrunkenSaibor
{

    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static string Name => "DrunkenSaibor";
        private Harmony harmony;

        public const string HARMONY_ID = "com.aurirex.drunkensaibor";

        [Init]
        public void Init(IPALogger logger, Config config, Zenjector zenjector)
        {
            Logger.log = logger;
            zenjector.OnApp<DSCoreInstaller>().WithParameters(config.Generated<PluginConfig>());
            zenjector.OnGame<DSGameInstaller>().ShortCircuitForTutorial();
            zenjector.OnMenu<DSMenuInstaller>();
            SmoothCameraControllerPatch.OnPostSmoothCameraControllerInit += SmoothCameraWrapper.SetSCC;
        }

        [OnEnable]
        public void OnEnable()
        {
            harmony = new Harmony(HARMONY_ID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.log.Debug($"{Name} enabled!");
        }

        [OnDisable]
        public void OnDisable()
        {
            harmony.UnpatchAll(HARMONY_ID);
            Logger.log.Debug($"{Name} disabled!");
        }
    }
}
