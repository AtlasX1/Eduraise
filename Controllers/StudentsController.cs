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
    public class StudentsController : ControllerBase
    {
        private readonly EduraiseContext _context;

        public StudentsController(EduraiseContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Students>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Students>> GetStudents(int id)
        {
            var students = await _context.Students.FindAsync(id);

            if (students == null)
            {
                return NotFound();
            }

            return students;
        }
        [HttpGet("getCourses/{student_id}")]
        public async Task<ActionResult<IEnumerable<Courses>>> GetStudentCourses(int student_id)
        {
            var coursesId = await _context.CourseStudent.Where(up => up.StudentId == student_id).Select(up => up.CourseId).ToListAsync();
            var studentCourses = await _context.Courses.Where(up => coursesId.Contains(up.CourseId)).ToListAsync();
            return studentCourses;
        }

        [HttpGet("getStudent/{student_email}")]
        public async Task<ActionResult<IEnumerable<Students>>> GetStudent(string student_email)
        {
            var studentId = await _context.Students.Where(up => up.StudentEmail == student_email).ToListAsync();

            if (studentId == null)
            {
                return NotFound();
            }

            return studentId;
        }

        [HttpGet("isSubscribed/{student_id}/{course_id}")]
        public async Task<ActionResult<bool>> isSubscribed(int student_id, int course_id)
        {
            var subscribed = await _context.CourseStudent.Where(el => el.StudentId == student_id && el.CourseId == course_id).ToListAsync();

            if (subscribed == null)
            {
                return false;
            }

            return true;
        }

        [HttpGet("getSubscriptions")] 
        public async Task<ActionResult<IEnumerable<CourseStudent>>> GetSubscriptions()
        {
            return await _context.CourseStudent.ToListAsync();
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("updateStudent/{id}")]
        public async Task<IActionResult> PutStudent(int id, Students student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentsExists(id))
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

        // POST: api/Students
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Students>> PostStudents(Students students)
        {
            _context.Students.Add(students);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscriptions", new { id = students.StudentId }, students);
        }

        [HttpPost("addSubscription")]
        public async Task<IActionResult> PostSubscription(CourseStudent subscription)
        {
            CourseStudent newSub = new CourseStudent();
            newSub.CourseId = subscription.CourseId;
            newSub.DateOfOverview = DateTime.Now;
            newSub.StudentId = subscription.StudentId;
            _context.CourseStudent.Add(newSub);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Students>> DeleteStudents(int id)
        {
            var students = await _context.Students.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }

            _context.Students.Remove(students);
            await _context.SaveChangesAsync();

            return students;
        }

        // DELETE: api/Students/5
        [HttpDelete("unsubscribe/{student_id}/{course_id}")]
        public async Task<ActionResult<CourseStudent>> Unsubscribe(int student_id, int course_id)
        {
            var subscription = await _context.CourseStudent.Where(el => el.StudentId == student_id && el.CourseId == course_id).ToListAsync();
            if (subscription == null)
            {
                return NotFound();
            }

            _context.CourseStudent.Remove(subscription[0]);
            await _context.SaveChangesAsync();

            return subscription[0];
        }

        private bool StudentsExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
