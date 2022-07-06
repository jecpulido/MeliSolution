using AutoMapper;
using Meli.AccessLayer.ContextMongoDB;
using Meli.AccessLayer.Contract;
using Meli.AccessLayer.Repository;
using Meli.Common.Entities;
using Meli.ServiceLayer.Contract;
using Meli.ServiceLayer.Service;
using Meli.WepApi.MapperProfile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meli.WepApi
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
            services.Configure<MongoSettings>(
                options =>
                {
                    options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoDb:Database").Value;
                }
                );
            services.AddSingleton<MongoSettings>();

            services.AddTransient<IDnaContext, DnaContext>();
            //services.AddTransient<IDnaRepository, DnaRepository>();
            services.AddScoped(typeof(IIndividualRepository), typeof(DnaRepository));
            services.AddScoped(typeof(IIndividualService), typeof(IndividualService));

            services.AddControllers();
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsRule", rule =>
                {
                    rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Meli.WepApi", Version = "v1" });
            });

            //Mapper
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meli.WepApi v1"));
            //}

            app.UseRouting();

            app.UseCors("CorsRule");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
