using Microsoft.EntityFrameworkCore;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess
{
    public class MpkContext : DbContext
    {
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<ControlStop> ControlStops { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteType> RouteTypes { get; set; }
        public DbSet<Shape> Shapes { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<StopType> StopTypes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public MpkContext(DbContextOptions<MpkContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-N60GSEK\\SQLEXPRESS;Database=MPK.Connect;Trusted_Connection=True;");
        }
    }
}