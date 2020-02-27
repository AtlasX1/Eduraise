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
    public class TestsController : ControllerBase
    {
        private readonly EduraiseContext _context;

        public TestsController(EduraiseContext context)
        {
            _context = context;
        }

        // GET: api/Tests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tests>>> GetTests()
        {
            return await _context.Tests.ToListAsync();
        }

        // GET: api/Tests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tests>> GetTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            return test;
        }

        [HttpGet("getQuestions/{test_id}")]
        public async Task<ActionResult<IEnumerable<Questions>>> GetQuestions(int test_id)
        {
            var questions = await _context.Questions.Where(el => el.TestId == test_id).ToListAsync();

            if (questions == null)
            {
                return NotFound();
            }

            return questions;
        }

        [HttpGet("getAnswers/{question_id}")]
        public async Task<ActionResult<IEnumerable<Answers>>> GetAnswers(int question_id)
        {
            var questions = await _context.Answers.Where(el => el.QuestionId == question_id).ToListAsync();

            if (questions == null)
            {
                return NotFound();
            }

            return questions;
        }

        // GET: api/Tests/5/1
        [HttpGet("getPassedTestsCount/{course_id}/{student_id}")]
        public async Task<ActionResult<int>> GetPassedTestsCount(int course_id, int student_id)
        {
            var blocks = await _context.Block.Where(el => el.CourseId == course_id).ToListAsync();
            var tests = new List<Tests>();
            var temp = new List<Tests>();
            foreach (Block b in blocks)
            {
                temp = await _context.Tests.Where(el => el.BlockId == b.BlockId).ToListAsync();
                foreach (Tests t in temp)
                {
                    tests.Add(t);
                }
            }

            var passedTestsCount = 0;
            
            foreach (Tests t in tests)
            {
                if (_context.Marks.ToList().Exists(el => el.StudentId == student_id && el.TestId == t.TestId))
                {
                    passedTestsCount++;
                }
            }

            return passedTestsCount;
        }

        // GET: api/Tests/5/1
        [HttpGet("getCourseTestsCount/{course_id}")]
        public async Task<ActionResult<int>> GetTestsCount(int course_id)
        {
            var blocks = await _context.Block.Where(el => el.CourseId == course_id).ToListAsync();
            var tests = new List<Tests>();
            var temp = new List<Tests>();
            foreach (Block b in blocks)
            {
                temp = await _context.Tests.Where(el => el.BlockId == b.BlockId).ToListAsync();
                foreach (Tests t in temp)
                {
                    tests.Add(t);
                }
            }

            return tests.Count;
        }
    }
}
