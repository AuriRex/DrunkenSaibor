using DrunkenSaibor.Data;
using DrunkenSaibor.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DrunkenSaibor.Managers
{
    public class CameraNuisanceController : MonoBehaviour
    {
        private NuisanceManager _nuisanceManager;
        private DiContainer _container;

        private List<Nuisance> _attachedNuisanceList;

        private bool _isInitialized = false;

        [Inject]
        public void Construct(DiContainer container, NuisanceManager nuisanceManager)
        {
            _container = container;
            _nuisanceManager = nuisanceManager;

            _attachedNuisanceList = new List<Nuisance>();
            _nuisanceManager.AddCameraNuisanceController(this);
            _isInitialized = true;
        }

        public void OnEnable()
        {
            if (!_isInitialized)
            {
                Logger.log.Debug("CameraNuisanceController First Enabled!");
                StartCoroutine(Utils.DoAfter(.2f, () => {
                    Logger.log.Debug("DoAfter()");
                    if (!_isInitialized)
                    {
                        NuisanceManager.NonZenjectedCameraNuisanceControllerFirstEnabled(this);
                    }
                }));
            }  
        }

        public bool AnyDisablesScore()
        {
            foreach(Nuisance n in _attachedNuisanceList)
            {
                if (n.ShouldRender() && n.DisablesScoreSubmission) return true;
            }
            return false;
        }

        public void OnDestroy()
        {
            DestroyAllNuisances();
            _nuisanceManager.RemoveCameraNuisanceController(this);
        }

        public void EnableAllNuisances() => SetAllNuisancesEnabled(true);

        public void DisableAllNuisances() => SetAllNuisancesEnabled(false);

        public void SetAllNuisancesEnabled(bool v)
        {
            foreach (Nuisance n in _attachedNuisanceList)
            {
                if(n)
                    n.enabled = v;
            }
        }

        public void AddNuisance(Type T)
        {
            if (this.gameObject.GetComponent(T) != null) return;
            Nuisance n = this.gameObject.AddComponent(T) as Nuisance;
            _container.Inject(n);
            _attachedNuisanceList.Add(n);
        }

        public void DestroyAllNuisances()
        {
            foreach(Nuisance n in _attachedNuisanceList)
            {
                GameObject.Destroy(n);
            }
            _attachedNuisanceList = new List<Nuisance>();
        }
    }
}
