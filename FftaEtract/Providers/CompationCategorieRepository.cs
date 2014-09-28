using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FftaExtract.Providers
{
    using FftaExtract.DatabaseModel;

    public class CompationCategorieRepository
    {
        public IEnumerable<CompetitionCategory> GetCategories()
        {

            //3393, // SH CL 2009
            yield return new CompetitionCategory(CompetitionType.Salle, BowType.Classique, 5458, 2012); // Salle JH CL 2012 
            yield return new CompetitionCategory(CompetitionType.Salle, BowType.Classique, 5460, 2012); // Salle SH CL 2012 
            yield return new CompetitionCategory(CompetitionType.Salle, BowType.Classique, 6121, 2013); // Salle SH CL 2013
            yield return new CompetitionCategory(CompetitionType.Salle, BowType.Classique, 6802, 2014); // Salle SH CL 2014
            yield return new CompetitionCategory(CompetitionType.Fita, BowType.Classique, 7361, 2014); // FITA SH CL 2014
            //3394, // SF CL 2009
        }
    }
}
