using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AltaarefAPI;

namespace AltaarefAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CourseFacultiesController : ControllerBase
    {
        private readonly AltaarefContext _context;

        public CourseFacultiesController(AltaarefContext context)
        {
            _context = context;
        }

        // GET: api/CourseFaculties
        [HttpGet]
        public IEnumerable<CourseFaculty> GetCourseFaculty()
        {
            return _context.CourseFaculty;
        }

        // GET: api/CourseFaculties/{CourseId}/{FacultyId}
        [HttpGet("{CourseId}/{FacultyId}")]
        public async Task<IActionResult> GetCourseFaculty([FromRoute] int CourseId, [FromRoute] int FacultyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var courseFaculty = await _context.CourseFaculty
                .SingleOrDefaultAsync(m => m.CourseId == CourseId && m.FacultyId == FacultyId);

            if (courseFaculty == null)
            {
                return NotFound();
            }

            return Ok(courseFaculty);
        }

        // PUT: api/CourseFaculties/{CourseId}/{FacultyId}
        [HttpPut("{CourseId}/{FacultyId}")]
        public async Task<IActionResult> PutCourseFaculty([FromRoute] int FacultyId, [FromRoute] int CourseId, [FromBody] CourseFaculty courseFaculty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (FacultyId != courseFaculty.FacultyId)
            {
                return BadRequest();
            }

            _context.Entry(courseFaculty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseFacultyExists(FacultyId, CourseId))
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

        // POST: api/CourseFaculties
        [HttpPost]
        public async Task<IActionResult> PostCourseFaculty([FromBody] CourseFaculty courseFaculty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CourseFaculty.Add(courseFaculty);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseFacultyExists(courseFaculty.FacultyId, courseFaculty.CourseId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCourseFaculty", new { id = courseFaculty.CourseId }, courseFaculty);
        }

        // DELETE: api/CourseFaculties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseFaculty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var courseFaculty = await _context.CourseFaculty.SingleOrDefaultAsync(m => m.CourseId == id);
            if (courseFaculty == null)
            {
                return NotFound();
            }

            _context.CourseFaculty.Remove(courseFaculty);
            await _context.SaveChangesAsync();

            return Ok(courseFaculty);
        }

        private bool CourseFacultyExists(int FacultyId, int CourseId)
        {
            return _context.CourseFaculty.Any(e => e.CourseId == CourseId && e.FacultyId == FacultyId);
        }
    }
}