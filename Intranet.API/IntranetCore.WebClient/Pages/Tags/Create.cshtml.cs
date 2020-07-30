using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntranetCore.WebClient.Clients;
using IntranetCore.WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IntranetCore.WebClient.Pages.Tags
{
    public class CreateModel : PageModel
    {

        private readonly TagsClient _client;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public CreateModel(TagsClient client)
        {
            this._client = client;
        }
        [BindProperty()]
        public EtiquetaForCreationModel Tag { get; set; }

        public void OnGet()
        {
            //Tag = new EtiquetaForCreationModel();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                await _client.CreateTag(Tag, cancellationTokenSource.Token);
                if (string.IsNullOrEmpty(_client.Message))
                {
                    TempData["Message"] = "Tag created!";
                    return RedirectToPage("./Detail", new { tagName = Tag.Nombre });
                }
                else {
                    ModelState.AddModelError("TagExists", _client.Message);
                }
            }

            return Page();

        }
    }
}