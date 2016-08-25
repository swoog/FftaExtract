namespace FftaExtract.Tests
{
    using System;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using NSubstitute;

    using Xunit;
    using Pattern.Logging;

    public class ScrapUrlPalmares
    {
        [Fact]
        public void Should_scrap_url_When_year_is_2009()
        {
            var archerDataProvider = RunScrapUrl("464853", CompetitionType.Campagne, BowType.Classique, 3567, 2009, Sexe.Homme, Category.CadetHomme);

            Assert.Equal(3, archerDataProvider.Competitions.Count);
            Assert.Equal(2009, archerDataProvider.Competitions[0].Year);
        }

        [Fact]
        public void Should_scrap_url_When_year_is_2010()
        {
            var archerDataProvider = RunScrapUrl("464853", CompetitionType.Campagne, BowType.Classique, 4229, 2010, Sexe.Homme, Category.CadetHomme);

            Assert.Equal(0, archerDataProvider.Competitions.Count);
        }

        [Fact]
        public void Should_scrap_url_When_year_is_2010_and_junior()
        {
            var archerDataProvider = RunScrapUrl("464853", CompetitionType.Campagne, BowType.Classique, 4231, 2010, Sexe.Homme, Category.JuniorHomme);

            Assert.Equal(0, archerDataProvider.Competitions.Count);
        }

        [Fact]
        public void Should_scrap_url_When_year_is_2010_and_classique_senior()
        {
            var archerDataProvider = RunScrapUrl("761424", CompetitionType.Fita, BowType.Classique, 4290, 2010, Sexe.Homme, Category.SeniorHomme);

            Assert.Equal(4, archerDataProvider.Competitions.Count);
            AssertCompetition(0, archerDataProvider, 566, new DateTime(2010, 9, 18));
            AssertCompetition(1, archerDataProvider, 569, new DateTime(2010, 6, 27));
            AssertCompetition(2, archerDataProvider, 531, new DateTime(2010, 5, 16));
            AssertCompetition(3, archerDataProvider, 524, new DateTime(2010, 6, 13));
        }

        private static void AssertCompetition(int index, ArcherDataProvider archerDataProvider, int score, DateTime date)
        {
            Assert.Equal(score, archerDataProvider.Competitions[index].Score);
            Assert.Equal(date, archerDataProvider.Competitions[index].Begin);
            Assert.Equal(2010, archerDataProvider.Competitions[index].Year);
        }

        [Fact]
        public void Should_scrap_url_When_year_is_2016_and_senior_homme()
        {
            var archerDataProvider = RunScrapUrl("359095", CompetitionType.Salle, BowType.Classique, 9427, 2016, Sexe.Homme, Category.SeniorHomme);

            Assert.Equal(3, archerDataProvider.Competitions.Count);
            Assert.Equal(530, archerDataProvider.Competitions[0].Score);
            Assert.Equal(2016, archerDataProvider.Competitions[0].Year);
            Assert.Equal(7, archerDataProvider.Competitions[0].Rank);
            Assert.Equal(513, archerDataProvider.Competitions[1].Score);
            Assert.Equal(2016, archerDataProvider.Competitions[1].Year);
            Assert.Equal(6, archerDataProvider.Competitions[1].Rank);
            Assert.Equal(496, archerDataProvider.Competitions[2].Score);
            Assert.Equal(2016, archerDataProvider.Competitions[2].Year);
            Assert.Equal(30, archerDataProvider.Competitions[2].Rank);
        }

        private static ArcherDataProvider RunScrapUrl(string codeArcher, CompetitionType competitionType, BowType bowType, int idFfta, int year, Sexe sex, Category category)
        {
            var palmares = new PalmaresProvider(null, Substitute.For<ILogger>());
            var archerDataProvider = new ArcherDataProvider(codeArcher);
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
