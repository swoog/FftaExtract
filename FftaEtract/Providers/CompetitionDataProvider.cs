namespace FftaExtract.Providers
{
    using System;

    using FftaExtract.DatabaseModel;

    public class CompetitionDataProvider
    {
        public CompetitionDataProvider(int year, DateTime begin, DateTime end, string name, CompetitionType competitionType, BowType bowType, int score)
        {
            this.BowType = bowType;
            this.Score = score;
            this.Year = year;
            this.Begin = begin;
            this.End = end;
            this.CompetitionType = competitionType;
            this.Name = name;
        }

        public int Year { get; private set; }

        public DateTime Begin { get; private set; }

        public DateTime End { get; private set; }

        public CompetitionType CompetitionType { get; private set; }

        public string Name { get; private set; }

        public BowType BowType { get; private set; }

        public int Score { get; private set; }
    }
}