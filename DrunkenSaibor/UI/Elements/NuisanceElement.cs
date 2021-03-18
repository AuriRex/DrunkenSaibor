using BeatSaberMarkupLanguage.Attributes;
using DrunkenSaibor.Data;

namespace DrunkenSaibor.UI.Elements
{
    public class NuisanceElement : CustomListElement
    {

        private string _referenceName;
        private string _color = "lime";
        private DrunkEffectData _data;

        public NuisanceElement(DrunkEffectData data)
        {
            _data = data;
            _referenceName = _data.Name;
        }

        public bool Enabled
        {
            get => _color.Equals("lime");
            set
            {
                BGColor = value ? "lime" : "red";
                _data.ShouldRender = value;
            }
        }

        [UIValue("reference-name")]
        public string ReferenceName
        {
            get => _referenceName;
            set
            {
                _referenceName = value;
                NotifyPropertyChanged(nameof(ReferenceName));
            }
        }

        [UIValue("color")]
        protected string BGColor
        {
            get => _color;
            set
            {
                _color = value;
                NotifyPropertyChanged(nameof(BGColor));
            }
        }

    }
}
