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
    [Route("api/UserNotifications")]
    public class UserNotificationsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public UserNotificationsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/UserNotifications
        [HttpGet]
        public IEnumerable<UserNotification> GetUserNotifications()
        {
            return _context.UserNotifications;
        }

        // GET: api/UserNotifications/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserNotification([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userNotification = await _context.UserNotifications.SingleOrDefaultAsync(m => m.Id == id);

            if (userNotification == null)
            {
                return NotFound();
            }

            return Ok(userNotification);
        }

        // GET: api/UserNotifications/5
        [HttpGet("{GetByStudentId/StudentId}")]
        public async Task<IActionResult> GetUserNotificationByStudentId([FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userNotification = await _context.UserNotifications.SingleOrDefaultAsync(m => m.StudentId == StudentId);

            if (userNotification == null)
            {
                return NotFound();
            }

            return Ok(userNotification);
        }

        // PUT: api/UserNotifications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserNotification([FromRoute] int id, [FromBody] UserNotification userNotification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userNotification.Id)
            {
                return BadRequest();
            }

            _context.Entry(userNotification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserNotificationExists(id))
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

        // POST: api/UserNotifications
        [HttpPost]
        public async Task<IActionResult> PostUserNotification([FromBody] UserNotification userNotification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserNotifications.Add(userNotification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserNotification", new { id = userNotification.Id }, userNotification);
        }

        // DELETE: api/UserNotifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserNotification([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userNotification = await _context.UserNotifications.SingleOrDefaultAsync(m => m.Id == id);
            if (userNotification == null)
            {
                return NotFound();
            }

            _context.UserNotifications.Remove(userNotification);
            await _context.SaveChangesAsync();

            return Ok(userNotification);
        }

        // DELETE: api/UserNotifications/5
        [HttpDelete("ByStudentId/{StudentId}")]
        public async Task<IActionResult> DeleteUserNotificationByStudentId([FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userNotification = await _context.UserNotifications.SingleOrDefaultAsync(m => m.StudentId == StudentId);
            if (userNotification == null)
            {
                return NotFound();
            }

            _context.UserNotifications.Remove(userNotification);
            await _context.SaveChangesAsync();

            return Ok(userNotification);
        }

        private bool UserNotificationExists(int id)
        {
            return _context.UserNotifications.Any(e => e.Id == id);
        }
    }
}