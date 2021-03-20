using DrunkenSaibor.Managers;
using UnityEngine;
using Zenject;

namespace DrunkenSaibor.Data
{
    public abstract class Nuisance : MonoBehaviour
    {
        private DrunkEffectData _data;
        private Material _material;
        public DrunkEffectData Data
        {
            get => _data;
            internal set
            {
                _data = value;
                _material = value?.Material;
            }
        }

        public bool IsActive { get; private set; } = false;

        protected NuisanceManager _nuisanceManager;
        protected DSAssetLoader _dsAssetLoader;
        protected NIntensityMappings _mappings;

        public abstract string Name { get; protected set; }
        public abstract bool DisablesScoreSubmission { get; protected set; }

        [Inject]
        public void Init(NuisanceManager nuisanceManager, DSAssetLoader dsAssetLoader, NIntensityMappings mappings)
        {
            _nuisanceManager = nuisanceManager;
            _dsAssetLoader = dsAssetLoader;
            _mappings = mappings;

            NuisanceInit();
        }

        protected virtual void NuisanceInit() => Data = _dsAssetLoader.Get(Name);

        public virtual void Update() => _mappings?.SetEffectProperties(Data?.ReferenceName, _nuisanceManager.Intensity, ref _material);

        public void OnEnable()
        {
            IsActive = true;
            NuisanceEnabled();
        }

        public virtual void NuisanceEnabled()
        {

        }

        public void OnDisable()
        {
            IsActive = false;
            NuisanceDisabled();
        }

        public virtual void NuisanceDisabled()
        {

        }

        public void OnDestroy()
        {

        }

        public bool ShouldRender() => Data?.ShouldRender != null && Data.ShouldRender;

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (Data == null || !Data.ShouldRender)
            {
                Graphics.Blit(source, destination);
                return;
            }

            Graphics.Blit(source, destination, Data.Material);
        }

    }
}
