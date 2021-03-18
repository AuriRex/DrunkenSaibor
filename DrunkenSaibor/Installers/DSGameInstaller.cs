using DrunkenSaibor.Managers;
using Zenject;

namespace DrunkenSaibor.Installers
{
    class DSGameInstaller : Installer<DSGameInstaller> 
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<NuisanceGameController>().AsSingle();
        }
    }
}
