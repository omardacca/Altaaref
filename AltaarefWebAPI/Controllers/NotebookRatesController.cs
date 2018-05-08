using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AltaarefWebAPI.Contexts;
using AltaarefWebAPI.Models;
using System.Linq;

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

        // GET: api/NotebookRates/5
        [HttpGet("TopRated/{StudentId}")]
        public async Task<IActionResult> GetNotebookTopRated([FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentCoursesId = _context.StudentCourses.Where(sc => sc.StudentId == StudentId).Select(sc => sc.CourseId).Distinct();

            var notebookRates = from p in _context.NotebookRates
                                where studentCoursesId.Contains(p.Notebook.CourseId)
                                group p by p.NotebookId into nt
                                select new { NotebookId = nt.Key, Sum = nt.Sum(n => n.Rate) };

            var topTenRated = notebookRates
                                .OrderBy(nr => nr.Sum)
                                .Take(10);

            if (topTenRated == null)
            {
                return NotFound();
            }

            return Ok(topTenRated);
        }

        // GET: api/NotebookRates/5
        [HttpGet("GetAllNotebookRates/{NotebookId}")]
        public async Task<IActionResult> GetNotebookAllRates([FromRoute] int NotebookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dictionary<byte, int> ratesDic = new Dictionary<byte, int>();

            var One = _context.NotebookRates.Where(m => m.NotebookId == NotebookId && m.Rate == 1).Count();
            var two = _context.NotebookRates.Where(m => m.NotebookId == NotebookId && m.Rate == 2).Count();
            var three = _context.NotebookRates.Where(m => m.NotebookId == NotebookId && m.Rate == 3).Count();
            var four = _context.NotebookRates.Where(m => m.NotebookId == NotebookId && m.Rate == 4).Count();
            var five = _context.NotebookRates.Where(m => m.NotebookId == NotebookId && m.Rate == 5).Count();

            ratesDic.Add(1, One);
            ratesDic.Add(2, two);
            ratesDic.Add(3, three);
            ratesDic.Add(4, four);
            ratesDic.Add(5, five);

            return Ok(ratesDic);
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