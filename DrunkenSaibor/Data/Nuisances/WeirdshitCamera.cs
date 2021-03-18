namespace DrunkenSaibor.Data.Nuisances
{
    class WeirdshitCamera : Nuisance
    {
        public override string Name { get; protected set; } = "fms_cat_weirdshit";
        public override bool DisablesScoreSubmission { get; protected set; } = false;
    }
}
