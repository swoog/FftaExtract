namespace FftaEtract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ArcherDataProvider
    {
        public ArcherDataProvider()
        {
            this.Competitions = new List<CompetitionDataProvider>();
        }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public int Num {
            get
            {
                return Convert.ToInt32(this.Code.Substring(0, this.Code.Length - 1));
            }
        }

        public string Code { get; set; }

        public void AddCompetition(CompetitionDataProvider competitionDataProvider)
        {
            this.Competitions.Add(competitionDataProvider);
        }

        public IList<CompetitionDataProvider> Competitions { get; private set; } 
    }
}