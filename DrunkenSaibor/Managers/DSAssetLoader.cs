using DrunkenSaibor.Configuration;
using DrunkenSaibor.Data;
using DrunkenSaibor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace DrunkenSaibor.Managers
{
    public class DSAssetLoader : IInitializable, IDisposable
    {
        private readonly PluginConfig _pluginConfig;
        private Dictionary<string, DrunkEffectData> _effects;

        public DSAssetLoader(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
        }

        public void Initialize() => Load();

        private const string PATH = "DrunkenSaibor.Resources.Effects.";
        private const string EXTENSION = ".bsfx";
        public const string NUISANCE_NAMESPACE = "DrunkenSaibor.Data.Nuisances";

        public List<string> disableByDefault = new List<string>()
        {
            "fms_cat_weirdshit"
        };

        private void Load()
        {
            _effects = new Dictionary<string, DrunkEffectData>();

            string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            foreach (string s in resources)
            {
                if (s.StartsWith(PATH))
                {
                    string name = s.Substring(PATH.Length, s.Length - PATH.Length - EXTENSION.Length);
                    Logger.log.Info($"Loading > {name}");
                    var def = Utils.LoadSFX(s);
                    if(disableByDefault.Contains(def.ReferenceName))
                    {
                        def.EnabledByDefault = false;
                    }
                    _effects.Add(name, def);
                }
            }

            try
            {
                var go = new GameObject($"{nameof(DSAssetLoader)}TempGameObject");
                foreach (Type t in Utils.GetTypesInNamespace(NUISANCE_NAMESPACE))
                {
                    Nuisance instance = go.AddComponent(t) as Nuisance;

                    if (_effects.TryGetValue(instance.Name, out DrunkEffectData data)) {
                        data.RuntimeType = t;
                    }
                    else
                    {
                        Logger.log.Warn($"No effect matching Type \"{t.Name}\"!");
                    }

                    UnityEngine.Object.Destroy(instance);
                }
                UnityEngine.Object.Destroy(go);
            }
            catch(Exception ex)
            {
                Logger.log.Error($"Loader Type matcher encountered an error: {ex.Message}");
                Logger.log.Debug(ex.StackTrace);
            }
            
            foreach(DrunkEffectData data in _effects.Values)
            {
                if (!_pluginConfig.Nuisances.Any(x => x.Name.Equals(data.ReferenceName)))
                {
                    _pluginConfig.Nuisances.Add(new PluginConfig.SavedNuisance()
                    {
                        Enabled = data.EnabledByDefault,
                        Name = data.ReferenceName
                    });
                }
            }

        }

        public DrunkEffectData Get(string v)
        {
            if (v == null) return null;
            _effects.TryGetValue(v, out DrunkEffectData data);
            return data;
        }

        public void Dispose()
        {

        }

        public DrunkEffectData[] GetAll()
        {
            DrunkEffectData[] data = new DrunkEffectData[_effects.Values.Count];
            _effects.Values.CopyTo(data, 0);
            return data;
        }

        internal bool TryGetEffectWithType(Type t, out DrunkEffectData data)
        {
            if (_effects.Values.Any(x => x.RuntimeType.Equals(t)))
            {
                data = _effects.Values.First(x => x.RuntimeType.Equals(t));
                return true;
            }
            data = null;
            return false;
        }
    }
}
