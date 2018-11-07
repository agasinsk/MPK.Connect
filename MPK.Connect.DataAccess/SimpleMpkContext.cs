using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess
{
    public class SimpleMpkContext : DbContext, IMpkContext
    {
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<StopTime> StopTimes { get; set; }
        public DbSet<Trip> Trips { get; set; }

        public SimpleMpkContext(DbContextOptions<SimpleMpkContext> options) : base(options)
        {
        }

        public SimpleMpkContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StopTime>()
                .HasKey(p => new { p.TripId, p.StopId, p.StopSequence });

            modelBuilder.Ignore<Shape>();
            modelBuilder.Ignore<ShapeBase>();

            modelBuilder.Entity<Trip>()
                .Ignore(nameof(Trip.Shape));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build().GetConnectionString(nameof(SimpleMpkContext));
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}