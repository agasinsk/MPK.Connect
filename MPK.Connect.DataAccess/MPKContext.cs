using Microsoft.EntityFrameworkCore;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess
{
    public class MpkContext : DbContext
    {
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<CalendarDate> CalendarDates { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<FeedInfo> FeedInfos { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Shape> Shapes { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public MpkContext(DbContextOptions<MpkContext> options) : base(options)
        {
        }

        public MpkContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-N60GSEK\\SQLEXPRESS;Database=MPK.Connect;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Variant>()
                .HasOne(p => p.JoinStop)
                .WithMany(b => b.JoinVariants)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Variant>()
                .HasOne(p => p.DisjoinStop)
                .WithMany(b => b.DisjoinVariants)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}