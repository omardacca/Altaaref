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
    [Route("api/NotebookRates")]
    public class NotebookRatesController : Controller
    {
        private readonly AltaarefDbContext _context;

        public NotebookRatesController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/NotebookRates
        [HttpGet]
        public IEnumerable<NotebookRates> GetNotebookRates()
        {
            return _context.NotebookRates;
        }

        // GET: api/NotebookRates/5
        [HttpGet("{NotebookId}/{StudentId}")]
        public async Task<IActionResult> GetNotebookRates([FromRoute] int NotebookId, [FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebookRates = await _context.NotebookRates.SingleOrDefaultAsync(m => m.NotebookId == NotebookId && m.StudentId == StudentId);

            if (notebookRates == null)
            {
                return NotFound();
            }

            return Ok(notebookRates);
        }

        // PUT: api/NotebookRates/5
        [HttpPut("{NotebookId}/{StudentId}")]
        public async Task<IActionResult> PutNotebookRates([FromRoute] int NotebookId, [FromRoute] int StudentId, [FromBody] NotebookRates notebookRates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (NotebookId != notebookRates.NotebookId || StudentId != notebookRates.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(notebookRates).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotebookRatesExists(NotebookId, StudentId))
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

        // POST: api/NotebookRates
        [HttpPost]
        public async Task<IActionResult> PostNotebookRates([FromBody] NotebookRates notebookRates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.NotebookRates.Add(notebookRates);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NotebookRatesExists(notebookRates.NotebookId, notebookRates.StudentId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNotebookRates", new { id = notebookRates.NotebookId }, notebookRates);
        }

        // DELETE: api/NotebookRates/5
        [HttpDelete("{NotebookId}/{StudentId}")]
        public async Task<IActionResult> DeleteNotebookRates([FromRoute] int NotebookId, [FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebookRates = await _context.NotebookRates.SingleOrDefaultAsync(m => m.NotebookId == NotebookId && m.StudentId == StudentId);
            if (notebookRates == null)
            {
                return NotFound();
            }

            _context.NotebookRates.Remove(notebookRates);
            await _context.SaveChangesAsync();

            return Ok(notebookRates);
        }

        private bool NotebookRatesExists(int studentid, int notebookid)
        {
            return _context.NotebookRates.Any(e => e.NotebookId == studentid && e.StudentId == notebookid);
        }
    }
}