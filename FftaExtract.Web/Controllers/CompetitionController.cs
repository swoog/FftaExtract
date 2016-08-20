using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FftaExtract.Web.Controllers
{
    using System.Collections;
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Resources;
    using System.Threading.Tasks;
    using System.Web;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Newtonsoft.Json;

    public class CompetitionController : JobController
    {
        private readonly IRepository competitionCategorieRepository;

        private readonly Job job;

        public CompetitionController(Job job, IRepository competitionCategorieRepository)
        {
            this.job = job;
            this.competitionCategorieRepository = competitionCategorieRepository;
        }

        [Route("api/competition")]
        public async Task<IHttpActionResult> Get()
        {
            return await this.Job(
                () =>
                {
                    foreach (var competitionInfo in this.competitionCategorieRepository.GetCompetitionWithoutLocation())
                    {
                        this.job.Push($"api/competition/{competitionInfo.Id}");
                    }
                });
        }

        [Route("api/competition/{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            return await this.Job(
                       async () =>
                           {
                               var competitionInfo = this.competitionCategorieRepository.GetCompetitionInfo(id);

                               if (competitionInfo.Location == null)
                               {
                                   var location = await FindLocation(competitionInfo.Name);

                                   foreach (var resourceSet in location.resourceSets)
                                   {
                                       if (resourceSet.estimatedTotal == 1)
                                       {
                                           var r = resourceSet.resources.Single();

                                           var point = r.point;

                                           var locationDb = DbGeography.FromText(string.Format(CultureInfo.InvariantCulture, "POINT({0} {1})", point.coordinates[0], point.coordinates[1]));

                                           this.competitionCategorieRepository.SaveCompetionInfoLocation(id, locationDb);
                                       }
                                   }
                               }
                           });
        }

        private async Task<Location> FindLocation(string competitionInfoName)
        {
            var urlQuery =
                "http://dev.virtualearth.net/REST/v1/Locations?q={0}%20France&o=json&key=AiLxJ448im16__TCg5HH0XqULMc8RliJk6duJ9FnNACKEeaX8OcJRoimBULZrmjj";

            var url = string.Format(urlQuery, HttpUtility.UrlEncode(competitionInfoName));

            var client = new HttpClient();

            var content = await client.GetStringAsync(url);

            var locationResult = JsonConvert.DeserializeObject<Location>(content);

            return locationResult;
        }
    }

    public class Location
    {
        public ResourceSetLocation[] resourceSets;
    }

    public class ResourceSetLocation
    {
        public int estimatedTotal;

        public ResourceLocation[] resources;
    }

    public class ResourceLocation
    {
        public PointLocation point;
    }

    public class PointLocation
    {
        public double[] coordinates;
    }
}
