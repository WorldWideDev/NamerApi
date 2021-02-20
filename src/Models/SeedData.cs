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
                    },
                    new NameEntry
                    {
                        Name = "Desmond", Weight = 0.2f
                    },
                    new NameEntry
                    {
                        Name = "Bruce", Weight = 0.7f
                    },
                    new NameEntry
                    {
                        Name = "Barney", Weight = 0.9f
                    },
                    new NameEntry
                    {
                        Name = "Yoshi", Weight = 0.2f
                    },
                    new NameEntry
                    {
                        Name = "Francis", Weight = 0.0f
                    },
                    new NameEntry
                    {
                        Name = "Ernest", Weight = 0.5f
                    },
                    new NameEntry
                    {
                        Name = "Julius", Weight = 1.0f
                    },
                    new NameEntry
                    {
                        Name = "Quintin", Weight = 0.2f
                    },
                    new NameEntry
                    {
                        Name = "Ryan", Weight = 0.7f
                    },
                    new NameEntry
                    {
                        Name = "Andrew", Weight = 0.9f
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
