using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WatchlistMonolith.Context;
using WatchlistMonolith.Services;

namespace WatchlistMonolith
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// something
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            
            #region Automapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles.MediaProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region MVC

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #endregion

            #region SQL

            //Use SQL Server
            var connectionString = Configuration["ConnectionStrings:MediasDemoDBConnectionString"];
            services.AddDbContext<MediasContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<IMediasRepository, MediasRepository>();
            services.AddScoped<IUsersRepository, UserRepository>();
            services.AddScoped<IWatchlistRepository, WatchlistRepository>();

            #endregion

            #region SWAGGER

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                        $"WatchlistOpenAPISpecification",
                        new Microsoft.OpenApi.Models.OpenApiInfo()
                        {
                            Title = "Watchlist API",
                            Version = "v1",
                            Description = "Through this API you can access watchlists, medias and users.",
                            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                            {
                                Email = "lcc.omar.aceves@gmail.com",
                                Name = "Omar Aceves",
                                Url = new Uri("https://github.com/omaraceves")
                            },
                            License = new Microsoft.OpenApi.Models.OpenApiLicense()
                            {
                                Name = "MIT License",
                                Url = new Uri("https://opensource.org/licenses/MIT")
                            }
                        });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();

            try
            {
                app.UseSwaggerUI(setupAction =>
                {
                    setupAction.SwaggerEndpoint(
                        "/swagger/WatchlistOpenAPISpecification/swagger.json",
                        "Watchlist API");

                    setupAction.RoutePrefix = "";

                    setupAction.DefaultModelExpandDepth(2);
                    setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                });
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
