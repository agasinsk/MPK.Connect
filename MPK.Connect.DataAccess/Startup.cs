using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MPK.Connect.DataAccess
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<MpkContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MpkContext")));
        }
    }
}