using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AltaarefWebAPI.Contexts;
using AltaarefWebAPI.Models;

namespace AltaarefWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/HelpFaculties")]
    public class HelpFacultiesController : Controller
    {
        private readonly AltaarefDbContext _context;

        public HelpFacultiesController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/HelpFaculties
        [HttpGet]
        public IEnumerable<HelpFaculty> GetHelpFaculty()
        {
            return _context.HelpFaculty;
        }

        // GET: api/HelpFaculties/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHelpFaculty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpFaculty = await _context.HelpFaculty.SingleOrDefaultAsync(m => m.HelpRequestId == id);

            if (helpFaculty == null)
            {
                return NotFound();
            }

            return Ok(helpFaculty);
        }

        // GET: api/HelpFaculties/5
        [HttpGet("getbylist/{listOfFaculties}")]
        public async Task<IActionResult> c([FromRoute] int[] listOfFaculties)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<IQueryable<HelpRequest>> list = new List<IQueryable<HelpRequest>>();
            foreach(var item in listOfFaculties)
            {
                var x = _context.HelpFaculty.Where(h => h.FacultyId == item)
                    .Select(h => h.HelpRequest).Distinct();
                list.Add(x);
            }
            var distinclist = list.Distinct();

//            var helpFaculty = await _context.HelpFaculty.Where()

            if (distinclist == null)
            {
                return NotFound();
            }

            return Ok(distinclist);
        }

        // PUT: api/HelpFaculties/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHelpFaculty([FromRoute] int id, [FromBody] HelpFaculty helpFaculty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != helpFaculty.HelpRequestId)
            {
                return BadRequest();
            }

            _context.Entry(helpFaculty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HelpFacultyExists(id))
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

        // POST: api/HelpFaculties
        [HttpPost]
        public async Task<IActionResult> PostHelpFaculty([FromBody] IEnumerable<HelpFaculty> helpFacultiesList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.HelpFaculty.AddRange(helpFacultiesList);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return CreatedAtAction("GetHelpFaculty", helpFacultiesList);
        }

        // DELETE: api/HelpFaculties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHelpFaculty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpFaculty = await _context.HelpFaculty.SingleOrDefaultAsync(m => m.HelpRequestId == id);
            if (helpFaculty == null)
            {
                return NotFound();
            }

            _context.HelpFaculty.Remove(helpFaculty);
            await _context.SaveChangesAsync();

            return Ok(helpFaculty);
        }

        private bool HelpFacultyExists(int id)
        {
            return _context.HelpFaculty.Any(e => e.HelpRequestId == id);
        }
    }
}