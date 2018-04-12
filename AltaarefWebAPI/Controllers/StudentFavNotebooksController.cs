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
    [Route("api/StudentFavNotebooks")]
    public class StudentFavNotebooksController : Controller
    {
        private readonly AltaarefDbContext _context;

        public StudentFavNotebooksController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentFavNotebooks
        [HttpGet]
        public IEnumerable<StudentFavNotebooks> GetStudentFavNotebooks()
        {
            return _context.StudentFavNotebooks;
        }

        // GET: api/StudentFavNotebooks/5/5
        [HttpGet("{StudentId}/{NotebookId}")]
        public async Task<IActionResult> GetStudentFavNotebooks([FromRoute] int StudentId, [FromRoute] int NotebookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentFavNotebooks = await _context.StudentFavNotebooks
                .SingleOrDefaultAsync(m => m.NotebookId == NotebookId && m.StudentId == StudentId);

            if (studentFavNotebooks == null)
            {
                return NotFound();
            }

            return Ok(studentFavNotebooks);
        }

        // GET: api/StudentFavNotebooks/5/5
        [HttpGet("Details/{StudentId}/{NotebookId}")]
        public async Task<IActionResult> GetStudentFavNotebookDetails([FromRoute] int StudentId, [FromRoute] int NotebookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentFavNotebooks = _context.StudentFavNotebooks
                .Where(m => m.NotebookId == NotebookId && m.StudentId == StudentId)
                .Select(fv => fv.Notebook);

            if (studentFavNotebooks == null)
            {
                return NotFound();
            }

            return Ok(studentFavNotebooks);
        }

        // PUT: api/StudentFavNotebooks/5
        [HttpPut("{StudentId}/{NotebookId}")]
        public async Task<IActionResult> PutStudentFavNotebooks([FromRoute] int StudentId, [FromRoute] int NotebookId, [FromBody] StudentFavNotebooks studentFavNotebooks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (StudentId != studentFavNotebooks.StudentId || NotebookId != studentFavNotebooks.NotebookId)
            {
                return BadRequest();
            }

            _context.Entry(studentFavNotebooks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentFavNotebooksExists(StudentId, NotebookId))
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

        // POST: api/StudentFavNotebooks
        [HttpPost]
        public async Task<IActionResult> PostStudentFavNotebooks([FromBody] StudentFavNotebooks studentFavNotebooks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudentFavNotebooks.Add(studentFavNotebooks);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentFavNotebooksExists(studentFavNotebooks.NotebookId, studentFavNotebooks.StudentId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentFavNotebooks",
                new { NotebookId = studentFavNotebooks.NotebookId, StudentId = studentFavNotebooks.StudentId }, studentFavNotebooks);
        }

        // DELETE: api/StudentFavNotebooks/5
        [HttpDelete("{StudentId}/{NotebookId}")]
        public async Task<IActionResult> DeleteStudentFavNotebooks([FromRoute] int StudentId, [FromRoute] int NotebookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentFavNotebooks = await _context.StudentFavNotebooks
                .SingleOrDefaultAsync(m => m.NotebookId == NotebookId && m.StudentId == StudentId);
            if (studentFavNotebooks == null)
            {
                return NotFound();
            }

            _context.StudentFavNotebooks.Remove(studentFavNotebooks);
            await _context.SaveChangesAsync();

            return Ok(studentFavNotebooks);
        }

        private bool StudentFavNotebooksExists(int StudentId, int NotebookId)
        {
            return _context.StudentFavNotebooks.Any(e => e.NotebookId == NotebookId && e.StudentId == StudentId);
        }
    }
}