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
    [Route("api/StudentCourses")]
    public class StudentCoursesController : Controller
    {
        private readonly AltaarefDbContext _context;

        public StudentCoursesController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentCourses
        [HttpGet]
        public IEnumerable<StudentCourses> GetStudentCourses()
        {
            return _context.StudentCourses;
        }

        // GET: api/StudentCourses/5
        [HttpGet("{CourseId}")]
        public IActionResult GetStudentsByCourse([FromRoute] int CourseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentCourses = _context.StudentCourses
                .Include(sc => sc.Student)
                .Where(m => m.CourseId == CourseId)
                .Select(sc => sc.Student)
                .ToList();

            if (studentCourses == null)
            {
                return NotFound();
            }

            return Ok(studentCourses);
        }

        // GET: api/StudentCourses/5
        [HttpGet("{StudentId}/{CourseId}")]
        public async Task<IActionResult> GetStudentCourses([FromRoute] int StudentId, [FromRoute] int CourseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentCourses = await _context.StudentCourses.SingleOrDefaultAsync(m => m.StudentId == StudentId && m.CourseId == CourseId);

            if (studentCourses == null)
            {
                return NotFound();
            }

            return Ok(studentCourses);
        }

        // PUT: api/StudentCourses/5
        [HttpPut("{StudentId}/{CourseId}")]
        public async Task<IActionResult> PutStudentCourses([FromRoute] int StudentId, [FromRoute] int CourseId, [FromBody] StudentCourses studentCourses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (StudentId != studentCourses.StudentId || CourseId != studentCourses.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(studentCourses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCoursesExists(StudentId, CourseId))
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

        // POST: api/StudentCourses
        [HttpPost]
        public async Task<IActionResult> PostStudentCourses([FromBody] StudentCourses studentCourses)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudentCourses.Add(studentCourses);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentCoursesExists(studentCourses.StudentId ,studentCourses.CourseId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentCourses", new { studentCourses.CourseId, studentCourses.StudentId }, studentCourses);
        }

        // DELETE: api/StudentCourses/5
        [HttpDelete("{StudentId}/{CourseId}")]
        public async Task<IActionResult> DeleteStudentCourses([FromRoute] int StudentId, [FromRoute] int CourseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentCourses = await _context.StudentCourses.SingleOrDefaultAsync(m => m.CourseId == CourseId && m.StudentId == StudentId);
            if (studentCourses == null)
            {
                return NotFound();
            }

            _context.StudentCourses.Remove(studentCourses);
            await _context.SaveChangesAsync();

            return Ok(studentCourses);
        }

        private bool StudentCoursesExists(int StudentId, int CourseId)
        {
            return _context.StudentCourses.Any(e => e.CourseId == CourseId || e.StudentId == StudentId);
        }
    }
}