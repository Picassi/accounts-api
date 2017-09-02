using System;
using System.Net;

namespace Picassi.Api.Accounts.Client.Auth
{
    public class HttpResponseException : Exception
    {
        private HttpStatusCode statusCode;

        public HttpResponseException(HttpStatusCode statusCode, string content = null) : base(content)
        {
            this.statusCode = statusCode;
        }
    }
}
