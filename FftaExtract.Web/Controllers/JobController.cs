namespace FftaExtract.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class JobController : ApiController
    {
        protected async Task<IHttpActionResult> Job(Action action)
        {
            return await this.Job(async () => await Task.Run(action));
        }

        protected async Task<IHttpActionResult> Job(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                return this.Json(new JobResult { Error = true, ErrorMessage = ex.ToString() });
            }

            return this.Json(new JobResult());
        }
    }
}