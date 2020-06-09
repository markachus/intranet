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
    public class EditModel : PageModel
    {
        private readonly TagsClient _client;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public EditModel(TagsClient client)
        {
            this._client = client;
        }

        [BindProperty()]
        public EtiquetaModel Tag { get; set; }

        public async Task<IActionResult> OnGet(string tagName)
        {
            Tag = await _client.GetTag(tagName, cancellationTokenSource.Token);
            if (Tag == null) {
                Tag = new EtiquetaModel();
            }
            if (Tag == null) return RedirectToPage("./NotFound");
            return Page();
        }


        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                await _client.PartialUpdate(Tag, cancellationTokenSource.Token);
                TempData["Message"] = "Tag saved!";
                return RedirectToPage("./Detail", new { tagName = Tag.Nombre});
            }

            return Page();

        }

    }
}