namespace FftaExtract.Web.Models
{
    using System.Collections.Generic;

    using FftaExtract.DatabaseModel;

    public class HomeModel
    {
        public GlobalStats Stats { get; set; }

        public IList<CompetitionStats> LastCompetitions { get; set; }
    }
}