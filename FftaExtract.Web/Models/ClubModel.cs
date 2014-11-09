namespace FftaExtract.Web.Models
{
    using System.Collections.Generic;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Web.Controllers;

    public class ClubModel 
    {
        public Club Club { get; set; }

        public List<YearModel> Years { get; set; }
    }
}