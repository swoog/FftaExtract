namespace FftaEtract
{
    using System;
    using System.Collections.Generic;

    public class Archer
    {
        public Archer()
        {
            this.Competitions = new List<Competition>();
        }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public int Num {
            get
            {
                return Convert.ToInt32(this.Code.Substring(0, this.Code.Length - 1));
            }
        }

        public string Code { get; set; }

        public void AddCompetition(Competition competition)
        {
            this.Competitions.Add(competition);
        }

        public IList<Competition> Competitions { get; private set; } 
    }
}