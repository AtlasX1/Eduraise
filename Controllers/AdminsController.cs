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
    public class AdminsController : ControllerBase
    {
        private readonly EduraiseContext _context;

        public AdminsController(EduraiseContext context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EduraiseContext>();
            var options = optionsBuilder
                .UseSqlServer(@"Data Source=DESKTOP-6BABV49;Initial Catalog=dbo_CMS;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework")
                .Options;
  
            _context = new EduraiseContext(options);
         
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admins>>> GetAdmins()
        {
	      
            return await _context.Admins.ToListAsync();
        }

        // GET: api/Admins/VerifiedCourses
        [HttpGet("CoursesList")]
        public async Task<ActionResult<IEnumerable<Courses>>> GetVerifiedCourses()
        {
            var courses = await _context.Courses.Where(c => c.IsVerified == true).ToListAsync();
            return courses;
        }

        [HttpGet("CoursesList/NotVerifiedCourses")]
        public async Task<ActionResult<IEnumerable<Courses>>> GetNotVerifiedCourses()
        {
            var courses = await _context.Courses.Where(c => c.IsVerified == false).ToListAsync();
            return courses;
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admins>> GetAdmins(int id)
        {
            var admins = await _context.Admins.FindAsync(id);
          
            if (admins == null)
            {
                return NotFound();
            }

            return admins;
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmins(int id, Admins admins)
        {
            if (id != admins.AdminId)
            {
                return BadRequest();
            }

            _context.Entry(admins).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminsExists(id))
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

        // POST: api/Admins
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Admins>> PostAdmins(Admins admins)
        {
            _context.Admins.Add(admins);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmins", new { id = admins.AdminId }, admins);
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Admins>> DeleteAdmins(int id)
        {
            var admins = await _context.Admins.FindAsync(id);
            if (admins == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(admins);
            await _context.SaveChangesAsync();

            return admins;
        }

        private bool AdminsExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
    }
}
