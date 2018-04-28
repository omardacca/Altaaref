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
    [Route("api/StudyGroupComments")]
    public class StudyGroupCommentsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public StudyGroupCommentsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/StudyGroupComments
        [HttpGet]
        public IEnumerable<StudyGroupComment> GetStudyGroupComment()
        {
            return _context.StudyGroupComment;
        }

        // GET: api/StudyGroupComments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudyGroupComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupComment = await _context.StudyGroupComment.SingleOrDefaultAsync(m => m.Id == id);

            if (studyGroupComment == null)
            {
                return NotFound();
            }

            return Ok(studyGroupComment);
        }

        // GET: StudyGroupComments/BySGId/5
        [HttpGet("BySGId/{StudyGroupId}")]
        public async Task<IActionResult> GetCommentsByStudyGroupId([FromRoute] int StudyGroupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupComment = _context.StudyGroupComment.Where(sgc => sgc.StudyGroupId == StudyGroupId)
                .Select(sgc => new StudyGroupCommentView
                {
                    CommentId = sgc.Id,
                    StudentId = sgc.StudentId,
                    StudentFullName = sgc.Student.FullName,
                    ProfilePicBlobUrl = sgc.Student.ProfilePicBlobUrl,
                    Comment = sgc.Comment,
                    FullTime = sgc.FullTime
                });

            if (studyGroupComment == null)
            {
                return NotFound();
            }

            return Ok(studyGroupComment);
        }

        // PUT: api/StudyGroupComments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudyGroupComment([FromRoute] int id, [FromBody] StudyGroupComment studyGroupComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studyGroupComment.Id)
            {
                return BadRequest();
            }

            _context.Entry(studyGroupComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyGroupCommentExists(id))
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

        // POST: api/StudyGroupComments
        [HttpPost]
        public async Task<IActionResult> PostStudyGroupComment([FromBody] StudyGroupComment studyGroupComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudyGroupComment.Add(studyGroupComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudyGroupComment", new { id = studyGroupComment.Id }, studyGroupComment);
        }

        // DELETE: api/StudyGroupComments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyGroupComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupComment = await _context.StudyGroupComment.SingleOrDefaultAsync(m => m.Id == id);
            if (studyGroupComment == null)
            {
                return NotFound();
            }

            _context.StudyGroupComment.Remove(studyGroupComment);
            await _context.SaveChangesAsync();

            return Ok(studyGroupComment);
        }

        private bool StudyGroupCommentExists(int id)
        {
            return _context.StudyGroupComment.Any(e => e.Id == id);
        }
    }
}