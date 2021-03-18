namespace DrunkenSaibor.Data.Nuisances
{
    class PixelateCamera : Nuisance
    {
        public override string Name { get; protected set; } = "pixelate";
        public override bool DisablesScoreSubmission { get; protected set; } = false;
    }
}
