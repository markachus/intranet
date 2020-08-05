using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntranetCore.Data.ResourceParameters;
using IntranetCore.WebClient.Clients;
using IntranetCore.WebClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IdentityModel.Client;
using System.Net.Http;

namespace IntranetCore.WebClient.Pages.Tags
{
    [AuthorizeAttribute]
    public class ListModel : PageModel
    {
        private readonly TagsClient _client;
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient _IDPClient;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public IEnumerable<EtiquetaModel> Tags { get; set; }

        [BindProperty(SupportsGet = true)]
        public EtiquetasResourceParameters TagParams {get; set;}


        public ListModel(TagsClient client, IHttpClientFactory clientFactory)
        {
            this._client = client;
            this._clientFactory = clientFactory;
        }

        public IEnumerable<EtiquetaModel> Etiquetas { get; set; }

        public async void OnGet()
        {
            Tags = new List<EtiquetaModel>();
            Tags = _client.GetTags(TagParams, _cancellationTokenSource.Token).Result;

            await WriteOutIdentityInformation();
        }

        private async Task WriteOutIdentityInformation() {

            var identityToken = await HttpContext.
                GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            Debug.WriteLine($"Identity token: {identityToken}");

            foreach (var claim in User.Claims) {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim Value: {claim.Value}");
            }

            //_IDPClient = _clientFactory.CreateClient("IDPClient");

            //var metadataResponse = await _IDPClient.GetDiscoveryDocumentAsync();
            //if (metadataResponse.IsError)
            //{
            //    throw new Exception(
            //        "Problem accessing the discovery endpoint",
            //        metadataResponse.Exception);
            //}

            ////metadataResponse.UserInfoEndpoint
            //var accessToken = await HttpContext
            //    .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);


            //var userInfoRespone = await _IDPClient.GetUserInfoAsync(new UserInfoRequest //Another extension method
            //{
            //    Address = metadataResponse.UserInfoEndpoint,
            //    Token = accessToken
            //});

            //if (userInfoRespone.IsError)
            //{
            //    throw new Exception(
            //        "Problem accessing te userinfo endpoint",
            //        userInfoRespone.Exception);
            //}

            //var address = userInfoRespone.Claims
            //    .FirstOrDefault(c => c.Type == "address")?.Value;

            //Debug.WriteLine($"Address {address}");

        }
    }
}