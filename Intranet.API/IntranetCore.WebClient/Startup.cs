using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntranetCore.WebClient.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IntranetCore.WebClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddHttpClient<TagsClient>();

            services.AddAuthentication(options =>
            {
                //DefaultSchem could be any name but we choose a predifined name
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                //Enable Cooke-base authentcation for the default scheme
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                //OpenIDConnect handler
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options => {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = "https://localhost:44318";
                    options.ClientId = "intranetwebapp";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.UsePkce = false; //true by default
                    options.Scope.Add("openid"); //included by default, but added for clarity
                    options.Scope.Add("profile"); //included by default, but added for clarity
                    options.SaveTokens = true; //save tokens so ewe can use them afterwards
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
