using DrunkenSaibor.Util;
using System;
using Zenject;

namespace DrunkenSaibor.Managers
{
    public class NuisanceGameController : IInitializable, IDisposable
    {
        private NuisanceManager _nuisanceManager;
        private IGamePause _gamePause;

        [Inject]
        public NuisanceGameController(NuisanceManager nuisanceManager, [InjectOptional] IGamePause gamePause)
        {
            _nuisanceManager = nuisanceManager;
            _gamePause = gamePause;
        }

        public void Initialize()
        {
            if(_gamePause != null)
            {
                _gamePause.didPauseEvent += _nuisanceManager.OnPause;
                _gamePause.didResumeEvent += _nuisanceManager.OnResume;
            }

            SharedCoroutineStarter.instance.StartCoroutine(Utils.DoAfter(.5f, () => {
                _nuisanceManager.OnGameStart();
            }));
        }

        public void Dispose()
        {
            if(_gamePause != null)
            {
                _gamePause.didPauseEvent -= _nuisanceManager.OnPause;
                _gamePause.didResumeEvent -= _nuisanceManager.OnResume;
            }
            _nuisanceManager.OnGameStop();
        }

    }
}
