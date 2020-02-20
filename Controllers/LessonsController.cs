using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eduraise.Models;

namespace Eduraise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly EduraiseContext _context;

        public LessonsController(EduraiseContext context)
        {
            _context = context;
        }

        // GET: api/Lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lessons>>> GetLessons()
        {
            return await _context.Lessons.ToListAsync();
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lessons>> GetLessons(int id)
        {
            var lessons = await _context.Lessons.FindAsync(id);

            if (lessons == null)
            {
                return NotFound();
            }

            return lessons;
        }

        // PUT: api/Lessons/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLessons(int id, Lessons lessons)
        {
            if (id != lessons.LessonId)
            {
                return BadRequest();
            }

            _context.Entry(lessons).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonsExists(id))
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

        // POST: api/Lessons
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Lessons>> PostLessons(Lessons lessons)
        {
            _context.Lessons.Add(lessons);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLessons", new { id = lessons.LessonId }, lessons);
        }

        // DELETE: api/Lessons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lessons>> DeleteLessons(int id)
        {
            var lessons = await _context.Lessons.FindAsync(id);
            if (lessons == null)
            {
                return NotFound();
            }

            _context.Lessons.Remove(lessons);
            await _context.SaveChangesAsync();

            return lessons;
        }

        private bool LessonsExists(int id)
        {
            return _context.Lessons.Any(e => e.LessonId == id);
        }
    }
}
