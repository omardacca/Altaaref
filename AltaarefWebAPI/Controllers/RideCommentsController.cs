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
    [Route("api/RideComments")]
    public class RideCommentsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public RideCommentsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/RideComments
        [HttpGet]
        public IEnumerable<RideComments> GetRideComments()
        {
            return _context.RideComments;
        }

        // GET: api/RideComments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRideComments([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rideComments = await _context.RideComments.SingleOrDefaultAsync(m => m.Id == id);

            if (rideComments == null)
            {
                return NotFound();
            }

            return Ok(rideComments);
        }

        // GET: api/RideComments/5
        [HttpGet("GetByRideId/{RideId}")]
        public async Task<IActionResult> GetRideCommentsByRideId([FromRoute] int RideId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rideComments = await _context.RideComments.SingleOrDefaultAsync(m => m.RideId == RideId);

            if (rideComments == null)
            {
                return NotFound();
            }

            return Ok(rideComments);
        }

        // PUT: api/RideComments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRideComments([FromRoute] int id, [FromBody] RideComments rideComments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rideComments.Id)
            {
                return BadRequest();
            }

            _context.Entry(rideComments).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RideCommentsExists(id))
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

        // POST: api/RideComments
        [HttpPost]
        public async Task<IActionResult> PostRideComments([FromBody] RideComments rideComments)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.RideComments.Add(rideComments);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRideComments", new { id = rideComments.Id }, rideComments);
        }

        // DELETE: api/RideComments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRideComments([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rideComments = await _context.RideComments.SingleOrDefaultAsync(m => m.Id == id);
            if (rideComments == null)
            {
                return NotFound();
            }

            _context.RideComments.Remove(rideComments);
            await _context.SaveChangesAsync();

            return Ok(rideComments);
        }

        private bool RideCommentsExists(int id)
        {
            return _context.RideComments.Any(e => e.Id == id);
        }
    }
}