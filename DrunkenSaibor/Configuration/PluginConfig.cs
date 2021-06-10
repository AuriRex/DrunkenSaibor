using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace DrunkenSaibor.Configuration
{
    public class PluginConfig
    {
        public virtual bool Enabled { get; set; } = true;
        public virtual bool LevelIndependentHarassment { get; set; } = false;

        [NonNullable, UseConverter(typeof(ListConverter<SavedNuisance>))]
        public virtual List<SavedNuisance> Nuisances { get; set; } = new List<SavedNuisance>();

        public class SavedNuisance
        {
            public virtual string Name { get; set; } = string.Empty;
            public virtual bool Enabled { get; set; } = false;
        }

        internal bool TryGetNuisanceSettingWithName(string referenceName, out SavedNuisance savedNuisance)
        {
            if (Nuisances.Any(x => x.Name.Equals(referenceName)))
            {
                savedNuisance = Nuisances.First(x => x.Name.Equals(referenceName));
                return true;
            }
            savedNuisance = null;
            return false;
        }
    }
}
