using IPA.Loader;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace DrunkenSaibor.Managers
{
    internal class CameraManager : IInitializable, IDisposable
    {
        public Camera[] Cameras { get; private set; } = null;

        public virtual void Initialize()
        {
            Logger.log.Debug("Initializing DrunkenSaibor CameraManager!");
            SharedCoroutineStarter.instance.StartCoroutine(Util.Utils.DoAfter(0.1f, () => {
                Refresh();
            }));
        }
        public virtual void Dispose()
        {
            Logger.log.Debug("Disposing DrunkenSaibor CameraManager!");
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
                    cam.gameObject.AddComponent<CameraNuisanceController>();
                }
                else
                {
                    GameObject.Destroy(cnc);
                    cam.gameObject.AddComponent<CameraNuisanceController>();
                }
            }
        }

        public virtual void Refresh()
        {
            PluginMetadata cameraTwo = PluginManager.GetPluginFromId("Camera2");
            if (cameraTwo != null)
            {
                List<Camera> cameras = new List<Camera>();
                Type cam2Type = cameraTwo?.Assembly.GetType("Camera2.Behaviours.Cam2");
                MonoBehaviour[] allCam2s = GameObject.FindObjectsOfType(cam2Type) as MonoBehaviour[];
                foreach (MonoBehaviour cam2 in allCam2s)
                {
                    var camera = cam2Type.GetProperty("UCamera", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cam2, null) as Camera;
                    if (camera != null)
                    {
                        cameras.Add(camera);
                    }
                }
                cameras.AddRange(Camera.allCameras);
                Cameras = cameras.ToArray();
            }
            else
            {
                Cameras = Camera.allCameras;
            }

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
