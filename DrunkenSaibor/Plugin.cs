using DrunkenSaibor.Configuration;
using DrunkenSaibor.Installers;
using DrunkenSaibor.Util;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using SiraUtil.Zenject;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IPALogger = IPA.Logging.Logger;

namespace DrunkenSaibor
{

    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static string Name => "DrunkenSaibor";

        public const string HARMONY_ID = "com.aurirex.drunkensaibor";

        [Init]
        public void Init(IPALogger logger, Config config, Zenjector zenjector, PluginMetadata meta)
        {
            Logger.log = logger;

            if (PluginManager.GetPluginFromId("ShaderExtensions") == null)
            {
                if (!LoadShaderDescriptors()) return;
            }

            zenjector.OnApp<DSCoreInstaller>().WithParameters(config.Generated<PluginConfig>());
            zenjector.OnGame<DSGameInstaller>(false).ShortCircuitForTutorial();
            zenjector.OnMenu<DSMenuInstaller>();
        }

        [OnEnable]
        public void OnEnable() => Logger.log.Debug($"{Name} enabled!");
        
        [OnDisable]
        public void OnDisable() => Logger.log.Debug($"{Name} disabled!");

        /// <summary>
        /// Load the ShaderEffect component
        /// from the ShaderExtensions namespace so that it can be accessed
        /// </summary>
        private bool LoadShaderDescriptors()
        {
            try
            {
                Assembly.Load(Utils.LoadFromResource("DrunkenSaibor.Resources.ShaderExtensionsComponents.dll"));
                return true;
            }
            catch (Exception)
            {
                Logger.log.Info("Couldn't load shader descriptors");
                return false;
            }
        }
    }
}
