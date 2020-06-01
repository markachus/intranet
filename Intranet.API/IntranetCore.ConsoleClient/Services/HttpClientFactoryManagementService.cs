using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IntranetCore.ConsoleClient.Models;
using Marvin.StreamExtensions;

namespace IntranetCore.ConsoleClient.Services
{
    public class HttpClientFactoryManagementService : IIntegrationService
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TagsClient _tagsClient;

        public HttpClientFactoryManagementService(IHttpClientFactory httpClientFactory, TagsClient tagsClient)
        {
            this._httpClientFactory = httpClientFactory;
            this._tagsClient = tagsClient;
        }

        async Task IIntegrationService.Run()
        {
            //await TestReuseHttpClient(_cancellationTokenSource.Token);
            //await GetTagsWithNamedHttpClientFactory(_cancellationTokenSource.Token);
            await GetTagsWithTypedHttpClientFactory(_cancellationTokenSource.Token);
        }

        public async Task GetDisposingHttpClient(CancellationToken cancellationToken) {

            for (int i = 0; i <= 10; i++) { 
                using(HttpClient client = new HttpClient())
                {
                    HttpRequestMessage req = new HttpRequestMessage(
                        HttpMethod.Get,
                        "http://www.google.com");

                        using (var response = await client.SendAsync(req, 
                            HttpCompletionOption.ResponseHeadersRead,
                            cancellationToken)) {

                            var content = await response.Content.ReadAsStreamAsync();
                            response.EnsureSuccessStatusCode();

                            Console.WriteLine($"Request completed with status code {response.StatusCode}");

                        }
                }
            }
        }

        public async Task TestReuseHttpClient(CancellationToken cancellationToken)
        {


            HttpClient client = new HttpClient();

                for (int i = 0; i <= 10; i++)
            {
                    HttpRequestMessage req = new HttpRequestMessage(
                        HttpMethod.Get,
                        "http://www.google.com");

                    using (var response = await client.SendAsync(req,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken))
                    {

                        var content = await response.Content.ReadAsStreamAsync();
                        response.EnsureSuccessStatusCode();

                        Console.WriteLine($"Request completed with status code {response.StatusCode}");

                    }
                }
        }

        private async Task GetTagsWithHttpClientFactory(CancellationToken cancellationToken) {

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri("http://localhost:51044/");
            httpClient.Timeout = new TimeSpan(0, 0, 30);
            httpClient.DefaultRequestHeaders.Clear();

            var request = new HttpRequestMessage(
                HttpMethod.Get, 
                "/api/v1/tags");

            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await httpClient.SendAsync(
                request, 
                HttpCompletionOption.ResponseHeadersRead, 
                cancellationToken)) {

                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var tags =stream.ReadAndDeserializeFromJson<IEnumerable<EtiquetaModel>>();
            }

            
        }


        private async Task GetTagsWithNamedHttpClientFactory(CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("TagsClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/v1/tags");

            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken))
            {

                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var tags = stream.ReadAndDeserializeFromJson<IEnumerable<EtiquetaModel>>();
            }
        }

        private async Task GetTagsWithTypedHttpClientFactory(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/v1/tags");

            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await _tagsClient.Client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken))
            {

                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var tags = stream.ReadAndDeserializeFromJson<IEnumerable<EtiquetaModel>>();
            }


        }


    }
}
