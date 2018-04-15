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
    [Route("api/HelpRequestComments")]
    public class HelpRequestCommentsController : Controller
    {
        private readonly AltaarefDbContext _context;

        public HelpRequestCommentsController(AltaarefDbContext context)
        {
            _context = context;
        }

        // GET: api/HelpRequestComments
        [HttpGet]
        public IEnumerable<HelpRequestComment> GetHelpRequestComment()
        {
            return _context.HelpRequestComment;
        }

        // GET: api/HelpRequestComments/5
        [HttpGet("{HelpRequestId}")]
        public async Task<IActionResult> GetHelpRequestComment([FromRoute] int HelpRequestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpRequestComment = await _context.HelpRequestComment.SingleOrDefaultAsync(m => m.HelpRequestId == HelpRequestId);

            if (helpRequestComment == null)
            {
                return NotFound();
            }

            return Ok(helpRequestComment);
        }

        // PUT: api/HelpRequestComments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHelpRequestComment([FromRoute] int id, [FromBody] HelpRequestComment helpRequestComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != helpRequestComment.Id)
            {
                return BadRequest();
            }

            _context.Entry(helpRequestComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HelpRequestCommentExists(id))
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

        // POST: api/HelpRequestComments
        [HttpPost]
        public async Task<IActionResult> PostHelpRequestComment([FromBody] HelpRequestComment helpRequestComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.HelpRequestComment.Add(helpRequestComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHelpRequestComment", new { id = helpRequestComment.Id }, helpRequestComment);
        }

        // DELETE: api/HelpRequestComments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHelpRequestComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var helpRequestComment = await _context.HelpRequestComment.SingleOrDefaultAsync(m => m.Id == id);
            if (helpRequestComment == null)
            {
                return NotFound();
            }

            _context.HelpRequestComment.Remove(helpRequestComment);
            await _context.SaveChangesAsync();

            return Ok(helpRequestComment);
        }

        private bool HelpRequestCommentExists(int id)
        {
            return _context.HelpRequestComment.Any(e => e.Id == id);
        }
    }
}