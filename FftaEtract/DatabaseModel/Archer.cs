namespace FftaEtract.DatabaseModel
{
    using System.ComponentModel.DataAnnotations;

    public class Archer
    {
        [Key]
        public string Code { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}
