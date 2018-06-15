using Microsoft.EntityFrameworkCore;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess
{
    public class MpkContext : DbContext
    {
        public DbSet<Stop> Stops { get; set; }

        public DbSet<StopType> StopTypes { get; set; }

        public MpkContext(DbContextOptions<MpkContext> options) : base(options)
        {
        }
    }
}