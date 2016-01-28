namespace FftaExtract.Web
{
    using System.Web.Http.Filters;

    using Ninject.Extensions.Logging;

    public class LogError : ExceptionFilterAttribute
    {
        private ILogger logger;

        public LogError(ILogger logger)
        {
            this.logger = logger;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            this.logger.Error("Error in API : ", actionExecutedContext.Exception);
        }

    }
}