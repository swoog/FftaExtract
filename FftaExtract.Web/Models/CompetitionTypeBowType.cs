namespace FftaExtract.Web.Models
{
    using System;

    using FftaExtract.DatabaseModel;

    public class CompetitionTypeBowType : IComparable<CompetitionTypeBowType>, IComparable
    {
        public CompetitionType Type { get; }

        public BowType BowType { get; }

        public CompetitionTypeBowType(CompetitionType type, BowType bowType)
        {
            this.Type = type;
            this.BowType = bowType;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            var o = obj as CompetitionTypeBowType;

            return this.BowType == o.BowType && this.Type == o.Type;
        }

        public int CompareTo(CompetitionTypeBowType other)
        {
            var c = this.Type.CompareTo(other.Type);

            if (c == 0)
            {
                c = this.BowType.CompareTo(other.BowType);
            }

            return c;
        }

        public int CompareTo(object obj)
        {
            return this.CompareTo(this as CompetitionTypeBowType);
        }

        public double? GetArrowAverage(int average)
        {
            switch (this.Type)
            {
                case CompetitionType.Salle:
                    return average / 60.0;
                case CompetitionType.Federal:
                case CompetitionType.Fita:
                    return average / 72.0;
            }

            return null;
        }

        public double? GetTirAverage(int average)
        {
            switch (this.Type)
            {
                case CompetitionType.Salle:
                    return average / 20.0;
                case CompetitionType.Federal:
                case CompetitionType.Fita:
                    return average / 12.0;
            }

            return null;
        }
    }
}