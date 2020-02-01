using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NamesApi.Models
{
    public class NamesContext : DbContext
    {
        public NamesContext(DbContextOptions options) : base(options) {}
        public DbSet<NameEntry> Names {get;set;}
        
    }
}