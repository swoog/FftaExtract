namespace FftaExtract.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject.Extensions.Logging;

    using NSubstitute;

    using Xunit;

    public class ScrapUrlCalendrier
    {
        [Fact]
        public void Should_scrap_calendrier_When_have_two_date()
        {
            var archerDataProvider = RunScrapUrl(CompetitionType.Fita);

            Assert.Equal(2, archerDataProvider.Count);
            AssertCompetition(archerDataProvider[0], "ROUVRES", CompetitionType.Fita);
            AssertCompetition(archerDataProvider[1], "CACHY", CompetitionType.Fita);
        }

        [Fact]
        public void Should_scrap_calendrier_When_is_federal()
        {
            var archerDataProvider = RunScrapUrl(CompetitionType.Federal);

            Assert.Equal(3, archerDataProvider.Count);
            AssertCompetition(archerDataProvider[0], "NOYON", CompetitionType.Federal);
            AssertCompetition(archerDataProvider[1], "ROUVRES STADE", CompetitionType.Federal);
            AssertCompetition(archerDataProvider[2], "CACHY", CompetitionType.Federal);
        }

        private static void AssertCompetition(CompetitionDataProviderBase competitionDataProviderBase, string name, CompetitionType competitionType)
        {
            Assert.Equal(name, competitionDataProviderBase.Name);
            Assert.Equal(2015, competitionDataProviderBase.Year);
            Assert.Equal(new DateTime(2015, 8, 8), competitionDataProviderBase.Begin);
            Assert.Equal(new DateTime(2015, 8, 9), competitionDataProviderBase.End);
            Assert.Equal(competitionType, competitionDataProviderBase.CompetitionType);
        }

        private static IList<CompetitionDataProviderBase> RunScrapUrl(CompetitionType competitionType)
        {
            var palmares = new CalendrierProvider(null, Substitute.For<ILogger>(), null);
            
            return palmares.ScrapUrl(competitionType).Result;
        }
    }
}