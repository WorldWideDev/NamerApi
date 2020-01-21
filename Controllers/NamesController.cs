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
        private readonly NamesContext _context;

        public NamesController(NamesContext context)
        {
            _context = context;
        }

        // GET: api/Names
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NameEntry>>> GetNames()
        {
            return await _context.Names.ToListAsync();
        }

        // GET: api/Names/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NameEntry>> GetNameEntry(int id)
        {
            var nameEntry = await _context.Names.FindAsync(id);

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

            _context.Entry(nameEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            _context.Names.Add(nameEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNameEntry", new { id = nameEntry.Id }, nameEntry);
        }

        // DELETE: api/Names/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NameEntry>> DeleteNameEntry(int id)
        {
            var nameEntry = await _context.Names.FindAsync(id);
            if (nameEntry == null)
            {
                return NotFound();
            }

            _context.Names.Remove(nameEntry);
            await _context.SaveChangesAsync();

            return nameEntry;
        }

        private bool NameEntryExists(int id)
        {
            return _context.Names.Any(e => e.Id == id);
        }
    }
}
