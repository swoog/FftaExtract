namespace FftaExtract.DatabaseModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Competition
    {
        [Key]
        [Column(Order = 1)]
        public int Year { get; set; }

        public CompetitionInfo CompetitionInfo { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CompetitionInfoId { get; set; }

        [Key]
        [Column(Order = 3)]
        public CompetitionType Type { get; set; }
    }
}