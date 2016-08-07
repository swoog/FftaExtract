namespace FftaExtract.DatabaseModel
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Club
    {
        public int Id { get; set; }

        [MaxLength(200)]
        [Index("IX_ClubName", 1, IsUnique = true)]
        public string Name { get; set; }
    }
}