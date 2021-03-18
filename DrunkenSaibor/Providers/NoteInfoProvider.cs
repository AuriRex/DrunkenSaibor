
using DrunkenSaibor.Configuration;
using DrunkenSaibor.Managers;
using SiraUtil.Interfaces;
using System;
using Zenject;

namespace DrunkenSaibor.Providers
{
    internal class NoteInfoProvider : IModelProvider
    {
        public Type Type => typeof(NoteInfoDecorator);
        public int Priority { get; set; } = 301;

        private class NoteInfoDecorator : IPrefabProvider<GameNoteController>
        {
            public bool Chain => true;
            public bool CanSetup { get; private set; }

            [Inject]
            public void Construct(DiContainer Container, GameplayCoreSceneSetupData sceneSetupData, PluginConfig pluginConfig) => CanSetup = pluginConfig.Enabled;

            public GameNoteController Modify(GameNoteController original)
            {
                if (!CanSetup) return original;
                original.gameObject.AddComponent<NoteInfoGrabber>();
                return original;
            }
        }
    }
}

