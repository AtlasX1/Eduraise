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
    public class TeachersController : ControllerBase
    {
        private readonly EduraiseContext _context;

        public TeachersController(EduraiseContext context)
        {
            _context = context;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teachers>>> GetTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teachers>> GetTeachers(int id)
        {
            var teachers = await _context.Teachers.FindAsync(id);

            if (teachers == null)
            {
                return NotFound();
            }

            return teachers;
        }

        [HttpGet("{teacher_id}/AllCourses")]
        public async Task<ActionResult<IEnumerable<Courses>>> GetTeachersCourses(int teacher_id)
        {
            var courses = await _context.Courses.Where( c => c.TeachersId == teacher_id).ToListAsync();

            return courses;
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeachers(int id, Teachers teachers)
        {
            if (id != teachers.TeacherId)
            {
                return BadRequest();
            }

            _context.Entry(teachers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeachersExists(id))
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

        // POST: api/Teachers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Teachers>> PostTeachers(Teachers teachers)
        {
            _context.Teachers.Add(teachers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeachers", new { id = teachers.TeacherId }, teachers);
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Teachers>> DeleteTeachers(int id)
        {
            var teachers = await _context.Teachers.FindAsync(id);
            if (teachers == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teachers);
            await _context.SaveChangesAsync();

            return teachers;
        }

        private bool TeachersExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }
    }
}
