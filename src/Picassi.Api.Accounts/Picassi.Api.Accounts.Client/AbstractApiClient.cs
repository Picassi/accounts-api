using System;
using System.Net.Http;
using Picassi.Api.Accounts.Client.Auth;

namespace Picassi.Api.Accounts.Client
{
    public abstract class AbstractApiClient
    {
        protected readonly ApiClient ApiClient;

        protected AbstractApiClient(ApiClient apiClient)
        {
            ApiClient = apiClient;
        }

        protected void ValidateApiClient()
        {
            if (ApiClient == null) throw new InvalidOperationException("No client configured");
        }

        protected static void HandleErrorResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            var content = response.Content.ReadAsStringAsync().Result;
            throw new HttpRequestException($"Response {response.StatusCode}: " + response.ReasonPhrase + ", " + content);
        }

    }
}
