using System;
using MSA_Auth_API.Controllers;
using MSA_Auth_API.Extensions;
using MSA_Auth_API.ResponseModels;
using MSA_Auth_API.Repositories;
using MSA_Auth_API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
namespace MSA_Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services
                .AddAccountContext(Configuration.GetSection("DataSource:ConnectionString").Value)
                .AddScoped<IAccountRepository, IAccountRepository>()
                .AddScoped<IAccountService, AccountService>()
                .AddTokenAuthentication(Configuration)
                .AddResponseCaching()
                .AddOpenApiDocument(settings =>
                {
                    settings.Title = "Catalog API";
                    settings.DocumentName = "v3";
                    settings.Version = "v3";
                })
                .AddDistributedRedisCache(Configuration)
                .AddControllers()
                .AddValidation()
                .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddEventBus(Configuration);

            services.AddLinks(config =>
            {
                config.AddPolicy<ItemHateoasResponse>(policy =>
                {
                    policy
                        .RequireRoutedLink(nameof(ItemsHateoasController.Get), nameof(ItemsHateoasController.Get))
                        .RequireRoutedLink(nameof(ItemsHateoasController.GetById),
                            nameof(ItemsHateoasController.GetById), _ => new { id = _.Data.Id })
                        .RequireRoutedLink(nameof(ItemsHateoasController.Post), nameof(ItemsHateoasController.Post))
                        .RequireRoutedLink(nameof(ItemsHateoasController.Put), nameof(ItemsHateoasController.Put),
                            x => new { id = x.Data.Id })
                        .RequireRoutedLink(nameof(ItemsHateoasController.Delete), nameof(ItemsHateoasController.Delete),
                            x => new { id = x.Data.Id });
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MSA_Auth", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MSA_Auth v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
