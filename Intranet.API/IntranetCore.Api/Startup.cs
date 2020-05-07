using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using IntranetCore.Data.Helpers;
using IntranetCore.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            services.AddControllers(options => {
                options.ReturnHttpNotAcceptable = true;
            })
                .AddNewtonsoftJson(setup => {
                    setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddXmlDataContractSerializerFormatters()
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
                        problemDetails.Type = "http://localhost:51044/swagger";
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
                options.UseSqlServer(Configuration["IntranetDbConnectionString"]);
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

            services.AddSwaggerGen(swaggerOptions =>
            {
                swaggerOptions.SwaggerDoc(
                    "OpenApiSpecification", 
                    new OpenApiInfo { 
                        Version = "1.0",
                        Title = "Documentación de la Intranet API",
                        Description = "Está documentación se pone a servicio de aquellos departamentos que " +
                        "desean consumir la API Intranet desde una Mobile App o desde un App Web",
                        Contact = new OpenApiContact {  
                            Email = "markachus@gmail.com", 
                            Name = "Marc Esteve" 
                        }
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseSwagger();

            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint(
                    "/swagger/OpenApiSpecification/swagger.json",
                    "Intranet API");
                options.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
