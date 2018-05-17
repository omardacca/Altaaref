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
    [Route("api/StudentFaculties")]
    public class StudentFacultiesController : Controller
    {
        private readonly AltaarefDbContext _context;

        public StudentFacultiesController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentFaculties
        [HttpGet]
        public IEnumerable<StudentFaculty> GetStudentFaculties()
        {
            return _context.StudentFaculties;
        }

        // GET: api/StudentFaculties/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentFaculty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentFaculty = await _context.StudentFaculties.SingleOrDefaultAsync(m => m.FacultyId == id);

            if (studentFaculty == null)
            {
                return NotFound();
            }

            return Ok(studentFaculty);
        }

        // PUT: api/StudentFaculties/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentFaculty([FromRoute] int id, [FromBody] StudentFaculty studentFaculty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentFaculty.FacultyId)
            {
                return BadRequest();
            }

            _context.Entry(studentFaculty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentFacultyExists(id))
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

        // POST: api/StudentFaculties
        [HttpPost]
        public async Task<IActionResult> PostStudentFaculty([FromBody] List<StudentFaculty> studentFacultyList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudentFaculties.AddRange(studentFacultyList);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                foreach(var studentFaculty in studentFacultyList)
                {
                    if (StudentFacultyExists(studentFaculty.FacultyId))
                    {
                        return new StatusCodeResult(StatusCodes.Status409Conflict);
                    }
                    else
                    {
                        throw;
                    }
                }

            }

            return NoContent();
        }

        // DELETE: api/StudentFaculties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentFaculty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentFaculty = await _context.StudentFaculties.SingleOrDefaultAsync(m => m.FacultyId == id);
            if (studentFaculty == null)
            {
                return NotFound();
            }

            _context.StudentFaculties.Remove(studentFaculty);
            await _context.SaveChangesAsync();

            return Ok(studentFaculty);
        }

        private bool StudentFacultyExists(int id)
        {
            return _context.StudentFaculties.Any(e => e.FacultyId == id);
        }
    }
}