using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamesApi.Models;

namespace NamesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NamesController : ControllerBase
    {
        private readonly NamesContext _dbContext;

        // TODO: get this from loggied in user
        public static string USER_LAST_NAME = "Newsom";

        public NamesController(NamesContext context)
        {
            _dbContext = context;
        }

        // GET: api/Names
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NameEntry>>> GetNames()
        {
            return await _dbContext.Names.ToListAsync();
        }

        [HttpGet("first")]
        public async Task<ActionResult<IEnumerable<NameEntry>>> GetFirstNames()
        {
            return await GetFirstNamesAsync();
        }
        [HttpGet("middle")]
        public async Task<ActionResult<IEnumerable<NameEntry>>> GetMiddleNames()
        {
            return await GetMiddleNamesAsync();
        }
        [HttpGet("random")]
        public async Task<ActionResult<string>> GetRandomName()
        {
            return await GetRandomNameAsync();
        }

        // GET: api/Names/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NameEntry>> GetNameEntry(int id)
        {
            var nameEntry = await _dbContext.Names.FindAsync(id);

            if (nameEntry == null)
            {
                return NotFound();
            }

            return nameEntry;
        }

        // PUT: api/Names/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNameEntry(int id, NameEntry nameEntry)
        {
            if (id != nameEntry.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(nameEntry).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NameEntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Names
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<NameEntry>> PostNameEntry(NameEntry nameEntry)
        {
            _dbContext.Names.Add(nameEntry);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetNameEntry", new { id = nameEntry.Id }, nameEntry);
        }

        // DELETE: api/Names/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NameEntry>> DeleteNameEntry(int id)
        {
            var nameEntry = await _dbContext.Names.FindAsync(id);
            if (nameEntry == null)
            {
                return NotFound();
            }

            _dbContext.Names.Remove(nameEntry);
            await _dbContext.SaveChangesAsync();

            return nameEntry;
        }
        // DB Queries:
        // TODO break queries into  DDD model, use this design: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/net-core-microservice-domain-model    
        
        private bool NameEntryExists(int id)
        {
            return _dbContext.Names.Any(e => e.Id == id);
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
        public async Task<string> GetRandomNameAsync()
        {
            // TODO: randomly get a first name, but make first-name-weightedness more likely
            // FOR NOW: randomly grab first name w > 50% first-name-weightedness
            Random r = new Random();

            var firstNames = await _dbContext.Names.Where(n => n.Weight != 1.0f).Select(n => n.Name).ToArrayAsync();
            string rFirstName = firstNames[r.Next(firstNames.Length)];

            var middleNames = await _dbContext.Names.Where(n => n.Name != rFirstName).Where(n => n.Weight != 0.0f).Select(n => n.Name).ToArrayAsync();
            string rMiddle = middleNames[r.Next(middleNames.Length)];

            return $"{rFirstName} {rMiddle} {USER_LAST_NAME}";
        }
    }
}
