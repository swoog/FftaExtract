namespace FftaExtract.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject.Extensions.Logging;

    using NSubstitute;

    using Xunit;

    public class ScrapUrlClassment
    {
        [Fact]
        public void Should_code_property_of_archer_is_a_number_When_scrap_classment()
        {
            var archerDataProvider = RunScrapUrl(CompetitionType.Campagne, BowType.Poulie, 3574, 2009, Sexe.Femme, Category.JuniorFemme);

            Assert.Equal(4, archerDataProvider.Count);
            Assert.True(archerDataProvider.All(a => a.Sexe == Sexe.Femme));
            AssertArcher(archerDataProvider[0], "642176T", "642176", "DUBOS CAMILLE");
            AssertArcher(archerDataProvider[1], "624627T", "624627", "GRIMAULT CLAIRE");
            AssertArcher(archerDataProvider[2], "624064F", "624064", "GAILLOT CAMILLE");
            AssertArcher(archerDataProvider[3], "679524N", "679524", "BAVILLE MARION");
        }

        private static void AssertArcher(ArcherDataProvider dataProvider, string archerCode, string code, string lastName)
        {
            Assert.Equal(code, dataProvider.Code);
            Assert.Equal(archerCode, dataProvider.ArcherCode);
            Assert.Equal("", dataProvider.FirstName);
            Assert.Equal(lastName, dataProvider.LastName);
        }

        private static IList<ArcherDataProvider> RunScrapUrl(CompetitionType competitionType, BowType bowType, int idFfta, int year, Sexe sex, Category category)
        {
            var palmares = new ClassmentProvider(null, Substitute.For<ILogger>(), null);
            var competitionCategory = new CompetitionCategory(
                competitionType,
                bowType,
                idFfta,
                year,
                sex,
                category);

            return palmares.ScrapUrl(competitionCategory).Result;
        }
    }
}