using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using NLog;

namespace Picassi.Api.Accounts.Filters
{
    public class ApiRequestLogger : DelegatingHandler
    {
        private readonly Logger _logger = LogManager.GetLogger("RequestLogger");

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var clientIp = GetClientIp(request);
            _logger.Info("Request Received ({0}): {1}", clientIp, request.RequestUri);
            return await base.SendAsync(request, cancellationToken);
        }

        private static string GetClientIp(HttpRequestMessage request)
        {
            return request.Properties.ContainsKey("MS_HttpContext") ?
                ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress :
                "N/A";
        }
    }
}