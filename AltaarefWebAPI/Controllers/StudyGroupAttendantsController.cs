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
    [Route("api/StudyGroupAttendants")]
    public class StudyGroupAttendantsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public StudyGroupAttendantsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/StudyGroupAttendants
        [HttpGet]
        public IEnumerable<StudyGroupAttendants> GetStudyGroupAttendants()
        {
            return _context.StudyGroupAttendants;
        }

        // GET: api/StudyGroupAttendants/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudyGroupAttendants([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupAttendants = await _context.StudyGroupAttendants.SingleOrDefaultAsync(m => m.Id == id);

            if (studyGroupAttendants == null)
            {
                return NotFound();
            }

            return Ok(studyGroupAttendants);
        }

        // GET: api/StudyGroupAttendants/5
        [HttpGet("GetMiniStudentView/{GroupId}")]
        public async Task<IActionResult> GetMiniStudentView([FromRoute] int GroupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupAttendants = _context.StudyGroupAttendants.Where(sga => sga.StudyGroupId == GroupId)
                .Select(sga => new MiniStudentView { Id = sga.StudentId, FullName = sga.Student.FullName, ProfilePicBlobUrl = sga.Student.ProfilePicBlobUrl });

            if (studyGroupAttendants == null)
            {
                return NotFound();
            }

            return Ok(studyGroupAttendants);
        }


        // GET: api/StudyGroupAttendants/5
        [HttpGet("GetNames/{StudyGroupId}")]
        public async Task<IActionResult> GetAttendants([FromRoute] int StudyGroupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupAttendants = _context.StudyGroupAttendants
                .Where(sga => sga.StudyGroupId == StudyGroupId).Select(sga => sga.Student);

            if (studyGroupAttendants == null)
            {
                return NotFound();
            }

            return Ok(studyGroupAttendants);
        }

        [HttpGet("{StudyGroupId}/{StudentId}")]
        public async Task<IActionResult> GetStudyGroupAttendantsBySGAIdStdId([FromRoute] int StudyGroupId, [FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupAttendants = await _context.StudyGroupAttendants
                .SingleOrDefaultAsync(m => m.StudyGroupId == StudyGroupId && m.StudentId == StudentId);

            if (studyGroupAttendants == null)
            {
                return NotFound();
            }

            return Ok(studyGroupAttendants);
        }

        // PUT: api/StudyGroupAttendants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudyGroupAttendants([FromRoute] int id, [FromBody] StudyGroupAttendants studyGroupAttendants)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studyGroupAttendants.Id)
            {
                return BadRequest();
            }

            _context.Entry(studyGroupAttendants).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyGroupAttendantsExists(id))
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

        // POST: api/StudyGroupAttendants
        [HttpPost]
        public async Task<IActionResult> PostStudyGroupAttendants([FromBody] StudyGroupAttendants studyGroupAttendants)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudyGroupAttendants.Add(studyGroupAttendants);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudyGroupAttendants", new { id = studyGroupAttendants.Id }, studyGroupAttendants);
        }

        // DELETE: api/StudyGroupAttendants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyGroupAttendants([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupAttendants = await _context.StudyGroupAttendants.SingleOrDefaultAsync(m => m.Id == id);
            if (studyGroupAttendants == null)
            {
                return NotFound();
            }

            _context.StudyGroupAttendants.Remove(studyGroupAttendants);
            await _context.SaveChangesAsync();

            return Ok(studyGroupAttendants);
        }

        [HttpDelete("{StudyGroupId}/{StudentId}")]
        public async Task<IActionResult> DeleteSGABySGIdAndStId([FromRoute] int StudyGroupId, [FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupAttendants = await _context.StudyGroupAttendants
                .SingleOrDefaultAsync(m => m.StudyGroupId == StudyGroupId && m.StudentId == StudentId);
            if (studyGroupAttendants == null)
            {
                return NotFound();
            }

            _context.StudyGroupAttendants.Remove(studyGroupAttendants);
            await _context.SaveChangesAsync();

            return Ok(studyGroupAttendants);
        }

        private bool StudyGroupAttendantsExists(int id)
        {
            return _context.StudyGroupAttendants.Any(e => e.Id == id);
        }
    }
}