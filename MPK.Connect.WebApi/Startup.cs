using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Service.Business;
using MPK.Connect.Service.Business.Graph;

namespace MPK.Connect.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
                app.UseHsts();
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseReactDevelopmentServer(npmScript: "start");
            //    }
            //});
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();
            services.AddAutoMapper();

            services.AddTransient<IStopService, StopService>();
            services.AddTransient<ITimeTableService, TimeTableService>();
            services.AddTransient<IStopTimeService, StopTimeService>();
            services.AddTransient<ITravelPlanService, TravelPlanService>();
            services.AddTransient<IGraphBuilder, GraphBuilder>();
            services.AddTransient<IStopPathFinder, StopPathFinder>();
            services.AddTransient<IPathProvider, PathProvider>();
            services.AddTransient<ICoordinateLimitsProvider, CoordinateLimitsProvider>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(BaseRepository<>));

            services.AddSingleton(new LoggerFactory())
                .AddLogging(configure =>
                    configure
                        .AddConsole()
                        .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning))
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

            // Add dbContext
            services.AddDbContext<SimpleMpkContext>(options => options.UseSqlServer(Configuration.GetConnectionString(nameof(SimpleMpkContext))), ServiceLifetime.Scoped);

            services.AddScoped<IMpkContext, SimpleMpkContext>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }
    }
}