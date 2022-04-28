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
using FluentValidation.AspNetCore;
using FluentValidation;
using Models.Validators;
using WebAPI.FIlters;

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
                .AddXmlSerializerFormatters()
                //dodanie walidacji do kontroler�w
                .AddFluentValidation(x =>
                    //automatyczne zarejestrowanie walidator�w, kt�re s� w tym samym assembly co wskazany
                    x.RegisterValidatorsFromAssemblyContaining<OrderValidator>()
                );

            //r�czne dodtawanie walidator�w
            //services.AddTransient<IValidator<Order>, OrderValidator>();
            //services.AddTransient<IValidator<Product>, ProductValidator>();

            //je�li nie okreslomy inaczej, to zastosowane zostanie br i gzip
            services.AddResponseCompression(x =>
            {
                x.Providers.Clear();
                x.Providers.Add<BrotliCompressionProvider>();
                x.Providers.Add<GzipCompressionProvider>();
                x.EnableForHttps = true; //je�eli u�ywamy https, nale�y to w��czy� (domyslnie wy��czone)
            });

            services.AddSingleton<IOrdersService, OrdersService>()
                    .AddSingleton<ICrudService<Order>>(x => x.GetService<IOrdersService>())
                    .AddSingleton<ICrudService<User>, CrudService<User>>()
                    .AddSingleton<ICrdParentService<Product>, ProductService>();
            services.AddTransient<OrderFaker>()
                .AddTransient<EntityFaker<User>, UserFaker>()
                .AddTransient<EntityFaker<Product>, ProductFaker>();

            //domy�lnia walidacja modelu jest w��czona - w poni�szy spos�b mo�na j� wy��czy�
            //services.Configure<ApiBehaviorOptions>(x => x.SuppressModelStateInvalidFilter = true);

            services.AddScoped<ConsoleLogFilter>();
            services.AddSingleton(x => new LimiterFilter(5));
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
