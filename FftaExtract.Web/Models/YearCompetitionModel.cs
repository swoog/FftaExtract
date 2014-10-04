namespace FftaExtract.Web.Models
{
    using System.Collections;
    using System.Collections.Generic;

    using FftaExtract.DatabaseModel;

    public class YearCompetitionModel
    {
        public IList<TyepCompetitionModel> Types { get; set; }

        public int Year { get; set; }
    }

    public class TyepCompetitionModel
    {
        public IList<CompetitionScore> Competitions { get; set; }

        public CompetitionType Type { get; set; }

        public CompetitionScore[] HighScores { get; set; }

        public int Average { get; set; }
    }
}