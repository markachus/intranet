using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
