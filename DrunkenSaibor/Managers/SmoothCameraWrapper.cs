namespace DrunkenSaibor.Managers
{
    public class SmoothCameraWrapper
    {

        private static SmoothCamera _smoothCamera;

        public SmoothCamera Value
        {
            get => _smoothCamera;
        }

        public bool IsAvailable
        {
            get => _smoothCamera != null;
        }

        internal static void SetSCC(SmoothCameraController smoothCameraController, SmoothCamera smoothCamera)
        {
            
            _smoothCamera = smoothCamera;
            Logger.log.Debug($"SmoothCamera set! -> {smoothCamera != null}");
        }

    }
}
