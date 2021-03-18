using DrunkenSaibor.Configuration;
using DrunkenSaibor.Data.Nuisances;
using DrunkenSaibor.Managers;
using DrunkenSaibor.Providers;
using SiraUtil.Interfaces;
using Zenject;

namespace DrunkenSaibor.Installers
{
    class DSCoreInstaller : Installer<DSCoreInstaller>
    {
        private readonly PluginConfig _pluginConfig;

        [Inject]
        public DSCoreInstaller(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
        }

        public override void InstallBindings()
        {
            Container.Bind<PluginConfig>().FromInstance(_pluginConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<DSAssetLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<NuisanceManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<NIntensityMappings>().AsSingle();

            Container.Bind<SmoothCameraWrapper>().AsSingle();

            Container.Bind(typeof(IModelProvider), typeof(NoteInfoProvider)).To<NoteInfoProvider>().AsSingle();

            //Container.Bind<RotateCamera>().FromNewComponentOn(_smoothCamera.gameObject).AsSingle().NonLazy();
        }
    }
}
