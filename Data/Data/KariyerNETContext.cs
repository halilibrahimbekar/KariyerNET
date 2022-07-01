using Microsoft.EntityFrameworkCore;
using Service.Entities;

namespace KariyerNET.Data
{
    public class KariyerNETContext : DbContext
    {
        public KariyerNETContext(DbContextOptions<KariyerNETContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}
