namespace FftaExtract.DatabaseModel
{
    public class CompetitionStats
    {
        public Competition Competition { get; set; }

        public int CountArchers { get; set; }

        public CompetitionInfo CompetitionInfo { get; set; }
    }
}