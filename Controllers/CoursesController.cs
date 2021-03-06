﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eduraise.Models;
using Microsoft.AspNetCore.Cors;

namespace Eduraise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly EduraiseContext _context;

        public CoursesController(EduraiseContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Courses>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Courses>> GetCourses(int id)
        {
            var courses = await _context.Courses.FindAsync(id);

            if (courses == null)
            {
                return NotFound();
            }

            return courses;
        }
        [HttpGet("getAmountOfSubscribers/{course_id}")]
        public async Task<ActionResult<int>> GetAmountOfSubscribers(int course_id)
        {
            var courseStudent = await _context.CourseStudent.Where(el => el.CourseId == course_id).ToListAsync();
            return courseStudent.Count;
        }
        // GET: api/Courses/5/Blocks
        [HttpGet("{courseId}/Blocks")]
        public async Task<ActionResult<IEnumerable<Block>>> GetBlocks(int courseId)
        {
            var blocks = await _context.Block.Where( b => b.CourseId == courseId).ToListAsync();

            if (blocks == null)
            {
                return NotFound();
            }

            return blocks;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourses(int id, Courses courses)
        {
            if (id != courses.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(courses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoursesExists(id))
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

        // POST: api/Courses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
      
        [HttpPost]
        public async Task<ActionResult<Courses>> PostCourses(Courses courses)
        {
            _context.Courses.Add(courses);
            _context.Block.AddRange(courses.Block);
            foreach(var b in courses.Block)
            {
                b.CourseId = courses.CourseId;
                _context.Lessons.AddRange(b.Lessons);
                    foreach (var l in b.Lessons)
                        l.BlockId = b.BlockId;
            }
            await _context.SaveChangesAsync();

            //make a post request to form the block body and then to form a lesson body
            //The name of the action to use for generating the URL.
            // The route data to use for generating the URL.
            //The content value to format in the entity body.
            return CreatedAtAction("GetCourses", new { id = courses.CourseId }, courses);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Courses>> DeleteCourses(int id)
        {
            var courses = await _context.Courses.FindAsync(id);
            if (courses == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(courses);
            await _context.SaveChangesAsync();

            return courses;
        }

        private bool CoursesExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
