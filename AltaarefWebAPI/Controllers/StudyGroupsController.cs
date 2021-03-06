﻿using System;
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
        public int StudyGroupId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string StudentName { get; set; }
        public string Message { get; set; }
        public string Address { get; set; }
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
        [HttpGet("{courseId}/{IdentityId}")]
        public async Task<IActionResult> GetStudyGroup([FromRoute] int courseId, [FromRoute] string IdentityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroup = await _context.StudyGroups.SingleOrDefaultAsync(m =>
            m.CourseId == courseId && m.Student.IdentityId == IdentityId &&
            m.Date >= DateTime.Now
            );

            if (studyGroup == null)
            {
                return NotFound();
            }

            return Ok(studyGroup);
        }


        // GET: api/StudyGroups/5
        [HttpGet("ByStudentId/{StudentId}")]
        public async Task<IActionResult> GetStudyGroupsByStudentId([FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroup = _context.StudyGroups.Where(m => m.Student.Id == StudentId && m.Date >= DateTime.Now)
                .Select(sg => new StudyGroupView
                {
                    StudyGroupId = sg.Id,
                    CourseId = sg.CourseId,
                    CourseName = sg.Course.Name,
                    StudentName = sg.Student.FullName,
                    Message = sg.Message,
                    Address = sg.Address,
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
        [HttpGet("ById/{IdentityId}")]
        public async Task<IActionResult> GetStudyGroupByStudentId([FromRoute] string IdentityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroup = _context.StudyGroups.Where(m => m.Student.IdentityId == IdentityId && m.Date >= DateTime.Now)
                .Select(sg => new StudyGroupView
                {
                    StudyGroupId = sg.Id,
                    CourseId = sg.CourseId,
                    CourseName = sg.Course.Name,
                    StudentName = sg.Student.FullName,
                    Message = sg.Message,
                    Address = sg.Address,
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

            var studyGroupList = _context.StudyGroups.Where(s => s.CourseId == courseId && s.Date.Date == date.Date)
                .Select(sg => new StudyGroupView
                {
                    StudyGroupId = sg.Id,
                    CourseId = sg.CourseId,
                    CourseName = sg.Course.Name,
                    StudentName = sg.Student.FullName,
                    Message = sg.Message,
                    Address = sg.Address,
                    Date = sg.Date,
                    Time = sg.Time,
                    NumberOfAttendants = sg.StudyGroupAttendants.Where(s => s.StudyGroupId == sg.Id).Count()
                });

            if (date == null)
            {
                return NotFound();
            }

            return Ok(studyGroupList);
        }

        // GET: api/StudyGroups/5
        [HttpGet("{Id}/{NumOfAttendants}/{from:datetime}/{to:datetime}")]
        public IActionResult GetSGByCrsWithDateRangeAndNumOfAttends(int Id, int NumOfAttendants, DateTime from, DateTime to)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupList = _context.StudyGroups.Where(s =>
                s.CourseId == Id &&
                s.Date >= from &&
                s.Date <= to &&
                s.IsPublic == true &&
                s.StudyGroupAttendants.Where(sa =>
                    sa.StudyGroupId == s.Id).Count() <= NumOfAttendants)
                        .Select(sg => new StudyGroupView
                        {
                            StudyGroupId = sg.Id,
                            CourseId = sg.CourseId,
                            CourseName = sg.Course.Name,
                            StudentName = sg.Student.FullName,
                            Message = sg.Message,
                            Address = sg.Address,
                            Date = sg.Date,
                            Time = sg.Time,
                            NumberOfAttendants = sg.StudyGroupAttendants.Where(s => s.StudyGroupId == sg.Id).Count()
                        });

            if (studyGroupList == null)
            {
                return NotFound();
            }

            return Ok(studyGroupList);
        }


        // GET: api/StudyGroups/
        [HttpGet("{Id}/{from:datetime}/{to:datetime}")]
        public IActionResult GetSGByCrsWithDateRange(int Id, DateTime from, DateTime to)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroupList = _context.StudyGroups.Where(s => s.CourseId == Id && s.Date >= from && s.Date <= to && s.IsPublic == true)
                .Select(sg => new StudyGroupView
                {
                    StudyGroupId = sg.Id,
                    CourseId = sg.CourseId,
                    CourseName = sg.Course.Name,
                    StudentName = sg.Student.FullName,
                    Message = sg.Message,
                    Address = sg.Address,
                    Date = sg.Date,
                    Time = sg.Time,
                    NumberOfAttendants = sg.StudyGroupAttendants.Where(s => s.StudyGroupId == sg.Id).Count()
                                        });

            if (studyGroupList == null)
            {
                return NotFound();
            }

            return Ok(studyGroupList);
        }

        // PUT: api/StudyGroups/5
        [HttpPut("{courseId}/{StudentId}")]
        public async Task<IActionResult> PutStudyGroup([FromRoute] int courseId, [FromRoute] int StudentId, [FromBody] StudyGroup studyGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (courseId != studyGroup.CourseId || StudentId != studyGroup.StudentId)
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
                if (!StudyGroupExists(courseId, StudentId))
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

            var identityId = await _context.Student.SingleOrDefaultAsync(s => s.Id == studyGroup.StudentId);

            return CreatedAtAction("GetStudyGroup", new { courseId = studyGroup.CourseId, IdentityId = identityId },
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
        [HttpDelete("{courseId}/{IdentityId}")]
        public async Task<IActionResult> DeleteStudyGroup([FromRoute] int courseId, [FromRoute] string IdentityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studyGroup = await _context.StudyGroups.SingleOrDefaultAsync(m => m.CourseId == courseId && m.Student.IdentityId == IdentityId);
            if (studyGroup == null)
            {
                return NotFound();
            }

            _context.StudyGroups.Remove(studyGroup);
            await _context.SaveChangesAsync();

            return Ok(studyGroup);
        }

        private bool StudyGroupExists(int courseId, int StudentId)
        {
            return _context.StudyGroups.Any(e => e.CourseId == courseId && e.StudentId == StudentId);
        }
    }
}
