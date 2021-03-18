namespace DrunkenSaibor.Data.Nuisances
{
    class RotateCamera : Nuisance
    {
        public override string Name { get; protected set; } = "rotation";
        public override bool DisablesScoreSubmission { get; protected set; } = false;

        protected override void NuisanceInit()
        {
            Logger.log.Debug($"{Name} nuisance initialized!");
            base.NuisanceInit();
        }
    }
}
