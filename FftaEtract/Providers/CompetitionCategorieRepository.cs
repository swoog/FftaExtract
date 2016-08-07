
namespace FftaExtract.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using System.Text.RegularExpressions;

    using FftaExtract.DatabaseModel;

    public class CompetitionCategorieRepository
    {
        public static int[] Years { get; } = { 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016 };

        private static readonly Dictionary<BowType, string> bowTypeToText = new Dictionary<BowType, string>()
                                                         {
                                                             { BowType.Classique, "CL" },
                                                             { BowType.Poulie, "CO" },
                                                             { BowType.BareBow, "BB" },
                                                         };

        private static readonly Dictionary<Category, string> categoryToText = new Dictionary<Category, string>()
                                                         {
                                                             { Category.JeuneHomme, "JEH" },
                                                             { Category.JeuneFemme, "JEF" },
                                                             { Category.JuniorHomme, "JH" },
                                                             { Category.JuniorFemme, "JF" },
                                                             { Category.SeniorHomme, "SH" },
                                                             { Category.SeniorFemme, "SF" },
                                                             { Category.VeteranHomme, "VH" },
                                                             { Category.VeteranFemme, "VF" },
                                                             { Category.ScratchHomme, "SCH" },
                                                             { Category.ScratchFemme, "SCF" },
                                                             { Category.CadetHomme, "CH" },
                                                             { Category.CadetFemme, "CF" },
                                                             { Category.BenjaminHomme, "BH" },
                                                             { Category.BenjaminFemme, "BF" },
                                                             { Category.MinimeHomme, "MH" },
                                                             { Category.MinimeFemme, "MF" },
                                                             { Category.SuperVeteranHomme, "SVH" },
                                                             { Category.SuperVeteranFemme, "SVF" },
                                                         };

        private static CompetitionCategory[] categories = null;

        private static CompetitionCategory[] categoriesHomme = null;
        private static CompetitionCategory[] categoriesFemme = null;

        private static readonly Dictionary<CompetitionType, string> conmpetitionTypeToText = new Dictionary<CompetitionType, string>()
                                                         {
                                                             { CompetitionType.Salle, "Salle" },
                                                             { CompetitionType.Fita, "Fita" },
                                                             { CompetitionType.Federal, "Federal" },
                                                             { CompetitionType.Campagne, "Campagne" },
                                                             { CompetitionType.Parcour3D, "3D" },
                                                         };

        private readonly Dictionary<CompetitionType, string> conmpetitionTypeToCode = new Dictionary<CompetitionType, string>
                                                                                 {
                                                                                     {
                                                                                         CompetitionType
                                                                                         .Salle,
                                                                                         "S"
                                                                                     },
                                                                                     {
                                                                                         CompetitionType
                                                                                         .Fita,
                                                                                         "F"
                                                                                     },
                                                                                     { CompetitionType.Federal, "E" },
                                                                                     { CompetitionType.Campagne, "C" },
                                                                                     { CompetitionType.Parcour3D, "3" },
                                                                                 };

        private static void InitializeCategories()
        {
            if (categories == null)
            {
                lock (Years)
                {
                    if (categories == null)
                    {
                        categories = InternalGetCategories().ToArray();
                        categoriesHomme = categories.Where(c => c.Sexe == Sexe.Homme).ToArray();
                        categoriesFemme = categories.Where(c => c.Sexe == Sexe.Femme).ToArray();
                    }
                }
            }
        }

        public IEnumerable<CompetitionCategory> GetCategories(Sexe? sexe, int? year)
        {
            InitializeCategories();

            if (year.HasValue)
            {
                return this.GetInternalCategories(sexe).Where(c => c.Year == year);
            }

            return this.GetInternalCategories(sexe);
        }

        private IEnumerable<CompetitionCategory> GetInternalCategories(Sexe? sexe)
        {
            if (sexe.HasValue)
            {
                if (sexe == Sexe.Homme)
                {
                    return categoriesHomme;
                }

                if (sexe == Sexe.Femme)
                {
                    return categoriesFemme;
                }
            }

            return categories;
        }

        private static IEnumerable<CompetitionCategory> InternalGetCategories()
        {
            foreach (var year in Years)
            {
                CompetitionCategoryMapping.LoadYear(year);
            }

            foreach (var competitionCategory in CompetitionCategoryMapping.CompetitionCategories)
            {
                yield return competitionCategory;
            }
        }

        public CompetitionCategory GetCategory(int year, Category cat, CompetitionType competitionType, BowType bowType)
        {
            InitializeCategories();

            var q = from c in categories
                    where
                        c.CompetitionType == competitionType && c.Category == cat && c.Year == year
                        && c.BowType == bowType
                    select c;
            return q.SingleOrDefault();
        }

        public IEnumerable<CompetitionTypeInfo> GetCompetitionTypes()
        {
            InitializeCategories();

            foreach (var competition in Enum.GetValues(typeof(CompetitionType)).Cast<CompetitionType>())
            {
                yield return new CompetitionTypeInfo { Code = conmpetitionTypeToCode[competition], Name = conmpetitionTypeToText[competition], EnumType = competition };
            }
        }
    }

    public class CompetitionTypeInfo
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public CompetitionType EnumType { get; set; }
    }
}
