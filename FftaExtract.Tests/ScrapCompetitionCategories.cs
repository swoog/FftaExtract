namespace FftaExtract.Tests
{
    using System.Linq;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using NSubstitute;

    using Xunit;
    using Pattern.Logging;

    public class ScrapCompetitionCategories
    {
        [Fact]
        public void Should_scrap_url_When_year_is_2009()
        {
            var competitionCategories = CompetitionCategoryMapping.GetCompetionCategoriesForYear(2009).Result;

            Assert.Equal(166, competitionCategories.Count);

            Assert.True(competitionCategories.Any(c => c.BowType == BowType.Classique
            && c.Category == Category.BenjaminFemme && c.CompetitionType == CompetitionType.Salle && c.IdFfta == 3422 && c.Sexe == Sexe.Femme && c.Year == 2009));
        }
    }
}
