using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FftaExtract.DatabaseModel
{
    public class CompetitionScore
    {
        public int Id { get; set; }

        public Competition Competition { get; set; }

        public int CompetitionId { get; set; }

        public Archer Archer { get; set; }

        public string ArcherCode { get; set; }

        public int Score { get; set; }

        public int Rank { get; set; }

        public BowType BowType { get; set; }
    }
}
