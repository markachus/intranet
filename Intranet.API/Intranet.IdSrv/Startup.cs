using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Configuration;
using Intranet.IdSrv.Config;
using System.Security.Cryptography.X509Certificates;

[assembly: OwinStartup(typeof(Intranet.IdSrv.Startup))]
namespace Intranet.IdSrv
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) {

            app.Map("/identity", idSrvApp =>
            {
                idSrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Embedded IdentityServer",
                    IssuerUri = Constants.IsSrvIssuerUri,
                    Factory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryUsers(Users.Get())
                    .UseInMemoryScopes(Scopes.Get()),
                    SigningCertificate = LoadCertificate()
                });
            });
        }


        X509Certificate2 LoadCertificate() {
            return new X509Certificate2($"{AppDomain.CurrentDomain.BaseDirectory}/bin/idsrv3test.pfx", "idsrv3test");
        }
    }
}