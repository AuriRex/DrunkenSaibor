using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DrunkenSaibor.Managers
{
    public class NIntensityMappings : IInitializable
    {
        private Dictionary<string, ActionRef<float, float, Material>> _mappings;

        delegate void ActionRef<T, T2, T3>(T item, T2 item2, ref T3 item3);

        public void Initialize()
        {
            _mappings = new Dictionary<string, ActionRef<float, float, Material>>();

            _mappings.Add("pixelate", (float intensity, float time, ref Material mat) => {
                float fintensity = 1 - Easing.InOutQuad(intensity);

                int pixels = (int) (2000f * fintensity);

                if (pixels < 4) pixels = 4;
                if (fintensity < .2f) pixels = 30000;

                mat.SetFloat("_length", pixels);
                mat.SetFloat("_height", pixels);
            });

            _mappings.Add("fms_cat_weirdshit", (float intensity, float time, ref Material mat) => {
                mat.SetFloat("_speed", 4 * intensity); // 10
                mat.SetFloat("_strength", 10 * intensity); // 10
            });

            _mappings.Add("zoom", (float intensity, float time, ref Material mat) => {
                intensity = intensity / 4;
                mat.SetFloat("_strength", - intensity); // < -.5 = screen flip! | -.33 > is unplayable
            });

            _mappings.Add("rotation", (float intensity, float time, ref Material mat) => {
                float val = Mathf.Sin(time);
                bool negative = val < 0;
                val = Mathf.Abs(val);
                val = Easing.InCirc(val) * (negative ? -1 : 1);
                mat.SetFloat("_RotationValue", intensity * val * 0.2f);
            });
        }

        public void SetEffectProperties(string referenceName, float intensity, ref Material mat, float time = 0)
        {
            if(!_mappings.ContainsKey(referenceName))
            {
                throw new ArgumentException($"Invalid mapping: {referenceName}");
            }
            if (mat == null) return;
            if (time == 0) time = Time.fixedTime;
            //Mathf.Clamp(intensity, 0, 1);
            _mappings.TryGetValue(referenceName, out ActionRef<float, float, Material> action);
            action?.Invoke(intensity, time, ref mat);
        }

        
    }
}
