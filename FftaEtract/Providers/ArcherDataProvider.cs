namespace FftaExtract.Providers
{
    using System;
    using System.Collections.Generic;

    public class ArcherDataProvider
    {
        public ArcherDataProvider()
        {
            this.Competitions = new List<CompetitionDataProvider>();
        }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public int Num
        {
            get
            {
                return Convert.ToInt32(this.Code);
            }
        }

        public string Code { get; set; }

        public void AddCompetition(CompetitionDataProvider competitionDataProvider)
        {
            this.Competitions.Add(competitionDataProvider);
        }

        public IList<CompetitionDataProvider> Competitions { get; private set; }

        public string Club { get; set; }
    }
}