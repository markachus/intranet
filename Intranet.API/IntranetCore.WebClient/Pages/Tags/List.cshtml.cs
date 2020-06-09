using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntranetCore.Data.ResourceParameters;
using IntranetCore.WebClient.Clients;
using IntranetCore.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IntranetCore.WebClient.Pages.Tags
{
    public class ListModel : PageModel
    {
        private readonly TagsClient _client;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public IEnumerable<EtiquetaModel> Tags { get; set; }

        [BindProperty(SupportsGet = true)]
        public EtiquetasResourceParameters TagParams {get; set;}


        public ListModel(TagsClient client)
        {
            this._client = client;
        }

        public IEnumerable<EtiquetaModel> Etiquetas { get; set; }

        public void OnGet()
        {


            Tags = _client.GetTags(TagParams, _cancellationTokenSource.Token).Result;
        }
    }
}