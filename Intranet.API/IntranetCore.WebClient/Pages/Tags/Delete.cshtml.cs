using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IntranetCore.WebClient.Clients;
using IntranetCore.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IntranetCore.WebClient.Pages.Tags
{
    public class DeleteModel : PageModel
    {
        private readonly TagsClient _client;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        [BindProperty(SupportsGet = true)]
        public EtiquetaModel Tag { get; set; }
        
        [TempData]
        public string Message { get; set; }

        public DeleteModel(TagsClient client)
        {
            this._client = client;
        }

        public async Task<IActionResult> OnGet(string tagName)
        {
            Tag = await _client.GetTag(tagName, _cancellationTokenSource.Token);
            if (Tag == null) return RedirectToPage("./NotFound");
            return Page();
        }

        public async Task<IActionResult> OnPost(string tagName)
        {
            try
            {
                await _client.DeleteTag(tagName, _cancellationTokenSource.Token);
            }
            catch (ApplicationException ex)
            {
                Tag = await _client.GetTag(tagName, _cancellationTokenSource.Token);
                TempData["Message"] = ex.Message;
                return Page();
            }

            return RedirectToPage("./List");

        }
    }
}