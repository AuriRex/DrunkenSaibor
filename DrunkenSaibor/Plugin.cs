using DrunkenSaibor.Configuration;
using DrunkenSaibor.Installers;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;

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
        }

        [OnEnable]
        public void OnEnable()
        {
            //harmony = new Harmony(HARMONY_ID);
            //harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.log.Debug($"{Name} enabled!");
        }

        [OnDisable]
        public void OnDisable()
        {
            //harmony.UnpatchAll(HARMONY_ID);
            Logger.log.Debug($"{Name} disabled!");
        }
    }
}
