namespace FftaExtract.DatabaseModel
{
    using System.ComponentModel.DataAnnotations;

    public class Archer
    {
        [Key]
        public string Code { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }
    }
}
