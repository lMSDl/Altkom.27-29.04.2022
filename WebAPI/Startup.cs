using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using Services.Bogus;
using Services.Bogus.Fakes;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.ResponseCompression;

namespace WebAPI
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
            services.AddControllers()
                //.AddJsonOptions(x => x.JsonSerializerOptions);
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    x.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                    //x.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                    x.SerializerSettings.DateFormatString = "yy MMM+dd";
                    x.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                })
                .AddXmlSerializerFormatters();

            //jeœli nie okreslomy inaczej, to zastosowane zostanie br i gzip
            services.AddResponseCompression(x =>
            {
                x.Providers.Clear();
                x.Providers.Add<BrotliCompressionProvider>();
                x.Providers.Add<GzipCompressionProvider>();
                x.EnableForHttps = true; //je¿eli u¿ywamy https, nale¿y to w³¹czyæ (domyslnie wy³¹czone)
            });

            services.AddSingleton<IOrdersService, OrdersService>()
                    .AddSingleton<ICrudService<Order>>(x => x.GetService<IOrdersService>())
                    .AddSingleton<ICrudService<User>, CrudService<User>>()
                    .AddSingleton<ICrdParentService<Product>, ProductService>();
            services.AddTransient<OrderFaker>()
                .AddTransient<EntityFaker<User>, UserFaker>()
                .AddTransient<EntityFaker<Product>, ProductFaker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
