namespace DrunkenSaibor.Data.Nuisances
{
    class ZoomCamera : Nuisance
    {
        public override string Name { get; protected set; } = "zoom";
        public override bool DisablesScoreSubmission { get; protected set; } = false;

        protected override void NuisanceInit()
        {
            Logger.log.Debug("Zoom Nuisance initialized!");
            base.NuisanceInit();
        }
    }
}
