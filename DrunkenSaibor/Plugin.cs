using DrunkenSaibor.Configuration;
using DrunkenSaibor.Installers;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace DrunkenSaibor
{

    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static string Name => "DrunkenSaibor";

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
        public void OnEnable() => Logger.log.Debug($"{Name} enabled!");

        [OnDisable]
        public void OnDisable() => Logger.log.Debug($"{Name} disabled!");
    }
}
