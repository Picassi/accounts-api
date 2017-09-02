using System.Web.Http.ExceptionHandling;
using NLog;

namespace Picassi.Api.Accounts.Filters
{
    public class ApiExceptionLogger : ExceptionLogger
    {
        private readonly Logger _logger = LogManager.GetLogger("ExceptionLogger");

        public override void Log(ExceptionLoggerContext context)
        {
            _logger.Error(context.ExceptionContext.Exception.ToString());
        }
    }
}