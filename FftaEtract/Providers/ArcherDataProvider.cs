namespace FftaExtract.Providers
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using FftaExtract.DatabaseModel;

    public class ArcherDataProvider
    {
        public ArcherDataProvider(string code, string archerCode)
        {
            var match = Regex.Match(code, "^([0-9]+)[a-zA-Z]$");
            if (match.Success)
            {
                this.Code = match.Groups[1].Value;
                this.ArcherCode = code;
            }
            else
            {
                this.Code = code;
                if (code == archerCode)
                {
                    this.ArcherCode = null;
                }
                else
                {
                    this.ArcherCode = archerCode;
                }
            }

            this.Competitions = new List<CompetitionDataProvider>();
        }

        public ArcherDataProvider(string code)
            : this(code, code)
        {

        }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Code { get; private set; }

        public void AddCompetition(CompetitionDataProvider competitionDataProvider)
        {
            this.Competitions.Add(competitionDataProvider);
        }

        public IList<CompetitionDataProvider> Competitions { get; private set; }

        public IList<ClubDataProvider> Club { get; set; }

        public Sexe? Sexe { get; set; }

        public string ArcherCode { get; set; }
    }
}