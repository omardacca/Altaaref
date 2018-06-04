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
    [Route("api/RidesInvitations")]
    public class RidesInvitationsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public RidesInvitationsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/RidesInvitations
        [HttpGet]
        public IEnumerable<RidesInvitations> GetRidesInvitations()
        {
            return _context.RidesInvitations;
        }

        // GET: api/RidesInvitations/5
        [HttpGet("{RideId}/{CandidateId}")]
        public async Task<IActionResult> GetRidesInvitations([FromRoute] int RideId, [FromRoute] int CandidateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ridesInvitations = await _context.RidesInvitations.SingleOrDefaultAsync(m => m.RideId == RideId && m.CandidateId == CandidateId);

            if (ridesInvitations == null)
            {
                return NotFound();
            }

            return Ok(ridesInvitations);
        }

        // GET: api/RidesInvitations/5
        [HttpGet("GetRidesInvitationsByRideId/{RideId}")]
        public async Task<IActionResult> GetRidesInvitationsByRideId([FromRoute] int RideId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ridesInvitations = _context.RidesInvitations.Where(m => m.RideId == RideId)
                .Select(rideInv => new RidesInvitations
                {
                    CandidateId = rideInv.CandidateId,
                    Candidate = new Student
                    {
                        Id = rideInv.Candidate.Id,
                        FullName = rideInv.Candidate.FullName,
                        ProfilePicBlobUrl = rideInv.Candidate.ProfilePicBlobUrl
                    },
                    RideId = rideInv.RideId,
                    Ride = new Ride
                    {
                        Id = rideInv.RideId,
                        Message = rideInv.Ride.Message,
                        Date = rideInv.Ride.Date,
                        FromCity = rideInv.Ride.FromCity,
                        FromAddress = rideInv.Ride.FromAddress,
                        ToCity = rideInv.Ride.ToCity,
                        ToAddress = rideInv.Ride.ToAddress,
                        FromLat = rideInv.Ride.FromLat,
                        FromLong = rideInv.Ride.FromLong,
                        ToLat = rideInv.Ride.ToLat,
                        ToLong = rideInv.Ride.ToLong,
                        NumOfFreeSeats = rideInv.Ride.NumOfFreeSeats,
                        DriverId = rideInv.Ride.DriverId,
                        Time = rideInv.Ride.Time,
                    },
                    Status = rideInv.Status
                });

            if (ridesInvitations == null)
            {
                return NotFound();
            }

            return Ok(ridesInvitations);
        }


        // GET: api/RidesInvitations/5
        [HttpGet("GetCandidateRidesInvitations/{CandidateId}")]
        public async Task<IActionResult> GetCandidateRidesInvitations([FromRoute] int CandidateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ridesInvitations = _context.RidesInvitations.Where(m => m.CandidateId == CandidateId).ToList();

            if (ridesInvitations == null)
            {
                return NotFound();
            }

            return Ok(ridesInvitations);
        }


        // PUT: api/RidesInvitations/5
        [HttpPut("{RideId}/{CandidateId}")]
        public async Task<IActionResult> PutRidesInvitations([FromRoute] int RideId, [FromRoute] int CandidateId, [FromBody] RidesInvitations ridesInvitations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (RideId != ridesInvitations.RideId || CandidateId != ridesInvitations.CandidateId)
            {
                return BadRequest();
            }

            _context.Entry(ridesInvitations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RidesInvitationsExists(RideId, CandidateId))
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

        // POST: api/RidesInvitations
        [HttpPost]
        public async Task<IActionResult> PostRidesInvitations([FromBody] RidesInvitations ridesInvitations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.RidesInvitations.Add(ridesInvitations);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RidesInvitationsExists(ridesInvitations.RideId, ridesInvitations.CandidateId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRidesInvitations", new { RideId = ridesInvitations.RideId, CandidateId = ridesInvitations.CandidateId }, ridesInvitations);
        }

        // DELETE: api/RidesInvitations/5
        [HttpDelete("{RideId}/{CandidateId}")]
        public async Task<IActionResult> DeleteRidesInvitations([FromRoute] int RideId, [FromRoute] int CandidateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ridesInvitations = await _context.RidesInvitations.SingleOrDefaultAsync(m => m.RideId == RideId && m.CandidateId == CandidateId);
            if (ridesInvitations == null)
            {
                return NotFound();
            }

            _context.RidesInvitations.Remove(ridesInvitations);
            await _context.SaveChangesAsync();

            return Ok(ridesInvitations);
        }

        private bool RidesInvitationsExists(int RideId, int CandidateId)
        {
            return _context.RidesInvitations.Any(e => e.RideId == RideId && e.CandidateId == CandidateId);
        }
    }
}