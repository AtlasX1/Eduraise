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
    public class BlocksController : ControllerBase
    {
        private readonly EduraiseContext _context;

        public BlocksController(EduraiseContext context)
        {
            _context = context;
        }

        // GET: api/Blocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Block>>> GetBlock()
        {
            return await _context.Block.ToListAsync();
        }

        // GET: api/Blocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Block>> GetBlock(int id)
        {
            var block = await _context.Block.FindAsync(id);

            if (block == null)
            {
                return NotFound();
            }

            return block;
        }

        // GET: api/Blocks/5/Lessons
        [HttpGet("{blockId}/Lessons")]
        public async Task<ActionResult<IEnumerable<Lessons>>> GetLessons(int blockId)
        {
            var lessons = await _context.Lessons.Where(l => l.BlockId == blockId).ToListAsync();

            if (lessons == null)
            {
                return NotFound();
            }

            return lessons;
        }

        // PUT: api/Blocks/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlock(int id, Block block)
        {
            if (id != block.BlockId)
            {
                return BadRequest();
            }

            _context.Entry(block).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockExists(id))
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

        // POST: api/Blocks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Block>> PostBlock(Block block)
        {
            _context.Block.Add(block);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlock", new { id = block.BlockId }, block);
        }

        // DELETE: api/Blocks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Block>> DeleteBlock(int id)
        {
            var block = await _context.Block.FindAsync(id);
            if (block == null)
            {
                return NotFound();
            }

            _context.Block.Remove(block);
            await _context.SaveChangesAsync();

            return block;
        }

        private bool BlockExists(int id)
        {
            return _context.Block.Any(e => e.BlockId == id);
        }
    }
}
