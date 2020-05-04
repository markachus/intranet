using Intranet.App.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.App.Services
{
    interface IAuthService
    {
        Task<TokenResponse> GetTokenAsync(string username, string password);
    }
}
