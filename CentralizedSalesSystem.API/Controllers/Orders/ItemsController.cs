using CentralizedSalesSystem.API.Models.Orders.enums;
using CentralizedSalesSystem.API.Models.Orders;
using CentralizedSalesSystem.API.Data;
using CentralizedSalesSystem.API.Models.Orders.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CentralizedSalesSystem.API.Controllers.Orders
{
    [ApiController]
    [Authorize]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly CentralizedSalesDbContext _context;

        public ItemsController(CentralizedSalesDbContext context)
        {
            _context = context;
        }

        // GET: items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemReadDto>>> GetItems()
        {
            var items = await _context.Items.ToListAsync();

            var result = items.Select(i => new ItemReadDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                Type = i.Type,
                Stock = i.Stock,
                BusinessId = i.BusinessId
            });

            return Ok(result);
        }

        // GET: items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemReadDto>> GetItem(long id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
                return NotFound();

            return new ItemReadDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Type = item.Type,
                Stock = item.Stock,
                BusinessId = item.BusinessId
            };
        }

        // POST: items/add
        [HttpPost("add")]
        public async Task<ActionResult<ItemReadDto>> CreateItem(ItemCreateDto dto)
        {
            var item = new Item
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Type = dto.Type,
                Stock = dto.Stock,
                BusinessId = dto.BusinessId
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            var result = new ItemReadDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Type = item.Type,
                Stock = item.Stock,
                BusinessId = item.BusinessId
            };

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, result);
        }
    }
}

