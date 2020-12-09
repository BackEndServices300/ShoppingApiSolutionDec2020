using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingApi.Data;
using ShoppingApi.Hubs;
using ShoppingApi.Profiles;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoppingApi
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:4200");
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                });
            });
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.AddScoped<ILookupProducts, EfSqlProducts>();
            services.AddScoped<IProductCommands, EfSqlProducts>();
            services.AddDbContext<ShoppingDataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("shopping"));
            });

            var mapperConfiguration = new MapperConfiguration(opt =>
            {
                opt.AddProfile(new ProductsProfile());
                
            });

            var mapper = mapperConfiguration.CreateMapper();

            services.AddSingleton<IMapper>(mapper);
            services.AddSingleton<MapperConfiguration>(mapperConfiguration);

            //// this makes it so we just have those settings availble... demo coming.
            var configForPricing = new ConfigurationForPricing();
            //Configuration.GetSection(configForPricing.SectionName).Bind(configForPricing);

            // this makes it so we can get this through IOptions<T>... 
            services.Configure<ConfigurationForPricing>(
                Configuration.GetSection(configForPricing.SectionName));

            services.AddSingleton<CurbsideChannel>();
            services.AddHostedService<CurbsideOrderProcessor>();
            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CurbsideHub>("/curbsidehub");
            });
        }
    }
}
