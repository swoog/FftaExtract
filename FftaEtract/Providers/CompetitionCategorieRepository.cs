using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FftaExtract.Providers
{
    using System.Collections;
    using System.Text.RegularExpressions;

    using FftaExtract.DatabaseModel;

    public class CompetitionCategorieRepository
    {
        private static int[] years = new[] { 2011, 2012, 2013, 2014, 2015 };

        private static Dictionary<BowType, string> bowTypeToText = new Dictionary<BowType, string>()
                                                         {
                                                             { BowType.Classique, "CL" },
                                                             { BowType.Poulie, "CO" },
                                                             { BowType.BareBow, "BB" },
                                                         };

        private static Dictionary<Category, string> categoryToText = new Dictionary<Category, string>()
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
                                                         };

        private static CompetitionCategory[] categories = null;

        private static CompetitionCategory[] categoriesHomme = null;
        private static CompetitionCategory[] categoriesFemme = null;

        private static new Dictionary<CompetitionType, string> conmpetitionTypeToText = new Dictionary<CompetitionType, string>()
                                                         {
                                                             { CompetitionType.Salle, "Salle" },
                                                             { CompetitionType.Fita, "Fita" },
                                                             { CompetitionType.Federal, "Federal" },
                                                             { CompetitionType.Campagne, "Campagne" },
                                                             { CompetitionType.Parcour3D, "3D" },
                                                         };

        static CompetitionCategorieRepository()
        {
            categories = InternalGetCategories().ToArray();
            categoriesHomme = categories.Where(c => c.Sexe == Sexe.Homme).ToArray();
            categoriesFemme = categories.Where(c => c.Sexe == Sexe.Femme).ToArray();
        }

        public IEnumerable<CompetitionCategory> GetCategories(Sexe? sexe, int? year)
        {
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
            CompetitionType[] competitionTypes = GetTypes<CompetitionType>();
            BowType[] bowTypes = GetTypes<BowType>();
            Category[] categories = GetTypes<Category>();

            var regexes = CompetitionCategoryMapping.ignoredCategories.Select(c => new Regex(string.Format("^{0}$", c))).ToList();

            var sb = new StringBuilder();

            foreach (var year in years)
            {
                foreach (var competitionType in competitionTypes)
                {
                    foreach (var category in categories)
                    {
                        foreach (var bowType in bowTypes)
                        {
                            var key = year + "_" +
                                conmpetitionTypeToText[competitionType] + "_" +
                                categoryToText[category] + "_" +
                                bowTypeToText[bowType];

                            //if (regexes.Any(r => r.Match(key).Success))
                            //{
                            //    continue;
                            //}

                            if (!CompetitionCategoryMapping.code.ContainsKey(key))
                            {
                                //sb.AppendLine(key);
                            }
                            else
                            {
                                yield return new CompetitionCategory(competitionType, bowType, CompetitionCategoryMapping.code[key], year, key.Contains("H_") ? Sexe.Homme : Sexe.Femme, category);
                            }
                        }
                    }
                }
            }

            if (sb.Length > 0)
            {
                throw new NotImplementedException(sb.ToString());
            }
        }

        private static T[] GetTypes<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }

        public CompetitionCategory GetCategory(int year, Category cat, CompetitionType competitionType, BowType bowType)
        {
            var q = from c in categories
                    where
                        c.CompetitionType == competitionType && c.Category == cat && c.Year == year
                        && c.BowType == bowType
                    select c;
            return q.SingleOrDefault();
        }
    }
}
