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
    [Route("api/HelpRequests")]
    public class HelpRequestsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public HelpRequestsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/HelpRequests
        [HttpGet]
        public IEnumerable<HelpRequest> GetHelpRequest()
        {
            return _context.HelpRequest;
        }

        // GET: api/HelpRequests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHelpRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpRequest = await _context.HelpRequest.SingleOrDefaultAsync(m => m.Id == id);

            if (helpRequest == null)
            {
                return NotFound();
            }

            return Ok(helpRequest);
        }

        // GET: api/HelpRequests/5
        [HttpGet("General")]
        public async Task<IActionResult> GetGeneralHelpRequest()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpRequest = _context.HelpRequest.Where(h => h.IsGeneral == true)
                .Select(h =>
                new HelpRequest
                {
                    Id = h.Id,
                    IsGeneral = h.IsGeneral,
                    Date = h.Date,
                    IsMet = h.IsMet,
                    Message = h.Message,
                    Views = h.Views,
                    Student = h.Student
                });

            if (helpRequest == null)
            {
                return NotFound();
            }

            return Ok(helpRequest);
        }
        
        // GET: api/HelpRequests/5
        [HttpGet("AllNotGeneral/{*afterDate}")]
        public async Task<IActionResult> GetNotGeneralHelpRequest(DateTime afterDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpRequest = _context.HelpRequest.Where(h => h.IsGeneral == false && h.Date >= afterDate);

            if (helpRequest == null)
            {
                return NotFound();
            }

            return Ok(helpRequest);
        }

        // GET: api/HelpRequests/5
        [HttpGet("ByStudentId/{IdentityId}")]
        public async Task<IActionResult> GetHelpRequestByStudentId([FromRoute] string IdentityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpRequest = _context.HelpRequest.Where(h => h.Student.IdentityId == IdentityId)
                .Select(h =>
                new HelpRequest
                {   Id = h.Id,
                    IsGeneral = h.IsGeneral,
                    IsMet = h.IsMet,
                    Message = h.Message,
                    Views = h.Views,
                    Student = h.Student
                });

            if (helpRequest == null)
            {
                return NotFound();
            }

            return Ok(helpRequest);
        }

        // GET: api/HelpRequests/5
        [HttpGet("GetByStdId/{StudentId}")]
        public async Task<IActionResult> GetHelpRequestByStdId([FromRoute] int StudentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpRequest = _context.HelpRequest.Where(h => h.Student.Id == StudentId)
                .Select(h =>
                new HelpRequest
                {
                    Id = h.Id,
                    IsGeneral = h.IsGeneral,
                    IsMet = h.IsMet,
                    Message = h.Message,
                    Views = h.Views,
                    Date = h.Date
                });

            if (helpRequest == null)
            {
                return NotFound();
            }

            return Ok(helpRequest);
        }

        // PUT: api/HelpRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHelpRequest([FromRoute] int id, [FromBody] HelpRequest helpRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != helpRequest.Id)
            {
                return BadRequest();
            }

            _context.Entry(helpRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HelpRequestExists(id))
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

        // POST: api/HelpRequests
        [HttpPost]
        public async Task<IActionResult> PostHelpRequest([FromBody] HelpRequest helpRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            helpRequest.Date = DateTime.Now;

            _context.HelpRequest.Add(helpRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHelpRequest", new { id = helpRequest.Id }, helpRequest);
        }

        // DELETE: api/HelpRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHelpRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpRequest = await _context.HelpRequest.SingleOrDefaultAsync(m => m.Id == id);
            if (helpRequest == null)
            {
                return NotFound();
            }

            _context.HelpRequest.Remove(helpRequest);
            await _context.SaveChangesAsync();

            return Ok(helpRequest);
        }

        private bool HelpRequestExists(int id)
        {
            return _context.HelpRequest.Any(e => e.Id == id);
        }
    }
}