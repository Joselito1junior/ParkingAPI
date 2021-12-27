using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingAPI.V1.Repositories;
using ParkingAPI.V1.Repositories.Contracts;
using AutoMapper;
using ParkingAPI.Helpers;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace ParkingAPI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //AutoMapper Config
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDbContext<ParkingSpaceContext>(opt => 
            {
                opt.UseSqlite("Data Source=Data\\ParkingSpace.db");
            });

            services.AddScoped<IParkingSpaceRepository, ParkingSpaceRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddApiVersioning(cfg =>
            {
                cfg.ReportApiVersions = true;
                //cfg.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Title = "ParkingAPI - v1,",
                    Version = "v1"
                });

                var ProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
                var ProjectName = $"{PlatformServices.Default.Application.ApplicationName}.xml";
                var CommentsPathArchiveXML = Path.Combine(ProjectPath, ProjectName);
                cfg.IncludeXmlComments(CommentsPathArchiveXML);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(cfg => 
            {
                cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkingAPI");
                cfg.RoutePrefix = "";
            });
        }
    }
}
