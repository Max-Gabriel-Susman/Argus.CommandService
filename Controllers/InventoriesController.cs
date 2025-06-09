using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Argus.CommandService.Models;

namespace Argus.CommandService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemsController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InventoryItemsController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/InventoryItems
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems()
        {
            return await _context.InventoryItems.ToListAsync();
        }

        // GET: api/InventoryItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetInventoryItem(long id)
        {
            var InventoryItem = await _context.InventoryItems.FindAsync(id);

            if (InventoryItem == null)
            {
                return NotFound();
            }

            return InventoryItem;
        }

        // PUT: api/InventoryItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryItem(long id, InventoryItem InventoryItem)
        {
            if (id != InventoryItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(InventoryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryItemExists(id))
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

        // POST: api/InventoryItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> PostInventoryItem(InventoryItem InventoryItem)
        {
            _context.InventoryItems.Add(InventoryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventoryItem), new { id = InventoryItem.Id }, InventoryItem);
        }

        // DELETE: api/InventoryItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(long id)
        {
            var InventoryItem = await _context.InventoryItems.FindAsync(id);
            if (InventoryItem == null)
            {
                return NotFound();
            }

            _context.InventoryItems.Remove(InventoryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryItemExists(long id)
        {
            return _context.InventoryItems.Any(e => e.Id == id);
        }
    }
}
