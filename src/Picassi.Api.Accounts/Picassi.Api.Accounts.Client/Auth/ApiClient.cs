using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Picassi.Api.Accounts.Client.Auth
{
    public class ApiClient : IDisposable
    {
        private readonly ITokenManager _tokenManager;
        private HttpClient _client;
        private readonly JsonSerializerSettings _settings;

        public ApiClient(string baseUrl, ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };
            _settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }

        public T GetJson<T>(string relativeUrl)
        {
            var request = BuildRequest(HttpMethod.Get, relativeUrl);
            AddAuthentication();
            return ReadResponse<T>(request);
        }

        public HttpResponseMessage PostJson<T>(string relativeUrl, T content)
        {
            var request = BuildRequest(HttpMethod.Post, relativeUrl, content);
            AddAuthentication();
            return GetResponse(request);
        }

        public HttpResponseMessage PutJson<T>(string relativeUrl, T content)
        {
            var request = BuildRequest(HttpMethod.Put, relativeUrl, content);
            AddAuthentication();
            return GetResponse(request);
        }

        public HttpResponseMessage Delete(string relativeUrl)
        {
            var request = BuildRequest(HttpMethod.Delete, relativeUrl);
            AddAuthentication();
            return GetResponse(request);
        }

        public T GetResponseObj<T>(HttpResponseMessage response)
        {
            var jsonContent = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }

        private static void ValidateResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            var error = response.Content.ReadAsStringAsync().Result;
            throw new HttpRequestException(response.ReasonPhrase, new HttpResponseException(response.StatusCode, error));
        }

        private void AddAuthentication()
        {
            if (_tokenManager == null) return;

            var token = _tokenManager.GetAccessToken();
            _client.SetBearerToken(token);
        }

        private HttpRequestMessage BuildRequest(HttpMethod method, string relativeUrl, object content = null)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(_client.BaseAddress, relativeUrl),
                Method = method,
            };

            if (content == null) return request;

            var json = JsonConvert.SerializeObject(content, _settings);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return request;
        }

        private HttpResponseMessage GetResponse(HttpRequestMessage request)
        {
            return _client.SendAsync(request).Result;
        }

        private T ReadResponse<T>(HttpRequestMessage request)
        {
            var response = GetResponse(request);
            ValidateResponse(response);
            var jsonContent = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(jsonContent);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _client = null;
        }
    }
}