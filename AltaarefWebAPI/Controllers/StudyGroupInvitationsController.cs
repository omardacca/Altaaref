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
        public StudyGroupView StudyGroup { get; set; }
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
        [HttpGet("{IdentityId}")]
        public async Task<IActionResult> GetViewStudyGroupInvitations([FromRoute] string IdentityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupInvitations = _context.StudyGroupInvitations
                .Where(si => si.Student.IdentityId == IdentityId && si.StudyGroup.Date >= DateTime.Today.Date && si.VerificationStatus == false) 
                .Select(m => 
                new ViewInvitation
                {
                    StudyGroup = new StudyGroupView
                    {
                        StudyGroupId = m.StudyGroupId,
                        Address = m.StudyGroup.Address,
                        Date = m.StudyGroup.Date,
                        Time = m.StudyGroup.Time,
                        Message = m.StudyGroup.Message,
                        CourseId = m.StudyGroup.CourseId,
                        CourseName = m.StudyGroup.Course.Name,
                        StudentName = m.Student.FullName
                    },
                    VerificationStatus = m.VerificationStatus
                });
                

            if (studyGroupInvitations == null)
            {
                return NotFound();
            }

            return Ok(studyGroupInvitations);
        }

        // PUT: api/StudyGroupInvitations/5
        [HttpPut("{StudentId}")]
        public async Task<IActionResult> PutStudyGroupInvitations([FromRoute] int StudentId, [FromBody] StudyGroupInvitations studyGroupInvitations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (StudentId != studyGroupInvitations.Student.Id)
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
                if (!StudyGroupInvitationsExists(StudentId))
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
        [HttpDelete("{IdentityId}")]
        public async Task<IActionResult> DeleteStudyGroupInvitations([FromRoute] string IdentityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupInvitations = await _context.StudyGroupInvitations.SingleOrDefaultAsync(m => m.Student.IdentityId == IdentityId);
            if (studyGroupInvitations == null)
            {
                return NotFound();
            }

            _context.StudyGroupInvitations.Remove(studyGroupInvitations);
            await _context.SaveChangesAsync();

            return Ok(studyGroupInvitations);
        }

        private bool StudyGroupInvitationsExists(int StudentId)
        {
            return _context.StudyGroupInvitations.Any(e => e.StudentId == StudentId);
        }
    }
}