using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using IntranetCore.API.Authentication;
using IntranetCore.Data.Helpers;
using IntranetCore.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace IntranetCore.Api
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

            services.AddHttpCacheHeaders(options => {
                options.MaxAge = 60;
                options.CacheLocation = Marvin.Cache.Headers.CacheLocation.Public;
            },
            validationmodeloptions => {
                validationmodeloptions.MustRevalidate = true;
            });

            services.AddResponseCaching();

            services.AddControllers(options => {

                //options.CacheProfiles.Add(new KeyValuePair<string, CacheProfile>(
                //                            "2minutoscacheprofile",
                //                            new CacheProfile { Duration = 120 }));

                options.ReturnHttpNotAcceptable = true;
                
                //Append because we want application/json to be the first outputformatter, and therefore, the default
                options.OutputFormatters.Append(new XmlSerializerOutputFormatter());


                options.Filters.Add(
                    new ProducesResponseTypeAttribute(
                        Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized));

                options.Filters.Add(
                    new ProducesResponseTypeAttribute(
                        Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest));

                options.Filters.Add(
                    new ProducesResponseTypeAttribute(
                        Microsoft.AspNetCore.Http.StatusCodes.Status406NotAcceptable));

                options.Filters.Add(
                    new ProducesResponseTypeAttribute(
                        Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError));

                //options.Filters.Add(new AuthorizeFilter());

            }).AddNewtonsoftJson(setup => {
                    setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                //.AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(options => {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var problemsDetailFactory = context.HttpContext.RequestServices
                    .GetService<ProblemDetailsFactory>();

                    var problemDetails = problemsDetailFactory.
                    CreateValidationProblemDetails(context.HttpContext, context.ModelState);
                    
                    problemDetails.Instance = context.HttpContext.Request.Path;

                    var actionExceutingContext = context as 
                        Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    //there are modelstate errors and the number of parameters passed
                    // macth the number of parameters parsed/found in the model binding

                    if (context.ModelState.ErrorCount > 0 && 
                            context.ActionDescriptor.Parameters.Count == 
                            actionExceutingContext?.ActionArguments.Count)
                    {
                        problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                        problemDetails.Type = "http://localhost:51044/index.html";
                        problemDetails.Title = "One or more validation errors ocurred";

                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    }

                    // One of the arguments wasn't found correctly o couldn't be parsed
                    // we return 400 bad request

                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "One or errors on input ocurred";

                    return new BadRequestObjectResult(problemDetails) {
                        ContentTypes = { "application/problem+json" }
                    };

                };
            });


            services.AddDbContext<IntranetCore.Data.IntranetDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IntranetDbConnectionString"));
            });

            services.AddScoped(typeof(IEtiquetaRepository), typeof(EtiquetaRepository));
            services.AddScoped(typeof(IEntradaRepository), typeof(EntradaRepository));
            services.AddScoped<IPropertyMappingService, PropertyMappingService>();

            //AutoMapper injection
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new IntranetMappingProfile());
            });

            services.AddSingleton<IMapper>(config.CreateMapper());

            // Authentication scheme: basic, oauth, apiKey, etc
            //services.AddAuthentication("Basic").
            //    AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);


            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VV";
            });

            services.AddApiVersioning(options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;

                //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            });


            var apiversionprovider = services.BuildServiceProvider().
                GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(swaggerOptions =>
            {

                foreach (var description in apiversionprovider.ApiVersionDescriptions)
                {
                    swaggerOptions.SwaggerDoc(
                    $"OpenApiSpecification{description.GroupName}",
                    new OpenApiInfo
                    {
                        Version = description.ApiVersion.ToString(),
                        Title = "Documentación de la Intranet API",
                        Description = "Está documentación se pone a servicio de aquellos departamentos que " +
                        "desean consumir la API Intranet desde una Mobile App o desde un App Web",
                        Contact = new OpenApiContact
                        {
                            Email = "markachus@gmail.com",
                            Name = "Marc Esteve"
                        }
                    });
                }

                //swaggerOptions.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme() { 
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "basic",
                //    Description= "Input user name and password to access de API"
                //});


                //swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    { 
                //        new OpenApiSecurityScheme { 
                //            Reference = new OpenApiReference { 
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "basicAuth"
                //            }
                //        }, new List<string>()
                //    }
                //});

                swaggerOptions.DocInclusionPredicate((documentName, apiDescription) =>
                {
                    var actionApiVersionModel = apiDescription.ActionDescriptor
                    .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }

                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v =>
                        $"OpenApiSpecificationv{v.ToString()}" == documentName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v =>
                        $"OpenApiSpecificationv{v.ToString()}" == documentName);
                });

                //Include comments in this library
                var xmlCommentsFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                swaggerOptions.IncludeXmlComments(xmlCommentsFullPath);


                var xmlSchemaCommentsFullPath = Path.Combine(AppContext.BaseDirectory, "IntranetCore.Data.xml");
                swaggerOptions.IncludeXmlComments(xmlSchemaCommentsFullPath);

            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                
                    foreach (var descr in apiVersionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/OpenApiSpecification{descr.GroupName}/swagger.json",
                            descr.GroupName.ToUpperInvariant());
                    }

                    options.RoutePrefix = "";
                });


            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
