namespace DrunkenSaibor.Data.Nuisances
{
    class ColorOverflowCamera : Nuisance
    {
        public override string Name { get; protected set; } = "color_overflow";
        public override bool DisablesScoreSubmission { get; protected set; } = false;
    }
}
