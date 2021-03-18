using IPA.Config.Stores;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace DrunkenSaibor.Configuration
{
    public class PluginConfig
    {
        public virtual bool Enabled { get; set; } = true;
        public virtual bool LevelIndependentHarassment { get; set; } = false;
    }
}
