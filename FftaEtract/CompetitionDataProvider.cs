namespace FftaEtract
{
    public class CompetitionDataProvider
    {
        public CompetitionDataProvider(int year, string name, CompetitionType competitionType, BowType bowType, int score)
        {
            this.BowType = bowType;
            this.Score = score;
            this.Year = year;
            this.CompetitionType = competitionType;
            this.Name = name;
        }

        public int Year { get; private set; }

        public CompetitionType CompetitionType { get; private set; }

        public string Name { get; private set; }

        public BowType BowType { get; private set; }

        public int Score { get; private set; }
    }
}