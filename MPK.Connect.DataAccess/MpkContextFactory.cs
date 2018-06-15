using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MPK.Connect.DataAccess
{
    public class MpkContextFactory : IDesignTimeDbContextFactory<MpkContext>
    {
        public MpkContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MpkContext>();
            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-N60GSEK\\SQLEXPRESS;Database=MPK.Core;Trusted_Connection=True;");

            return new MpkContext(optionsBuilder.Options);
        }
    }
}