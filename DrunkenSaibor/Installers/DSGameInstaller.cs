using DrunkenSaibor.Configuration;
using DrunkenSaibor.Managers;
using Zenject;

namespace DrunkenSaibor.Installers
{
    class DSGameInstaller : Installer<DSGameInstaller>
    {
        private readonly PluginConfig _pluginConfig;

        public DSGameInstaller(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
        }

        public override void InstallBindings()
        {
            if (!_pluginConfig.Enabled) return;

            Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<NuisanceGameController>().AsSingle();
        }
    }
}
