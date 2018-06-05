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
    [Route("api/RideAttendants")]
    public class RideAttendantsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public RideAttendantsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/RideAttendants
        [HttpGet]
        public IEnumerable<RideAttendants> GetRideAttendants()
        {
            return _context.RideAttendants;
        }

        // GET: api/RideAttendants/5
        [HttpGet("{AttendantId}/{RideId}")]
        public async Task<IActionResult> GetRideAttendants([FromRoute] int AttendantId, [FromRoute] int RideId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rideAttendants = await _context.RideAttendants.SingleOrDefaultAsync(m => m.RideId == RideId && m.AttendantId == AttendantId);

            if (rideAttendants == null)
            {
                return NotFound();
            }

            return Ok(rideAttendants);
        }

        // GET: api/RideAttendants/5
        [HttpGet("ByRideId/{RideId}")]
        public async Task<IActionResult> GetRideAttendantsByRideId([FromRoute] int RideId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rideAttendants = _context.RideAttendants.Where(m => m.RideId == RideId)
                .Select(r => r).ToList();

            if (rideAttendants == null)
            {
                return NotFound();
            }

            return Ok(rideAttendants);
        }

        // PUT: api/RideAttendants/5
        [HttpPut("{AttendantId}/{RideId}")]
        public async Task<IActionResult> PutRideAttendants([FromRoute] int AttendantId, [FromRoute] int RideId, [FromBody] RideAttendants rideAttendants)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (RideId != rideAttendants.RideId && AttendantId != rideAttendants.AttendantId)
            {
                return BadRequest();
            }

            _context.Entry(rideAttendants).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RideAttendantsExists(RideId, AttendantId))
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

        // POST: api/RideAttendants
        [HttpPost]
        public async Task<IActionResult> PostRideAttendants([FromBody] RideAttendants rideAttendants)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.RideAttendants.Add(rideAttendants);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RideAttendantsExists(rideAttendants.RideId, rideAttendants.AttendantId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRideAttendants", new { id = rideAttendants.RideId }, rideAttendants);
        }

        // DELETE: api/RideAttendants/5
        [HttpDelete("{AttendantId}/{RideId}")]
        public async Task<IActionResult> DeleteRideAttendants([FromRoute] int AttendantId, [FromRoute] int RideId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rideAttendants = await _context.RideAttendants.SingleOrDefaultAsync(m => m.RideId == RideId && m.AttendantId == AttendantId);
            if (rideAttendants == null)
            {
                return NotFound();
            }

            _context.RideAttendants.Remove(rideAttendants);
            await _context.SaveChangesAsync();

            return Ok(rideAttendants);
        }

        private bool RideAttendantsExists(int RideId, int AttendantId)
        {
            return _context.RideAttendants.Any(e => e.RideId == RideId && e.AttendantId == AttendantId);
        }
    }
}