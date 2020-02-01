using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NamesApi.Models;

namespace NamesApi.Services
{
    // TODO: Implement this service class using a true DDD model handling model needs for
    // NameEntry related queries.  
    // FOR NOW: Write quries on Business Entity right here in this dumb class.
    public class NamesService
    {
        private NamesContext _dbContext;
        public NamesService(NamesContext context)
        {
            _dbContext = context;
        }
        public async Task<List<NameEntry>> GetFirstNamesAsync()
        {
            var result = await _dbContext.Names.Where(n => n.Weight < .5f).OrderBy(n => n.Name).ToListAsync();
            return result;
        }
        public async Task<List<NameEntry>> GetMiddleNamesAsync()
        {
            var result = await _dbContext.Names.Where(n => n.Weight >= .5f).ToListAsync();
            return result;
        }
    }
}