using DrunkenSaibor.Data.Nuisances;
using DrunkenSaibor.Managers;
using Zenject;

namespace DrunkenSaibor.Installers
{
    class DSGameInstaller : Installer<DSGameInstaller> 
    {
       /* private MainCamera _mainCamera;
        private SmoothCameraWrapper _smoothCameraWrapper;*/

        [Inject]
        public DSGameInstaller(MainCamera mainCamera)
        {
/*            _mainCamera = mainCamera;
            _smoothCameraWrapper = smoothCameraWrapper;*/
        }

        public override void InstallBindings()
        {
            //Container.Bind<CameraNuisanceController>().FromNewComponentOn(_mainCamera.gameObject).AsTransient().NonLazy();
            //Container.Bind<CameraNuisanceController>().FromNewComponentOn(_smoothCameraWrapper.Value.gameObject).AsTransient().NonLazy();
            Container.BindInterfacesAndSelfTo<DefaultCameraManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<NuisanceGameController>().AsSingle();
        }
    }
}
