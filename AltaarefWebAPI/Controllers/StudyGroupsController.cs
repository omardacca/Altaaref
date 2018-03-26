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

            return CreatedAtAction("GetStudyGroup", new { courseId = studyGroup.CourseId, studentId = studyGroup.StudentId }, studyGroup);
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