namespace FftaExtract.DatabaseModel
{
    public class BestScore
    {
        public CompetitionType CompetitionType { get; set; }

        public int Score { get; set; }

        public BowType BowType { get; set; }

        public string CompetitionName { get; set; }
    }
}