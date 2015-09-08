namespace FftaExtract.Providers
{
    using System.Collections.Generic;

    using FftaExtract.DatabaseModel;

    public class ArcherDataProvider
    {
        public ArcherDataProvider()
        {
            this.Competitions = new List<CompetitionDataProvider>();
        }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Code { get; set; }

        public void AddCompetition(CompetitionDataProvider competitionDataProvider)
        {
            this.Competitions.Add(competitionDataProvider);
        }

        public IList<CompetitionDataProvider> Competitions { get; private set; }

        public IList<ClubDataProvider> Club { get; set; }

        public Sexe? Sexe { get; set; }
    }
}