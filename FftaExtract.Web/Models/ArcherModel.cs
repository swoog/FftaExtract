using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FftaExtract.Web.Models
{
    using FftaExtract.DatabaseModel;

    public class ArcherModel
    {
        public Archer Archer { get; set; }

        public List<string> Bows { get; set; }
    }
}