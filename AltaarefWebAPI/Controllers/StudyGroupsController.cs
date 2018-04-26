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

    public class StudyGroupView
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string StudentName { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int NumberOfAttendants { get; set; }
    }


    [Produces("application/json")]
    [Route("api/StudyGroups")]
    public class StudyGroupsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public StudyGroupsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/StudyGroups
        [HttpGet]
        public IEnumerable<StudyGroup> GetStudyGroups()
        {
            return _context.StudyGroups;
        }

        // GET: api/StudyGroups/5
        [HttpGet("{courseId}/{studentId}")]
        public async Task<IActionResult> GetStudyGroup([FromRoute] int courseId, [FromRoute] int studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroup = await _context.StudyGroups.SingleOrDefaultAsync(m => m.CourseId == courseId && m.StudentId == studentId);

            if (studyGroup == null)
            {
                return NotFound();
            }

            return Ok(studyGroup);
        }


        // GET: api/StudyGroups/5
        [HttpGet("ById/{studentId}")]
        public async Task<IActionResult> GetStudyGroupByStudentId([FromRoute] int studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroup = _context.StudyGroups.Where(m => m.StudentId == studentId)
                .Select(sg => new StudyGroupView
                {
                    CourseId = sg.CourseId,
                    CourseName = sg.Course.Name,
                    StudentName = sg.Student.FullName,
                    Message = sg.Message,
                    Date = sg.Date,
                    Time = sg.Time,
                    NumberOfAttendants = sg.StudyGroupAttendants.Where(s => s.StudyGroupId == sg.Id).Count()
                });

            if (studyGroup == null)
            {
                return NotFound();
            }

            return Ok(studyGroup);
        }

        // GET: api/StudyGroups/5
        [HttpGet("{courseId}/{*date}")]
        public IActionResult GetStudyGroupByDate(int courseId, DateTime date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupList = _context.StudyGroups.Where(s => s.CourseId == courseId && s.Date.Date == date.Date);

            if (date == null)
            {
                return NotFound();
            }

            return Ok(studyGroupList);
        }

        // GET: api/StudyGroups/5
        [HttpGet("{Id}/{*from}/{*to}")]
        public IActionResult GetSGByCrsWithDateRange(int id, DateTime from, DateTime to)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var studyGroupList = _context.StudyGroups.Where(s => s.CourseId == id && s.Date >= from && s.Date <= to);

            if (studyGroupList == null)
            {
                return NotFound();
            }

            return Ok(studyGroupList);
        }

        // GET: api/StudyGroups/5
        [HttpGet("{Id}/{numOfAttendants}/{from:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        public IActionResult GetSGByCrsWithDateRangeAndNumOfAttends(int Id, int numOfAttendants, DateTime from)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var studyGroupList = _context.StudyGroups.Where(s => 
                s.CourseId == Id && 
                s.Date >= from && 
                s.StudyGroupAttendants.Where(sa => 
                    sa.StudyGroupId == s.Id).Count() <= numOfAttendants);

            if (studyGroupList == null)
            {
                return NotFound();
            }

            return Ok(studyGroupList);
        }

        // PUT: api/StudyGroups/5
        [HttpPut("{courseId}/{studentId}")]
        public async Task<IActionResult> PutStudyGroup([FromRoute] int courseId, [FromRoute] int studentId, [FromBody] StudyGroup studyGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (courseId != studyGroup.CourseId || studentId != studyGroup.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(studyGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyGroupExists(courseId, studentId))
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

        // POST: api/StudyGroups
        [HttpPost]
        public async Task<IActionResult> PostStudyGroup([FromBody] StudyGroup studyGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StudyGroups.Add(studyGroup);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudyGroupExists(studyGroup.CourseId, studyGroup.StudentId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudyGroup", new { courseId = studyGroup.CourseId, studentId = studyGroup.StudentId },
                new StudyGroup
                {
                    Id = studyGroup.Id,
                    CourseId = studyGroup.CourseId,
                    StudentId = studyGroup.StudentId,
                    Message = studyGroup.Message,
                    Date = studyGroup.Date,
                    Time = studyGroup.Time,
                    IsPublic = studyGroup.IsPublic
                });
        }

        // DELETE: api/StudyGroups/5
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteStudyGroup([FromRoute] int courseId, [FromRoute] int studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroup = await _context.StudyGroups.SingleOrDefaultAsync(m => m.CourseId == courseId && m.StudentId == studentId);
            if (studyGroup == null)
            {
                return NotFound();
            }

            _context.StudyGroups.Remove(studyGroup);
            await _context.SaveChangesAsync();

            return Ok(studyGroup);
        }

        private bool StudyGroupExists(int courseId, int studentId)
        {
            return _context.StudyGroups.Any(e => e.CourseId == courseId && e.StudentId == studentId);
        }
    }
}
