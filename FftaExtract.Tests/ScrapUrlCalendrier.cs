namespace FftaExtract.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Xunit;

    public class ScrapUrlCalendrier
    {
        [Fact]
        public void Should_scrap_calendrier_When_have_two_date()
        {
            var archerDataProvider = RunScrapUrl(CompetitionType.Fita, new DateTime(2015, 8, 8), new DateTime(2015, 8, 8));

            Assert.Equal(2, archerDataProvider.Count);
            AssertCompetition(archerDataProvider[0], "ROUVRES", CompetitionType.Fita, new DateTime(2015, 8, 8), new DateTime(2015, 8, 9), 2015);
            AssertCompetition(archerDataProvider[1], "CACHY", CompetitionType.Fita, new DateTime(2015, 8, 8), new DateTime(2015, 8, 9), 2015);
        }

        [Fact]
        public void Should_scrap_calendrier_When_is_federal()
        {
            var archerDataProvider = RunScrapUrl(CompetitionType.Federal, new DateTime(2015, 8, 8), new DateTime(2015, 8, 8));

            Assert.Equal(3, archerDataProvider.Count);
            AssertCompetition(archerDataProvider[0], "NOYON", CompetitionType.Federal, new DateTime(2015, 8, 8), new DateTime(2015, 8, 9), 2015);
            AssertCompetition(archerDataProvider[1], "ROUVRES STADE", CompetitionType.Federal, new DateTime(2015, 8, 8), new DateTime(2015, 8, 9), 2015);
            AssertCompetition(archerDataProvider[2], "CACHY", CompetitionType.Federal, new DateTime(2015, 8, 8), new DateTime(2015, 8, 9), 2015);
        }

        [Fact]
        public void Should_scrap_calendrier_When_is_federal_and_other_date()
        {
            var archerDataProvider = RunScrapUrl(CompetitionType.Federal, new DateTime(2015, 8, 15), new DateTime(2015, 8, 15));

            Assert.Equal(2, archerDataProvider.Count);
            AssertCompetition(archerDataProvider[0], "VILLE SOUS LA FERTE", CompetitionType.Federal, new DateTime(2015, 8, 15), new DateTime(2015, 8, 16), 2015);
            AssertCompetition(archerDataProvider[1], "NOYON", CompetitionType.Federal, new DateTime(2015, 8, 15), new DateTime(2015, 8, 16), 2015);
        }

        [Fact]
        public void Should_scrap_calendrier_When_is_salle_and_other_date()
        {
            var archerDataProvider = RunScrapUrl(CompetitionType.Salle, new DateTime(2016, 9, 17), new DateTime(2016, 9, 19));

            Assert.Equal(1, archerDataProvider.Count);
            AssertCompetition(archerDataProvider[0], "NOUMEA", CompetitionType.Salle, new DateTime(2016, 9, 18), new DateTime(2016, 9, 18), 2017);
        }

        [Fact]
        public void Should_scrap_calendrier_When_name_containe_special_character()
        {
            var archerDataProvider = RunScrapUrl(CompetitionType.Salle, new DateTime(2016, 11, 19), new DateTime(2016, 11, 19));

            Assert.Equal(51, archerDataProvider.Count);
            Assert.True(archerDataProvider.Any(a => a.Name == "CESSON SÉVIGNÉ"));
            AssertCompetition(archerDataProvider.First(a => a.Name == "CESSON SÉVIGNÉ"), "CESSON SÉVIGNÉ", CompetitionType.Salle, new DateTime(2016, 11, 19), new DateTime(2016, 11, 20), 2017);
        }

        private static void AssertCompetition(CompetitionDataProviderBase competitionDataProviderBase, string name, CompetitionType competitionType, DateTime beginDate, DateTime endDate, int year)
        {
            Assert.Equal(name, competitionDataProviderBase.Name);
            Assert.Equal(year, competitionDataProviderBase.Year);
            Assert.Equal(beginDate, competitionDataProviderBase.Begin);
            Assert.Equal(endDate, competitionDataProviderBase.End);
            Assert.Equal(competitionType, competitionDataProviderBase.CompetitionType);
        }

        private static IList<CompetitionDataProviderBase> RunScrapUrl(CompetitionType competitionType, DateTime beginDate, DateTime endDate)
        {
            var palmares = new CalendrierProvider(null);

            return palmares.ScrapUrl(competitionType, beginDate, endDate).Result;
        }
    }
}