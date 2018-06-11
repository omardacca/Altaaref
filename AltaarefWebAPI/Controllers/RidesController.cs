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
    [Route("api/Rides")]
    public class RidesController : Controller
    {
        private readonly AltaarefDbContext _context;

        public RidesController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/Rides
        [HttpGet]
        public IEnumerable<Ride> GetRides()
        {
            return _context.Rides;
        }

        // GET: api/Rides/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRide([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ride = await _context.Rides.SingleOrDefaultAsync(m => m.Id == id && m.Date >= DateTime.Now);

            if (ride == null)
            {
                return NotFound();
            }

            return Ok(ride);
        }

        // GET: api/Rides/5
        [HttpGet("GetStudentRides/{StudentId}")]
        public async Task<IActionResult> GetRideByStudentId([FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ridesResults = _context.Rides.Where(m => m.DriverId == StudentId && m.Date >= DateTime.Now).Select(ride => new Ride
            {
                Id = ride.Id,
                FromLat = ride.FromLat,
                FromLong = ride.FromLong,
                ToLat = ride.ToLat,
                ToLong = ride.ToLong,
                Date = ride.Date,
                Time = ride.Time,
                FromCity = ride.FromCity,
                FromAddress = ride.FromAddress,
                ToCity = ride.ToCity,
                ToAddress = ride.ToAddress,
                Message = ride.Message,
                NumOfFreeSeats = ride.NumOfFreeSeats,
                DriverId = ride.DriverId,
                Driver = new Student
                {
                    Id = ride.Driver.Id,
                    FullName = ride.Driver.FullName,
                    ProfilePicBlobUrl = ride.Driver.ProfilePicBlobUrl
                },
                RideAttendants = ride.RideAttendants.Where(m => m.RideId == ride.Id).Select(m =>
                new RideAttendants
                {
                    AttendantId = m.AttendantId,
                    Attendant = new Student { Id = m.Attendant.Id, FullName = m.Attendant.FullName }
                }).ToList()
                
            });

            if (ridesResults == null)
            {
                return NotFound();
            }

            return Ok(ridesResults);
        }


        // GET: api/Rides/5
        [HttpGet("GetNearbyRides/{FromLong:double}/{FromLat:double}")]
        public IActionResult GetRidesWithDateAndTime([FromRoute] double FromLong, [FromRoute] double FromLat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var RidesList = _context.Rides.Where(s => s.Date >= DateTime.Now &&
                        ((s.FromLong >= s.FromLong - 0.15 &&
                        s.FromLong <= s.FromLong + 0.15) ||
                        (s.FromLat >= s.FromLat + 0.15 &&
                        s.FromLat <= s.FromLat - 0.15)))
                        .Select(ride => new Ride
                        {
                            Id = ride.Id,
                            FromLat = ride.FromLat,
                            FromLong = ride.FromLong,
                            ToLat = ride.ToLat,
                            ToLong = ride.ToLong,
                            Date = ride.Date,
                            Time = ride.Time,
                            FromCity = ride.FromCity,
                            FromAddress = ride.FromAddress,
                            ToCity = ride.ToCity,
                            ToAddress = ride.ToAddress,
                            Message = ride.Message,
                            NumOfFreeSeats = ride.NumOfFreeSeats,
                            DriverId = ride.DriverId,
                            Driver = new Student
                            {
                                Id = ride.Driver.Id,
                                FullName = ride.Driver.FullName,
                                ProfilePicBlobUrl = ride.Driver.ProfilePicBlobUrl
                            },
                            RideAttendants = ride.RideAttendants.Where(m => m.RideId == ride.Id).Select(m =>
                            new RideAttendants
                            {
                                AttendantId = m.AttendantId,
                                Attendant = new Student { Id = m.Attendant.Id, FullName = m.Attendant.FullName }
                            }).ToList()
                        });

            if (RidesList == null)
            {
                return NotFound();
            }

            return Ok(RidesList);
        }


        // GET: api/Rides/5
        [HttpGet("GetWithDateTime/{FromLong:double}/{FromLat:double}/{ToLong:double}/{ToLat:double}/{fromDate:datetime}/{timeDate:datetime}")]
        public IActionResult GetRidesWithDateAndTime(double FromLong, double FromLat, double ToLong, double ToLat, DateTime fromDate, DateTime timeDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var RidesList = _context.Rides.Where(s =>
                s.FromLong == FromLong &&
                s.FromLat == FromLat &&
                s.ToLong == ToLong &&
                s.ToLat == ToLat &&
                s.Date.ToShortDateString() == fromDate.ToShortDateString() &&
                (s.Time <= timeDate &&
                s.Time >= timeDate))
                .Select(ride => new Ride
                {
                    Id = ride.Id,
                    FromLat = ride.FromLat,
                    FromLong = ride.FromLong,
                    ToLat = ride.ToLat,
                    ToLong = ride.ToLong,
                    Date = ride.Date,
                    Time = ride.Time,
                    FromCity = ride.FromCity,
                    FromAddress = ride.FromAddress,
                    ToCity = ride.ToCity,
                    ToAddress = ride.ToAddress,
                    Message = ride.Message,
                    NumOfFreeSeats = ride.NumOfFreeSeats,
                    DriverId = ride.DriverId,
                    Driver = new Student
                    {
                        Id = ride.Driver.Id,
                        FullName = ride.Driver.FullName,
                        ProfilePicBlobUrl = ride.Driver.ProfilePicBlobUrl
                    },
                    RideAttendants = ride.RideAttendants
                });

            if (RidesList == null)
            {
                return NotFound();
            }

            return Ok(RidesList);
        }

        // GET: api/Rides/5
        [HttpGet("GetWithTime/{FromLong:double}/{FromLat:double}/{ToLong:double}/{ToLat:double}/{fromDate:datetime}")]
        public IActionResult GetRidesWithTime(double FromLong, double FromLat, double ToLong, double ToLat, DateTime fromDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var RidesList = _context.Rides.Where(s =>
                s.FromLong == FromLong &&
                s.FromLat == FromLat &&
                s.ToLong == ToLong &&
                s.ToLat == ToLat &&
                s.Date.ToShortTimeString() == fromDate.ToShortTimeString())
                .Select(ride => new Ride
                {
                    Id = ride.Id,
                    FromLat = ride.FromLat,
                    FromLong = ride.FromLong,
                    ToLat = ride.ToLat,
                    ToLong = ride.ToLong,
                    Date = ride.Date,
                    Time = ride.Time,
                    FromCity = ride.FromCity,
                    FromAddress = ride.FromAddress,
                    ToCity = ride.ToCity,
                    ToAddress = ride.ToAddress,
                    Message = ride.Message,
                    NumOfFreeSeats = ride.NumOfFreeSeats,
                    DriverId = ride.DriverId,
                    Driver = new Student
                    {
                        Id = ride.Driver.Id,
                        FullName = ride.Driver.FullName,
                        ProfilePicBlobUrl = ride.Driver.ProfilePicBlobUrl
                    },
                    RideAttendants = ride.RideAttendants
                });

            if (RidesList == null)
            {
                return NotFound();
            }

            return Ok(RidesList);

        }

        // GET: api/Rides/5
        [HttpGet("GetWithDate/{FromLong:double}/{FromLat:double}/{ToLong:double}/{ToLat:double}/{fromDate:datetime}")]
        public IActionResult GetRidesWithDate(double FromLong, double FromLat, double ToLong, double ToLat, DateTime fromDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var RidesList = _context.Rides.Where(s =>
                s.FromLong == FromLong &&
                s.FromLat == FromLat &&
                s.ToLong == ToLong &&
                s.ToLat == ToLat &&
                s.Date.ToShortDateString() == fromDate.ToShortDateString())
                .Select(ride => new Ride
                {
                    Id = ride.Id,
                    FromLat = ride.FromLat,
                    FromLong = ride.FromLong,
                    ToLat = ride.ToLat,
                    ToLong = ride.ToLong,
                    Date = ride.Date,
                    Time = ride.Time,
                    FromCity = ride.FromCity,
                    FromAddress = ride.FromAddress,
                    ToCity = ride.ToCity,
                    ToAddress = ride.ToAddress,
                    Message = ride.Message,
                    NumOfFreeSeats = ride.NumOfFreeSeats,
                    DriverId = ride.DriverId,
                    Driver = new Student
                    {
                        Id = ride.Driver.Id,
                        FullName = ride.Driver.FullName,
                        ProfilePicBlobUrl = ride.Driver.ProfilePicBlobUrl
                    },
                    RideAttendants = ride.RideAttendants
                });

            if (RidesList == null)
            {
                return NotFound();
            }

            return Ok(RidesList);

        }

        // GET: api/Rides/5
        [HttpGet("GetWithoutDate/{FromLong:double}/{FromLat:double}/{ToLong:double}/{ToLat:double}")]
        public IActionResult GetWithDateTime(double FromLong, double FromLat, double ToLong, double ToLat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var RidesList = _context.Rides.Where(s => 
                s.Date >= DateTime.Now &&
                s.FromLong == FromLong &&
                s.FromLat == FromLat &&
                s.ToLong == ToLong &&
                s.ToLat == ToLat)
                .Select(ride => new Ride
                {
                    Id = ride.Id,
                    FromLat = ride.FromLat,
                    FromLong = ride.FromLong,
                    ToLat = ride.ToLat,
                    ToLong = ride.ToLong,
                    Date = ride.Date,
                    Time = ride.Time,
                    FromCity = ride.FromCity,
                    FromAddress = ride.FromAddress,
                    ToCity = ride.ToCity,
                    ToAddress = ride.ToAddress,
                    Message = ride.Message,
                    NumOfFreeSeats = ride.NumOfFreeSeats,
                    DriverId = ride.DriverId,
                    Driver = new Student
                    {
                        Id = ride.Driver.Id,
                        FullName = ride.Driver.FullName,
                        ProfilePicBlobUrl = ride.Driver.ProfilePicBlobUrl
                    }
                });

            if (RidesList == null)
            {
                return NotFound();
            }

            return Ok(RidesList);
        }

        // GET: api/Rides/5
        [HttpGet("GetNumberOfAttendants/{RideId:int}")]
        public IActionResult GetNumberOfAttendants([FromRoute] int RideId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var RidesList = _context.Rides.Where(s => s.Id == RideId).Select(n => n.RideAttendants.Count);

            if (RidesList == null)
            {
                return NotFound();
            }

            return Ok(RidesList);
        }

        // GET: api/Rides/5
        [HttpGet("GetRideAttendantsByRideId/{RideID}")]
        public IActionResult GetRideAttendants(int RideId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var RidesList = _context.Rides.Where(s => s.Id == RideId)
                .Select(at => new List<RideAttendants>(at.RideAttendants.ToList()));

            if (RidesList == null)
            {
                return NotFound();
            }

            return Ok(RidesList);
        }

        // PUT: api/Rides/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRide([FromRoute] int id, [FromBody] Ride ride)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ride.Id)
            {
                return BadRequest();
            }

            _context.Entry(ride).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RideExists(id))
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

        // POST: api/Rides
        [HttpPost]
        public async Task<IActionResult> PostRide([FromBody] Ride ride)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Rides.Add(ride);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRideByStudentId", new { StudentId = ride.DriverId }, ride);
        }

        // DELETE: api/Rides/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRide([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ride = await _context.Rides.SingleOrDefaultAsync(m => m.Id == id);
            if (ride == null)
            {
                return NotFound();
            }

            _context.Rides.Remove(ride);
            await _context.SaveChangesAsync();

            return Ok(ride);
        }

        private bool RideExists(int id)
        {
            return _context.Rides.Any(e => e.Id == id);
        }
    }
}