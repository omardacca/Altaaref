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
    public class ViewNotebookStudent
    {
        public Notebook Notebook { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
    }

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
        [HttpGet("{IdentityId}/{NotebookId}")]
        public async Task<IActionResult> GetStudentFavNotebooks([FromRoute] string IdentityId, [FromRoute] int NotebookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentFavNotebooks = await _context.StudentFavNotebooks
                .SingleOrDefaultAsync(m => m.NotebookId == NotebookId && m.Student.IdentityId == IdentityId);

            if (studentFavNotebooks == null)
            {
                return NotFound();
            }

            return Ok(studentFavNotebooks);
        }

        // GET: api/StudentFavNotebooks/5/5
        [HttpGet("Details/{IdentityId}")]
        public async Task<IActionResult> GetStudentFavNotebookDetails([FromRoute] string IdentityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentFavNotebooks = _context.StudentFavNotebooks
                .Where(m => m.Student.IdentityId == IdentityId)
                .Select(fv => new ViewNotebookStudent { StudentId = fv.StudentId, StudentName = fv.Student.FullName, Notebook = fv.Notebook });

            if (studentFavNotebooks == null)
            {
                return NotFound();
            }

            return Ok(studentFavNotebooks);
        }

        // PUT: api/StudentFavNotebooks/5
        [HttpPut("{IdentityId}/{NotebookId}")]
        public async Task<IActionResult> PutStudentFavNotebooks([FromRoute] string IdentityId, [FromRoute] int NotebookId, [FromBody] StudentFavNotebooks studentFavNotebooks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (IdentityId != studentFavNotebooks.Student.IdentityId || NotebookId != studentFavNotebooks.NotebookId)
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
                if (!StudentFavNotebooksExists(IdentityId, NotebookId))
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
                if (StudentFavNotebooksExists(studentFavNotebooks.Student.IdentityId, studentFavNotebooks.NotebookId))
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
        [HttpDelete("{IdentityId}/{NotebookId}")]
        public async Task<IActionResult> DeleteStudentFavNotebooks([FromRoute] string IdentityId, [FromRoute] int NotebookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentFavNotebooks = await _context.StudentFavNotebooks
                .SingleOrDefaultAsync(m => m.NotebookId == NotebookId && m.Student.IdentityId == IdentityId);
            if (studentFavNotebooks == null)
            {
                return NotFound();
            }

            _context.StudentFavNotebooks.Remove(studentFavNotebooks);
            await _context.SaveChangesAsync();

            return Ok(studentFavNotebooks);
        }

        private bool StudentFavNotebooksExists(string IdentityId, int NotebookId)
        {
            return _context.StudentFavNotebooks.Any(e => e.NotebookId == NotebookId && e.Student.IdentityId == IdentityId);
        }
    }
}