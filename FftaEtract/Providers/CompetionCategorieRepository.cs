using System;
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
        private static int[] years = new[] { 2014 };

        private static string[] ignoredCategories = new[]
                                                 {
                                                     "2014_Fita_SH_CL",
                                                    "2014_Fita_SF_CL",
                                                     "2014_Fita_SH_CO",
                                                    "2014_Fita_SF_CO",
                                                 };


        static Dictionary<string, int> code = new Dictionary<string, int>()
                                           {
                                               {"2009_Salle_SH_CL", 3393 },
                                               {"2009_Salle_SF_CL", 3394 },
                                               {"2012_Salle_JH_CL",  5458},


                                            { "2014_Salle_CH_BB", 6825},
                                            { "2014_Salle_CF_BB", 6824},
                                            { "2014_Salle_SCF_BB", 6792},
                                            { "2014_Salle_SCH_BB", 6791},
                                            { "2014_Salle_BF_CL", 6793},
                                            { "2014_Salle_BH_CL", 6794},
                                            { "2014_Salle_MF_CL", 6795},
                                            { "2014_Salle_MH_CL", 6796},
                                            { "2014_Salle_CF_CL", 6797},
                                            { "2014_Salle_CH_CL", 6798},
                                            { "2014_Salle_JF_CL", 6799},
                                            { "2014_Salle_JH_CL", 6800},
                                            { "2014_Salle_SF_CL", 6801},
                                            { "2014_Salle_SH_CL", 6802},
                                            { "2014_Salle_VF_CL", 6803},
                                            { "2014_Salle_VH_CL", 6804},
                                            { "2014_Salle_SVF_CL", 6805},
                                            { "2014_Salle_SVH_CL", 6806},
                                            { "2014_Salle_CF_CO", 6822},
                                            { "2014_Salle_CH_CO", 6823},
                                            { "2014_Salle_JF_CO", 6807},
                                            { "2014_Salle_JH_CO", 6808},
                                            { "2014_Salle_SF_CO", 6809},
                                            { "2014_Salle_SH_CO", 6810},
                                            { "2014_Salle_VF_CO", 6811},
                                            { "2014_Salle_VH_CO", 6812},
                                            { "2014_Salle_SVF_CO", 6813},
                                            { "2014_Salle_SVH_CO", 6814},


                                            { "2014_Fita_BF_CL", 7250},
                                            { "2014_Fita_BH_CL", 7251},
                                            { "2014_Fita_MF_CL", 7252},
                                            { "2014_Fita_MH_CL", 7253},
                                            { "2014_Fita_CF_CL", 7254},
                                            { "2014_Fita_CH_CL", 7255},
                                            { "2014_Fita_JF_CL", 7256},
                                            { "2014_Fita_JH_CL", 7257},
                                            { "2014_Fita_VF_CL", 7260},
                                            { "2014_Fita_VH_CL", 7261},
                                            { "2014_Fita_SVF_CL", 7262},
                                            { "2014_Fita_SVH_CL", 7263},
                                            { "2014_Fita_SCF_CL", 7360},
                                            { "2014_Fita_SCH_CL", 7361},
                                            { "2014_Fita_CF_CO", 7275},
                                            { "2014_Fita_CH_CO", 7272},
                                            { "2014_Fita_JF_CO", 7264},
                                            { "2014_Fita_JH_CO", 7265},
                                            { "2014_Fita_VF_CO", 7268},
                                            { "2014_Fita_VH_CO", 7269},
                                            { "2014_Fita_SVF_CO", 7270},
                                            { "2014_Fita_SVH_CO", 7271},
                                            { "2014_Fita_SCF_CO", 7362},
                                            { "2014_Fita_SCH_CO", 7363},


                                            };

        private static Dictionary<BowType, string> bowTypeToText = new Dictionary<BowType, string>()
                                                         {
                                                             { BowType.Classique, "CL"},
                                                             { BowType.Poulie, "CO"},
                                                         };

        private static Dictionary<Category, string> categoryToText = new Dictionary<Category, string>()
                                                         {
                                                             { Category.JuniorHomme, "JH"},
                                                             { Category.SeniorHomme, "SH"},
                                                             { Category.JuniorFemme, "JF"},
                                                             { Category.SeniorFemme, "SF"},
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

        public IEnumerable<CompetitionCategory> GetCategories(Sexe? sexe)
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

                            if (ignoredCategories.Contains(key))
                            {
                                continue;
                            }

                            if (!code.ContainsKey(key))
                            {
                                throw new NotImplementedException(key);
                            }

                            yield return new CompetitionCategory(competitionType, bowType, code[key], year, key.Contains("H_") ? Sexe.Homme : Sexe.Femme);
                        }
                    }
                }
            }
        }

        private static T[] GetTypes<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
}
