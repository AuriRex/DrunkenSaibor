using System;
using UnityEngine;
using Zenject;

namespace DrunkenSaibor.Managers
{
    internal class DefaultCameraManager : IInitializable, IDisposable
    {

        public Camera[] Cameras { get; private set; } = null;

        public virtual void Initialize()
        {
            Logger.log.Debug("Initializing new CameraManager!");
            Refresh();
        }
        public virtual void Dispose()
        {
            Logger.log.Debug("Disposing CameraManager!");
            Clean();
        }

        private void OnCameraRefreshDone()
        {
            foreach (Camera cam in Cameras)
            {
                Logger.log.Debug(cam.name);
                if (cam.name.EndsWith(".cfg")) continue;
                CameraNuisanceController cnc = cam.gameObject.GetComponent<CameraNuisanceController>();
                if (cnc == null)
                {
                    cnc = cam.gameObject.AddComponent<CameraNuisanceController>();
                }
            }
        }

        public virtual void Refresh()
        {
            Cameras = Camera.allCameras;
            OnCameraRefreshDone();
        }

        internal virtual void Clean()
        {
            if (Cameras != null)
            {
                foreach (Camera cam in Cameras)
                {
                    if (cam == null) continue;
                    CameraNuisanceController cnc = cam.gameObject.GetComponent<CameraNuisanceController>();
                    if (cnc != null)
                    {
                        UnityEngine.Object.Destroy(cnc);
                    }
                }
            }
        }
    }
}
