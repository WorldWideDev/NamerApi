using Microsoft.EntityFrameworkCore;

namespace NamesApi.Models
{
    public class NamesContext : DbContext
    {
        public NamesContext(DbContextOptions options) : base(options) {}
        public DbSet<NameEntry> Names {get;set;}
    }
}