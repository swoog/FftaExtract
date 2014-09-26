using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FftaExtract.Web.Models
{
    using System.Collections;
    using System.Web.Mvc;

    using FftaExtract.DatabaseModel;

    public class ArcherModel
    {
        public Archer Archer { get; set; }

        public List<BowType> Bows { get; set; }

        public List<BestScore> BestScores { get; set; }

        public List<YearCompetitionModel> Competitions { get; set; }
    }
}