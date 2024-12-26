using Microsoft.AspNetCore.Mvc;
using CrudApi.Models;
using CrudApi.Helpers;
using CrudApi.Data;
using Microsoft.EntityFrameworkCore;


namespace CrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemsController(AppDbContext context)
        {
            _context = context;
        }

        // Get: api/Items 
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Item>>>> GetItems()
        {
            var items = await _context.Items.ToListAsync();
            var response = new ApiResponse<IEnumerable<Item>>("Items fetched successfully", 200, items);
            return Ok(response);
        }

        // GET: api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Item>>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound(new ApiResponse<Item>("Item not found", 404, null!));
            }

            var response = new ApiResponse<Item>("Item fetched successfully", 200, item);
            return Ok(response);
        }

        // POST: api/items 
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Item>>> PostItem(Item item)
        {
            _context .Items.Add(item);
            await _context.SaveChangesAsync();

            var response = new ApiResponse<Item>("Item created successfully", 201, item);
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, response);
        }

        // PUT: api/items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Item>>> PutItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest(new ApiResponse<Item>("Item ID mismatch", 400, null!));
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!_context.Items.Any(e => e.Id == id))
                {
                    return NotFound(new ApiResponse<Item>("Item not found", 404, null!));
                }
                throw;
            }

            var response = new ApiResponse<Item>("Item updated successfully", 200, item);
            return Ok(response);
        }

        // DELETE: api/items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Item>>> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if(item == null)
            {
                return NotFound(new ApiResponse<Item>("Item not found", 404, null!));
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            var response = new ApiResponse<Item>("Item deleted successfully", 204, null!);
            return NoContent();
        }
    }
}