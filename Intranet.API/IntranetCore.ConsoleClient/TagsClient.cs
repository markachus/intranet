using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace IntranetCore.ConsoleClient
{
    public class TagsClient
    {

        public TagsClient(HttpClient client)
        {
            Client = client;
            client.BaseAddress = new Uri("http://localhost:51044/");
            client.Timeout = new TimeSpan(0, 0, 30);
            client.DefaultRequestHeaders.Clear();
        }

        public HttpClient Client { get; }
    }
}
