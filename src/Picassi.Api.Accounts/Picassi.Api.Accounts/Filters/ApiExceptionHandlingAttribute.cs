using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using NLog;

namespace Picassi.Api.Accounts.Filters
{
    // Most of this class is covered by tests, but code coverage doesn't deal well with the fact that there is no path
    // to the end of the method (every path throws an exception) so we exclude it to avoid erroneous coverage %
    [ExcludeFromCodeCoverage]
    public class ApiExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        private static readonly Logger Logger = LogManager.GetLogger("ApiExceptionLogger");

        public override void OnException(HttpActionExecutedContext context)
        {
            Logger.Error(context.Exception, "Unexpected error");
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(GetContent(context.Exception))
            };
            throw new HttpResponseException(response);
        }

        private string GetContent(Exception exc)
        {
            var ret = exc.Message + "\n" + exc.StackTrace + "\n";
            if (exc.InnerException == null) return ret;

            ret += "============================================================";
            ret += GetContent(exc.InnerException);
            return ret;
        }
    }
}
