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
    [Route("api/Notebooks")]
    public class NotebooksController : Controller
    {
        private readonly AltaarefDbContext _context;

        public NotebooksController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/Notebooks
        [HttpGet]
        public IEnumerable<Notebook> GetNotebook()
        {
            return _context.Notebook;
        }

        // GET: api/Notebooks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotebook([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebook = await _context.Notebook.SingleOrDefaultAsync(m => m.Id == id);

            if (notebook == null)
            {
                return NotFound();
            }

            return Ok(notebook);
        }

        // GET: api/Notebooks/5
        [HttpGet("Search/ByNotebookName/{notebooktext}")]
        public async Task<IActionResult> GetSearchByName([FromRoute] string notebooktext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebook = _context.Notebook.Where(nb => nb.Name.Contains(notebooktext))
                                            .Select(nb => new ViewNotebookStudent { StudentId = nb.StudentId, StudentName = nb.Student.FullName, Notebook = nb });

            if (notebook == null)
            {
                return NotFound();
            }

            return Ok(notebook);
        }

        // GET: api/Notebooks/5
        [HttpGet("Search/ByCourseId/{CourseId}")]
        public async Task<IActionResult> GetSearchCourseId([FromRoute] int CourseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebook = _context.Notebook.Where(nb => nb.CourseId == CourseId)
                                            .Select(nb => new ViewNotebookStudent { StudentId = nb.StudentId, StudentName = nb.Student.FullName, Notebook = nb });

            if (notebook == null)
            {
                return NotFound();
            }

            return Ok(notebook);
        }

        // GET: api/Notebooks/5
        [HttpGet("Search/ByCourseIdAndName/{CourseId}/{notebooktext}")]
        public async Task<IActionResult> GetSearchCourseId([FromRoute] int CourseId, [FromRoute] string notebooktext)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebook = _context.Notebook.Where(nb => nb.CourseId == CourseId && nb.Name.Contains(notebooktext))
                                            .Select(nb => new ViewNotebookStudent { StudentId = nb.StudentId, StudentName = nb.Student.FullName, Notebook = nb });

            if (notebook == null)
            {
                return NotFound();
            }

            return Ok(notebook);
        }

        // GET: api/Notebooks/5
        [HttpGet("Recent/{StudentId}")]
        public async Task<IActionResult> GetRecentNotebook([FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentCoursesId = _context.StudentCourses.Where(sc => sc.StudentId == StudentId).Select(sc => sc.CourseId).Distinct();

            var notebook = _context.Notebook.Where(nb => studentCoursesId.Contains(nb.CourseId) &&
                                            nb.PublishDate.AddDays(14) >= DateTime.Today)
                                            .Select(nb => new ViewNotebookStudent { StudentId = nb.StudentId, StudentName = nb.Student.FullName, Notebook = nb });

            if (notebook == null)
            {
                return NotFound();
            }

            return Ok(notebook);
        }


        // GET: api/Notebooks/5
        [HttpGet("StudentFavoriteNumber/{id}")]
        public async Task<IActionResult> GetStudentFavoriteNotebooksNumber([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebook = _context.Notebook.Where(m => m.Id == id);
            var count = notebook.Select(m => m.StudentFavNotebooks.Count).Single();

            if (notebook == null)
            {
                return NotFound();
            }

            return Ok(count);
        }

        // GET: api/Notebooks/5
        [HttpGet("GetStudentNotebooks/{IdentityId}")]
        public async Task<IActionResult> GetStudentNotebooks([FromRoute] string IdentityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebook = _context.Notebook.Where(m => m.Student.IdentityId == IdentityId)
                .Select(fv => new ViewNotebookStudent { StudentId = fv.StudentId, StudentName = fv.Student.FullName, Notebook = fv});

            if (notebook == null)
            {
                return NotFound();
            }

            return Ok(notebook);
        }

        // GET: api/Notebooks/5
        [HttpGet("GetStudent/{id}")]
        public async Task<IActionResult> GetStudentByNotebookId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebook = _context.Notebook.Where(m => m.Id == id)
                .Select(n => new { StudentId = n.StudentId, StudentName = n.Student.FullName });

            if (notebook == null)
            {
                return NotFound();
            }

            return Ok(notebook);
        }

        // GET: api/Notebooks/5
        [HttpGet("Course/{courseId}")]
        public IActionResult GetNotebooksByCourseId([FromRoute] int courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebook = _context.Notebook.Where(c => c.CourseId == courseId).ToList();

            if (notebook == null)
            {
                return NotFound();
            }

            return Ok(notebook);
        }


        // PUT: api/Notebooks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotebook([FromRoute] int id, [FromBody] Notebook notebook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notebook.Id)
            {
                return BadRequest();
            }

            _context.Entry(notebook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotebookExists(id))
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

        // POST: api/Notebooks
        [HttpPost]
        public async Task<IActionResult> PostNotebook([FromBody] Notebook notebook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Notebook.Add(notebook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotebook", new { id = notebook.Id }, notebook);
        }

        // DELETE: api/Notebooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotebook([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notebook = await _context.Notebook.SingleOrDefaultAsync(m => m.Id == id);
            if (notebook == null)
            {
                return NotFound();
            }

            _context.Notebook.Remove(notebook);
            await _context.SaveChangesAsync();

            return Ok(notebook);
        }

        private bool NotebookExists(int id)
        {
            return _context.Notebook.Any(e => e.Id == id);
        }
    }
}