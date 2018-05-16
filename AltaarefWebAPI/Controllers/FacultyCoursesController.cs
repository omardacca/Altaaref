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
    [Route("api/FacultyCourses")]
    public class FacultyCoursesController : Controller
    {
        private readonly AltaarefDbContext _context;

        public FacultyCoursesController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/FacultyCourses
        [HttpGet]
        public IEnumerable<FacultyCourse> GetFacultyCourse()
        {
            return _context.FacultyCourse;
        }

        // GET: api/FacultyCourses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacultyCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var facultyCourse = await _context.FacultyCourse.SingleOrDefaultAsync(m => m.CourseId == id);

            if (facultyCourse == null)
            {
                return NotFound();
            }

            return Ok(facultyCourse);
        }

        // GET: api/FacultyCourses/5
        [HttpGet("GetCoursesByFacultiesId/{facultyId}")]
        public async Task<IActionResult> GetCoursesByFacultiesId([FromRoute] int facultyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var crs = _context.FacultyCourse.Where(fc => fc.FacultyId == facultyId).Select(fc => fc.Course).ToList();

            if (crs == null)
            {
                return NotFound();
            }

            return Ok(crs);
        }

        // GET: api/FacultyCourses/5
        [HttpGet("Courses/{FacultyId}")]
        public IActionResult GetCoursesByFacultyId([FromRoute] int FacultyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var facultyCourse = _context.FacultyCourse
                .Include(c => c.Course)
                .Where(fc => fc.FacultyId == FacultyId)
                .Select(fc => fc.Course)
                .ToList();


            if (facultyCourse == null)
            {
                return NotFound();
            }

            return Ok(facultyCourse);
        }

        // PUT: api/FacultyCourses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacultyCourse([FromRoute] int id, [FromBody] FacultyCourse facultyCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != facultyCourse.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(facultyCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacultyCourseExists(id))
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

        // POST: api/FacultyCourses
        [HttpPost]
        public async Task<IActionResult> PostFacultyCourse([FromBody] FacultyCourse facultyCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FacultyCourse.Add(facultyCourse);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FacultyCourseExists(facultyCourse.CourseId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFacultyCourse", new { id = facultyCourse.CourseId }, facultyCourse);
        }

        // DELETE: api/FacultyCourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacultyCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var facultyCourse = await _context.FacultyCourse.SingleOrDefaultAsync(m => m.CourseId == id);
            if (facultyCourse == null)
            {
                return NotFound();
            }

            _context.FacultyCourse.Remove(facultyCourse);
            await _context.SaveChangesAsync();

            return Ok(facultyCourse);
        }

        private bool FacultyCourseExists(int id)
        {
            return _context.FacultyCourse.Any(e => e.CourseId == id);
        }
    }
}