namespace Api.Options
{
    public class PaycheckSettings
    {
        public decimal BaseEmployeeCost { get; set; }

        public decimal DependentCost { get; set; }

        public decimal SeniorDependentCost { get; set; }

        public int SeniorDependentAgeThreshold { get; set; }

        public decimal HighEarnerThreshold { get; set; }

        public decimal HighEarnerPercentage { get; set; } // Stored as a percentage (e.g., 20 for 20%)

        public int PaychecksPerYear { get; set; }
    }
}
