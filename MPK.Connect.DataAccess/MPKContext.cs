﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess
{
    public class MpkContext : DbContext
    {
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<CalendarDate> CalendarDates { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<FeedInfo> FeedInfos { get; set; }
        public DbSet<FareRule> FareRules { get; set; }
        public DbSet<FareAttribute> FareAttributes { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<ShapePoint> ShapePoints { get; set; }
        public DbSet<Shape> Shapes { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<StopTime> StopTimes { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Trip> Trips { get; set; }

        public MpkContext(DbContextOptions<MpkContext> options) : base(options)
        {
        }

        public MpkContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShapePoint>()
                .HasKey(p => new { p.ShapeId, p.PointSequence });

            modelBuilder.Entity<Transfer>()
                .HasKey(p => new { p.FromStopId, p.ToStopId });

            modelBuilder.Entity<Transfer>()
                .HasOne(m => m.FromStop)
                .WithMany()
                .HasForeignKey(s => s.FromStopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                .HasOne(m => m.ToStop)
                .WithMany()
                .HasForeignKey(s => s.ToStopId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build().GetConnectionString("MpkContext");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}