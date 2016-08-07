namespace FftaExtract.DatabaseModel
{
    using System.ComponentModel.DataAnnotations;

    public class Club
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }
    }
}