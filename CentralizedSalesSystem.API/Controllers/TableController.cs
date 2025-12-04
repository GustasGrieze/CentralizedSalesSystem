using CentralizedSalesSystem.API.Data;
using CentralizedSalesSystem.API.Models;
using CentralizedSalesSystem.API.Models.DTOs;
using CentralizedSalesSystem.API.Models.Orders.enums;
using CentralizedSalesSystem.API.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentralizedSalesSystem.API.Controllers
{
    [Route("tables")]
    [Authorize]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly CentralizedSalesDbContext _db;

        public TableController(CentralizedSalesDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([
            FromQuery] int page = 1,
            [FromQuery] int limit = 20,
            [FromQuery] string? sortBy = "name",
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? filterByName = null,
            [FromQuery] string? filterByStatus = null,
            [FromQuery] int? filterByCapacity = null,
            [FromQuery] long? filterByBusinessId = null)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 20;

            IQueryable<Table> query = _db.Tables.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterByName))
            {
                query = query.Where(t => t.Name.Contains(filterByName));
            }

            if (!string.IsNullOrWhiteSpace(filterByStatus))
            {
                if (Enum.TryParse<TableStatus>(filterByStatus, true, out var statusParsed))
                {
                    query = query.Where(t => t.Status == statusParsed);
                }
            }

            if (filterByCapacity.HasValue)
            {
                query = query.Where(t => t.Capacity == filterByCapacity.Value);
            }

            if (filterByBusinessId.HasValue)
            {
                query = query.Where(t => t.BusinessId == filterByBusinessId.Value);
            }

            bool asc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
            var sortKey = (sortBy ?? "name").ToLower();

            var total = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)limit);

            query = sortKey switch
            {
                "capacity" => asc ? query.OrderBy(t => t.Capacity) : query.OrderByDescending(t => t.Capacity),
                "status" => asc ? query.OrderBy(t => t.Status) : query.OrderByDescending(t => t.Status),
                _ => asc ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name),
            };

            var items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            var result = new
            {
                data = items.Select(t => new {
                    id = t.Id,
                    businessId = t.BusinessId,
                    name = t.Name,
                    capacity = t.Capacity,
                    status = t.Status.ToString().ToLowerInvariant()
                }),
                page,
                limit,
                total,
                totalPages
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TableCreateDto dto)
        {
            if (dto == null) return BadRequest();

            var table = new Table
            {
                BusinessId = dto.BusinessId,
                Name = dto.Name,
                Capacity = dto.Capacity
            };

            if (!string.IsNullOrWhiteSpace(dto.Status) && Enum.TryParse<TableStatus>(dto.Status, true, out var statusParsed))
            {
                table.Status = statusParsed;
            }

            _db.Tables.Add(table);
            await _db.SaveChangesAsync();

            var created = new {
                id = table.Id,
                businessId = table.BusinessId,
                name = table.Name,
                capacity = table.Capacity,
                status = table.Status.ToString().ToLowerInvariant()
            };

            return CreatedAtAction(nameof(GetById), new { tableId = table.Id }, created);
        }

        [HttpGet("{tableId:long}")]
        public async Task<IActionResult> GetById([FromRoute] long tableId)
        {
            var t = await _db.Tables.FindAsync(tableId);
            if (t == null) return NotFound();

            var result = new {
                id = t.Id,
                businessId = t.BusinessId,
                name = t.Name,
                capacity = t.Capacity,
                status = t.Status.ToString().ToLowerInvariant()
            };

            return Ok(result);
        }

        [HttpPatch("{tableId:long}")]
        public async Task<IActionResult> Patch([FromRoute] long tableId, [FromBody] TablePatchDto dto)
        {
            if (dto == null) return BadRequest();

            var t = await _db.Tables.FindAsync(tableId);
            if (t == null) return NotFound();

            if (dto.BusinessId.HasValue) t.BusinessId = dto.BusinessId.Value;
            if (!string.IsNullOrWhiteSpace(dto.Name)) t.Name = dto.Name;
            if (dto.Capacity.HasValue) t.Capacity = dto.Capacity.Value;
            if (!string.IsNullOrWhiteSpace(dto.Status) && Enum.TryParse<TableStatus>(dto.Status, true, out var statusParsed))
            {
                t.Status = statusParsed;
            }

            await _db.SaveChangesAsync();

            var result = new {
                id = t.Id,
                businessId = t.BusinessId,
                name = t.Name,
                capacity = t.Capacity,
                status = t.Status.ToString().ToLowerInvariant()
            };

            return Ok(result);
        }

        [HttpDelete("{tableId:long}")]
        public async Task<IActionResult> Delete([FromRoute] long tableId)
        {
            var t = await _db.Tables.FindAsync(tableId);
            if (t == null) return NotFound();

            _db.Tables.Remove(t);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
