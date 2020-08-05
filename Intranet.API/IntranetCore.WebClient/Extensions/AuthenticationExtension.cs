using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetCore.WebClient.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAuthentication(options =>
            {
                //DefaultSchem could be any name but we choose a predifined name
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            //Enable Cooke-base authentcation for the default scheme
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                options.AccessDeniedPath = "/Authorization/AccessDenied";
            })
            //OpenIDConnect handler
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options => {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = "https://localhost:44318";
                options.ClientId = "intranetwebapp";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.UsePkce = true; //true by default
                                        //options.Scope.Add("openid"); //included by default, but added for clarity
                                        //options.Scope.Add("profile"); //included by default, but added for clarity
                            options.Scope.Add("roles");
                options.Scope.Add("address");
                options.SaveTokens = true; //save tokens so ewe can use them afterwards
                            options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.DeleteClaim("idp");
                options.ClaimActions.DeleteClaim("sid");
                options.ClaimActions.DeleteClaim("s_hash");
                options.ClaimActions.DeleteClaim("auth_time");
                            //options.ClaimActions.DeleteClaim("address");
                            options.ClaimActions.MapUniqueJsonKey("role", "role");

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = JwtClaimTypes.Role
                };
            });


            return services;
        }
    }
}
