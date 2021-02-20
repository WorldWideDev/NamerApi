using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
namespace NamesApi.Models
{
    public class EFNamesRepository : INamesRepository
    {
        private NamesDbContext context;
        public EFNamesRepository(NamesDbContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<NameEntry> Names => context.Names;
        public async Task<bool> NameEntryExists(int id)
        {
            NameEntry n = await context.Names.FindAsync(id);
            return n != null;
        }
        public bool NameEntryExists(string name)
        {
            return context.Names.Any(n => n.Name == name);
        }
        public IEnumerable<NameEntry> GetMiddleNames()
        {
            return Names.Where(n => n.Weight >= .5f);
        }
        public async Task<NameEntry> GetName(int id) => await context.Names.FindAsync(id);
        public async Task<NameEntry> CreateName(NameEntry p)
        {
            await context.AddAsync(p);
            await context.SaveChangesAsync();
            return p;
        }
        public async Task DeleteName(NameEntry p)
        {
            context.Remove(p);
            await context.SaveChangesAsync();
        }
        public async Task<NameEntry> UpdateName(NameEntry p)
        {
            p.CreatedAt = DateTime.Now;
            context.Names.Update(p);
            await context.SaveChangesAsync();
            return p;
        }
        public async Task<NameEntry> PatchName(int id, JsonPatchDocument<NameEntry> patchDoc)
        {
            var entry = await context.Names.FindAsync(id);
            if(entry != null)
            {
                patchDoc.ApplyTo(entry);
                context.Entry(entry).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            return entry;
        }
    }
}
