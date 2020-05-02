using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Unity.AspNet.WebApi;
using IdentityServer3.AccessTokenValidation;
using System.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(Intranet.API.Startup))]

namespace Intranet.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Para obtener más información sobre cómo configurar la aplicación, visite https://go.microsoft.com/fwlink/?LinkID=316888

            var config = new HttpConfiguration();

            WebApiConfig.Register(config);

            config.DependencyResolver = new UnityDependencyResolver(UnityConfig.Container);

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://markachusidsrv.azurewebsites.net/identity",
                RequiredScopes = new[] { "intranetapi"}
            });


            app.UseWebApi(config);

        }
    }
}
