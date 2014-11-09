﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FftaExtract.Providers
{
    using System.Collections;

    using FftaExtract.DatabaseModel;

    public class CompetionCategorieRepository
    {
        private static int[] years = new[] { 2013, 2014, 2015 };

        private static Dictionary<BowType, string> bowTypeToText = new Dictionary<BowType, string>()
                                                         {
                                                             { BowType.Classique, "CL" },
                                                             { BowType.Poulie, "CO" },
                                                         };

        private static Dictionary<Category, string> categoryToText = new Dictionary<Category, string>()
                                                         {
                                                             { Category.JuniorHomme, "JH" },
                                                             { Category.JuniorFemme, "JF" },
                                                             { Category.SeniorHomme, "SH" },
                                                             { Category.SeniorFemme, "SF" },
                                                             { Category.VeteranHomme, "VH" },
                                                             { Category.VeteranFemme, "VF" },
                                                             { Category.ScratchHomme, "SCH" },
                                                             { Category.ScratchFemme, "SCF" },
                                                         };

        private static CompetitionCategory[] categories = null;

        private static CompetitionCategory[] categoriesHomme = null;
        private static CompetitionCategory[] categoriesFemme = null;

        static CompetionCategorieRepository()
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

            foreach (var year in years)
            {
                foreach (var competitionType in competitionTypes)
                {
                    foreach (var category in categories)
                    {
                        foreach (var bowType in bowTypes)
                        {
                            var key = year + "_" +
                                competitionType + "_" +
                                categoryToText[category] + "_" +
                                bowTypeToText[bowType];

                            if (CompetitionCategoryMapping.ignoredCategories.Contains(key))
                            {
                                continue;
                            }

                            if (!CompetitionCategoryMapping.code.ContainsKey(key))
                            {
                                throw new NotImplementedException(key);
                            }

                            yield return new CompetitionCategory(competitionType, bowType, CompetitionCategoryMapping.code[key], year, key.Contains("H_") ? Sexe.Homme : Sexe.Femme, category);
                        }
                    }
                }
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
