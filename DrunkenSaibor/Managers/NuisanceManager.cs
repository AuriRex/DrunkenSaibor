using DrunkenSaibor.Configuration;
using DrunkenSaibor.Util;
using SiraUtil.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DrunkenSaibor.Managers
{
    public class NuisanceManager : IInitializable, IDisposable
    {
        private static DiContainer _container;
        private readonly Submission _submission;
        private readonly PluginConfig _pluginConfig;

        private static List<CameraNuisanceController> _cameraNuisanceControllers = new List<CameraNuisanceController>();

        /// <summary>
        /// This does also count fake notes!
        /// </summary>
        public int NotesSpawned { get; private set; } = 0;
        public int NotesCut { get; private set; } = 0;
        public int GoodCuts { get; private set; } = 0;
        public int BadCuts { get; private set; } = 0;
        public int Combo { get; private set; } = 0;

        public int NotesMissed { get; private set; } = 0;
        public int ConsecutiveMissed { get; private set; } = 0;

        public float Intensity { get; internal set; } = 0.3f;
        public float TargetIntensity { get; internal set; } = 0.3f;

        public Type[] NuisanceTypes { get; private set; }

        public bool InGame { get; private set; } = false;

        public NuisanceManager(DiContainer container, [InjectOptional] Submission submission, PluginConfig pluginConfig)
        {
            _container = container;
            _submission = submission;
            _pluginConfig = pluginConfig;
        }

        internal void OnGameStart()
        {
            Logger.log.Debug("OnGameStart");
            if(!_pluginConfig.LevelIndependentHarassment)
            {
                NotesCut = 0;
                GoodCuts = 0;
                BadCuts = 0;
                NotesMissed = 0;
                NotesSpawned = 0;
                Combo = 0;
                ConsecutiveMissed = 0;
                Intensity = 0;
                TargetIntensity = 0;
            }

            InGame = true;

            AddNuisances();
            bool scores = true;
            foreach (var cnc in _cameraNuisanceControllers)
            {
                if (cnc == null) continue;
                if (cnc.AnyDisablesScore()) scores = false;
            }
            if(!scores) _submission?.DisableScoreSubmission("Drunken Saibor", "Effect");

        }

        private void AddNuisances()
        {
            foreach (var cnc in _cameraNuisanceControllers)
            {
                if (cnc == null) continue;
                foreach(Type t in NuisanceTypes)
                {
                    cnc.AddNuisance(t);
                }
                cnc.SetAllNuisancesEnabled(_pluginConfig.Enabled);
            }
        }

        internal void OnGameStop()
        {
            Logger.log.Debug("OnGameStop");
            foreach (var cnc in _cameraNuisanceControllers)
            {
                if(cnc)
                    cnc.DisableAllNuisances();
            }

            InGame = false;
        }

        private float _pauseBackupIntensity = 0;
        internal void OnPause()
        {
            _pauseBackupIntensity = Intensity;
            if (_intensityCoroutine != null) SharedCoroutineStarter.instance.StopCoroutine(_intensityCoroutine);
            Intensity = 0f;
        }
        internal void OnResume()
        {
            Intensity = _pauseBackupIntensity;
            UpdateIntensity();
        }

        internal void AddCameraNuisanceController(CameraNuisanceController cameraNuisanceController)
        {
            if (!_cameraNuisanceControllers.Contains(cameraNuisanceController))
            {
                Logger.log.Debug($"New SmoothCameraNuisanceController on gameobject '{cameraNuisanceController.gameObject.name}'");
                _cameraNuisanceControllers.Add(cameraNuisanceController);
            }
        }

        internal void RemoveCameraNuisanceController(CameraNuisanceController cameraNuisanceController)
        {
            if (_cameraNuisanceControllers.Contains(cameraNuisanceController))
                _cameraNuisanceControllers.Remove(cameraNuisanceController);
        }

        private int _lastFrameCount = 0;
        private void UpdateIntensity()
        {
            int currentFrameCount = Time.frameCount;
            if (_lastFrameCount == currentFrameCount) return;

            if (NotesSpawned == 0) return;

            float cutP = (float) GoodCuts / (float) NotesSpawned;
            float cutBadP = (float) BadCuts / (float) NotesSpawned;
            float missedP = (float) NotesMissed / (float) NotesSpawned;

            float comboStat = Mathf.Clamp(((float) Combo) / 100f, 0f, 1f);

            float badP = (cutBadP + missedP) / 2f;

            float badStat = Easing.OutQuint(1 - badP);
            float goodStat = (Easing.InCirc(cutP) + Easing.OutQuad(comboStat)) / 2f;

            float firstFifty = Mathf.Clamp(((float) NotesSpawned) / 50f, 0f, 1f);

            TargetIntensity = Mathf.Clamp((goodStat * badStat) * firstFifty, 0f, 1f);
            if (_intensityCoroutine != null) SharedCoroutineStarter.instance.StopCoroutine(_intensityCoroutine);
            _intensityCoroutine = SharedCoroutineStarter.instance.StartCoroutine(IntensityLerp(this, .5f));

            //Logger.log.Debug($"I: {Intensity} - TI: {TargetIntensity}");

            _lastFrameCount = currentFrameCount;
        }

        private Coroutine _intensityCoroutine;


        private IEnumerator IntensityLerp(NuisanceManager nuisanceManager, float lerpDuration = 0.3f, Action onDone = null)
        {
            float timeElapsed = 0f;
            float startValue = nuisanceManager.Intensity;
            float endValue = nuisanceManager.TargetIntensity;
            while (timeElapsed < lerpDuration)
            {
                nuisanceManager.Intensity = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            nuisanceManager.Intensity = endValue;
            onDone?.Invoke();
        }

        internal void OnNoteSpawned(NoteController noteController)
        {
            if (!InGame) return;

            NotesSpawned++;
            //Logger.log.Info($"Notes spawned: {NotesSpawned}");
            UpdateIntensity();
        }

        internal void OnNoteCut(NoteController noteController, in NoteCutInfo noteCutInfo)
        {
            if (!InGame) return;

            NotesCut++;
            Combo++;
            ConsecutiveMissed = 0;
            if(noteCutInfo.allIsOK)
            {
                GoodCuts++;
            }
            else
            {
                BadCuts++;
            }

            UpdateIntensity();
        }

        internal void OnNoteMissed(NoteController noteController)
        {
            if (!InGame) return;

            NotesMissed++;
            Combo = 0;
            ConsecutiveMissed++;

            UpdateIntensity();
        }

        public static void NonZenjectedCameraNuisanceControllerFirstEnabled(CameraNuisanceController cameraNuisanceController)
        {
            if(!_cameraNuisanceControllers.Contains(cameraNuisanceController))
            {
                _container.Inject(cameraNuisanceController);
            }
        }

        public void Initialize()
        {
            NuisanceTypes = Utils.GetTypesInNamespace("DrunkenSaibor.Data.Nuisances");
            foreach(Type t in NuisanceTypes)
            {
                Logger.log.Debug($"NuisanceType: {t}");
            }
            if (_cameraNuisanceControllers == null)
                _cameraNuisanceControllers = new List<CameraNuisanceController>();
            Logger.log.Debug($"{typeof(NuisanceManager)} Initialized!");
        }


        public void Dispose()
        {
            if (_intensityCoroutine != null) SharedCoroutineStarter.instance.StopCoroutine(_intensityCoroutine);
            foreach (var cnc in _cameraNuisanceControllers)
            {
                if (cnc == null) continue;
                GameObject.Destroy(cnc);
            }
        }
    }
}
