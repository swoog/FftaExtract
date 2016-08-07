namespace FftaExtract.DatabaseModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Archer
    {
        [Key]
        public string Code { get; set; }

        [Index("IX_Lastname")]
        public string LastName { get; set; }

        [Index("IX_Firstname")]
        public string FirstName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public Sexe? Sexe { get; set; }

        public DateTime LastUpdate { get; set; }

        public string CodeArcher { get; set; }
    }
}
