using DrunkenSaibor.Managers;
using DrunkenSaibor.UI;
using Zenject;

namespace DrunkenSaibor.Installers
{
    class DSMenuInstaller : Installer<DSMenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuUIManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ModifierHost>().AsSingle();
        }
    }
}
