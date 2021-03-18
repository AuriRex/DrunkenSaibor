using BeatSaberMarkupLanguage.GameplaySetup;
using DrunkenSaibor.UI;
using System;
using Zenject;

namespace DrunkenSaibor.Managers
{
    public class MenuUIManager : IInitializable, IDisposable
    {
        private ModifierHost _modifierHost;

        [Inject]
        public MenuUIManager(ModifierHost modifierHost)
        {
            _modifierHost = modifierHost;
        }

        public void Initialize() => GameplaySetup.instance?.AddTab("Drunken Saibor", "DrunkenSaibor.UI.Views.modifiers.bsml", _modifierHost);

        public void Dispose()
        {
            if (GameplaySetup.IsSingletonAvailable)
            {
                GameplaySetup.instance.RemoveTab("Drunken Saibor");
            }
        }
    }
}
