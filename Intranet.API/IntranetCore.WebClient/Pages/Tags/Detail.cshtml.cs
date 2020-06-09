using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntranetCore.WebClient.Clients;
using IntranetCore.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IntranetCore.WebClient.Pages.Tags
{
    public class DetailModel : PageModel
    {
        private readonly TagsClient _client;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public DetailModel(TagsClient client)
        {
            this._client = client;
        }
        public EtiquetaModel Tag { get; set; }
        public async Task<IActionResult> OnGet(string tagName)
        {
            
            Tag = await _client.GetTag(tagName, cancellationTokenSource.Token);
            if (Tag == null) return RedirectToPage("./NotFound");
            return Page();
        }
    }
}