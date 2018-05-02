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
    public class ViewInvitation
    {
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public StudyGroup StudyGroup { get; set; }
        public bool VerificationStatus { get; set; }
    }

    [Produces("application/json")]
    [Route("api/StudyGroupInvitations")]
    public class StudyGroupInvitationsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public StudyGroupInvitationsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/StudyGroupInvitations
        [HttpGet]
        public IEnumerable<StudyGroupInvitations> GetStudyGroupInvitations()
        {
            return _context.StudyGroupInvitations;
        }

        // GET: api/StudyGroupInvitations/5
        [HttpGet("{StudentId}")]
        public async Task<IActionResult> GetViewStudyGroupInvitations([FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupInvitations = _context.StudyGroupInvitations
                .Where(si => si.StudentId == StudentId && si.StudyGroup.Date >= DateTime.Today.Date && si.VerificationStatus == false) // NOT PUBLISHED
                .Select(m => 
                new ViewInvitation
                {
                    StudentName = m.Student.FullName,
                    CourseName = m.StudyGroup.Course.Name,
                    StudyGroup = m.StudyGroup,
                    VerificationStatus = m.VerificationStatus
                });
                

            if (studyGroupInvitations == null)
            {
                return NotFound();
            }

            return Ok(studyGroupInvitations);
        }

        // PUT: api/StudyGroupInvitations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudyGroupInvitations([FromRoute] int id, [FromBody] StudyGroupInvitations studyGroupInvitations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studyGroupInvitations.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(studyGroupInvitations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyGroupInvitationsExists(id))
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

        // POST: api/StudyGroupInvitations
        [HttpPost]
        public async Task<IActionResult> PostStudyGroupInvitations([FromBody] IEnumerable<StudyGroupInvitations> studyGroupInvitations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudyGroupInvitations.AddRange(studyGroupInvitations);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                
            }

            return CreatedAtAction("GetStudyGroupInvitations", studyGroupInvitations);
        }


        // DELETE: api/StudyGroupInvitations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyGroupInvitations([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupInvitations = await _context.StudyGroupInvitations.SingleOrDefaultAsync(m => m.StudentId == id);
            if (studyGroupInvitations == null)
            {
                return NotFound();
            }

            _context.StudyGroupInvitations.Remove(studyGroupInvitations);
            await _context.SaveChangesAsync();

            return Ok(studyGroupInvitations);
        }

        private bool StudyGroupInvitationsExists(int id)
        {
            return _context.StudyGroupInvitations.Any(e => e.StudentId == id);
        }
    }
}