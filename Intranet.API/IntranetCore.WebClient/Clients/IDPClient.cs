using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntranetCore.WebClient.Clients
{
    public class IDPClient
    {

        private readonly HttpClient _httpClient;

        public IDPClient(HttpClient httpClient)
        {
            _httpClient = httpClient; 
            this._httpClient.BaseAddress = new Uri("https://localhost:44318");
            this._httpClient.Timeout = new TimeSpan(0, 0, 30);
            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

    }
}
