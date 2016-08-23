namespace FftaExtract.DatabaseModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class CompetitionInfo
    {
        public int Id { get; set; }

        [MaxLength(200)]
        [Index("IX_CompetitionName", 1, IsUnique = true)]
        public string Name { get; set; }

        public DbGeography Location { get; set; }
    }
}