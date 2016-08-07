using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FftaExtract.Tests
{
    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject.Extensions.Logging;

    using NSubstitute;

    using Xunit;

    public class ScrapUrl
    {
        [Fact]
        public void Should_scrap_url_When_year_is_2009()
        {
            var archerDataProvider = RunScrapUrl("464853A", CompetitionType.Campagne, BowType.Classique, 3567, 2009, Sexe.Homme, Category.CadetHomme);

            Assert.Equal(3, archerDataProvider.Competitions.Count);
            Assert.Equal(2009, archerDataProvider.Competitions[0].Year);
        }

        [Fact]
        public void Should_scrap_url_When_year_is_2010()
        {
            var archerDataProvider = RunScrapUrl("464853A", CompetitionType.Campagne, BowType.Classique, 4229, 2010, Sexe.Homme, Category.CadetHomme);

            Assert.Equal(0, archerDataProvider.Competitions.Count);
        }

        [Fact]
        public void Should_scrap_url_When_year_is_2010_and_junior()
        {
            var archerDataProvider = RunScrapUrl("464853A", CompetitionType.Campagne, BowType.Classique, 4231, 2010, Sexe.Homme, Category.JuniorHomme);

            Assert.Equal(0, archerDataProvider.Competitions.Count);
        }

        private static ArcherDataProvider RunScrapUrl(string codeArcher, CompetitionType competitionType, BowType bowType, int idFfta, int year, Sexe sex, Category category)
        {
            var palmares = new PalmaresProvider(null, Substitute.For<ILogger>());
            var archerDataProvider = new ArcherDataProvider() { Code = codeArcher };
            var competitionCategory = new CompetitionCategory(
                competitionType,
                bowType,
                idFfta,
                year,
                sex,
                category);

            palmares.ScrapUrl(competitionCategory, archerDataProvider).Wait();
            return archerDataProvider;
        }
    }
}
