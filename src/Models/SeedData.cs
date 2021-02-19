using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace NamesApi.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            NamesDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<NamesDbContext>();
            
            if(context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if(!context.Names.Any())
            {
                context.Names.AddRange
                (
                    new NameEntry
                    {
                        Name = "Dax", Weight = 0.2f
                    },
                    new NameEntry
                    {
                        Name = "Devon", Weight = 0.0f
                    },
                    new NameEntry
                    {
                        Name = "John", Weight = 0.5f
                    },
                    new NameEntry
                    {
                        Name = "MacLeod", Weight = 1.0f
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
