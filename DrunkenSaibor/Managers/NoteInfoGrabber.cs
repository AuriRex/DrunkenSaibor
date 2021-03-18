using UnityEngine;
using Zenject;

namespace DrunkenSaibor.Managers
{
    class NoteInfoGrabber : MonoBehaviour, INoteControllerNoteWasCutEvent, INoteControllerNoteWasMissedEvent, INoteControllerDidInitEvent
    {
        private NuisanceManager _nuisanceManager;
        private GameNoteController _gameNoteController;

        [Inject]
        public void Init(NuisanceManager nuisanceManager)
        {
            _nuisanceManager = nuisanceManager;
            _gameNoteController = GetComponent<GameNoteController>();

            _gameNoteController.didInitEvent.Add(this);
            _gameNoteController.noteWasCutEvent.Add(this);
            _gameNoteController.noteWasMissedEvent.Add(this);
        }

        protected void OnDestroy()
        {
            if (_gameNoteController != null)
            {
                _gameNoteController.didInitEvent.Remove(this);
                _gameNoteController.noteWasMissedEvent.Remove(this);
                _gameNoteController.noteWasCutEvent.Remove(this);
            }
        }

        public void HandleNoteControllerDidInit(NoteController noteController)
        {
            _nuisanceManager.OnNoteSpawned(noteController);
        }
        public void HandleNoteControllerNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
        {
            _nuisanceManager.OnNoteCut(noteController, noteCutInfo);
        }
        public void HandleNoteControllerNoteWasMissed(NoteController noteController)
        {
            _nuisanceManager.OnNoteMissed(noteController);
        }

    }
}
