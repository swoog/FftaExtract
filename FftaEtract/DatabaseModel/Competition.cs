namespace FftaExtract.DatabaseModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Competition
    {
        [Key]
        public int Id { get; set; }

        public int Year { get; set; }

        public CompetitionInfo CompetitionInfo { get; set; }

        public int CompetitionInfoId { get; set; }

        public CompetitionType Type { get; set; }
    }
}