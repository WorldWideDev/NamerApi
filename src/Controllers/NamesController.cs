using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamesApi.Models;
using System.Linq;
using NamesApi.Filters;
namespace NamesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [InvalidModelStateFilter]
    public class NamesController : ControllerBase
    {
        private INamesRepository _repository;

        // TODO: get this from loggied in user
        public static string USER_LAST_NAME = "Newsom";

        public NamesController(INamesRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Names
        [HttpGet("{nameType?}")]
        public IEnumerable<NameEntry> GetNames([FromQuery]string nameType)
        {
            switch(nameType)
            {
                case "middle":
                    return _repository.Names.Where(n => n.Weight >= .5f);
                case "first":
                    return _repository.Names
                        .Where(n => n.Weight <= .5f)
                        .OrderBy(n => n.Name);
                default:
                    return _repository.Names;
            }
        }
        [HttpGet("random")]
        public string GetRandomName()
        {
            // TODO: randomly get a first name, but make first-name-weightedness more likely
            // FOR NOW: randomly grab first name w > 50% first-name-weightedness
            Random r = new Random();

            var firstNames = _repository.Names.Where(n => n.Weight != 1.0f).Select(n => n.Name).ToArray();
            string rFirstName = firstNames[r.Next(firstNames.Length)];

            var middleNames = _repository.Names.Where(n => n.Name != rFirstName).Where(n => n.Weight != 0f).Select(n => n.Name).ToArray();
            string rMiddle = middleNames[r.Next(middleNames.Length)];

            return $"{rFirstName} {rMiddle} {USER_LAST_NAME}";
        }

        // GET: api/Names/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NameEntry>> GetNameEntry(int id)
        {
            var nameEntry = await _repository.GetName(id);

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

            try
            {
                await _repository.UpdateName(nameEntry);
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _repository.NameEntryExists(id);
                if(!exists)
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
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchNameEntry(int id, [FromBody]JsonPatchDocument<NameEntry> patchDoc)
        {
            var entry = await _repository.GetName(id);
            if(entry == null)
            {
                return NotFound();
            }
            patchDoc.ApplyTo(entry, ModelState);
            bool isValid = TryValidateModel(entry);
            if(isValid)
            {
                await _repository.UpdateName(entry);
            }
            return Ok(entry);
        }
        // POST: api/Names
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<NameEntry>> PostNameEntry(NameEntry nameEntry)
        {
            await _repository.CreateName(nameEntry);

            return CreatedAtAction("GetNameEntry", new { id = nameEntry.Id }, nameEntry);
        }

        // DELETE: api/Names/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NameEntry>> DeleteNameEntry(int id)
        {
            var nameEntry = await _repository.GetName(id);
            if (nameEntry == null)
            {
                return NotFound();
            }

            await _repository.DeleteName(nameEntry);

            return nameEntry;
        }
        

    }
}
