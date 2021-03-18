using DrunkenSaibor.Data;
using DrunkenSaibor.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using Zenject;

namespace DrunkenSaibor.Managers
{
    public class DSAssetLoader : IInitializable, IDisposable
    {
        private Dictionary<string, DrunkEffectData> _effects;

        public void Initialize() => Load();

        private const string PATH = "DrunkenSaibor.Resources.Effects.";
        private const string EXTENSION = ".dsfx";

        private void Load()
        {
            _effects = new Dictionary<string, DrunkEffectData>();

            string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            foreach (string s in resources)
            {
                if(s.StartsWith(PATH))
                {
                    string name = s.Substring(PATH.Length, s.Length - PATH.Length - EXTENSION.Length);
                    Logger.log.Info($"Loading > {name}");
                    _effects.Add(name, Utils.LoadSFX(s));
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
    }
}
