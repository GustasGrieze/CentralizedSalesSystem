using CentralizedSalesSystem.API.Data;
using CentralizedSalesSystem.API.Models;
using CentralizedSalesSystem.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentralizedSalesSystem.API.Controllers
{
    [Route("reservations")]
    [Authorize]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly CentralizedSalesDbContext _db;

        public ReservationController(CentralizedSalesDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([
            FromQuery] int page = 1,
            [FromQuery] int limit = 20,
            [FromQuery] string? sortBy = "createdAt",
            [FromQuery] string? sortDirection = "desc",
            [FromQuery] string? filterByName = null,
            [FromQuery] string? filterByPhone = null,
            [FromQuery] DateTimeOffset? filterByAppointmentTime = null,
            [FromQuery] DateTimeOffset? filterByCreationTime = null,
            [FromQuery] string? filterByStatus = null,
            [FromQuery] long? filterByBusinessId = null,
            [FromQuery] long? filterByUserId = null,
            [FromQuery] long? filterByTableId = null)
        {
            if (page < 1) page = 1;
            if (limit < 1) limit = 20;

            IQueryable<Reservation> query = _db.Reservations.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterByName))
            {
                query = query.Where(r => r.CustomerName != null && r.CustomerName.Contains(filterByName));
            }

            if (!string.IsNullOrWhiteSpace(filterByPhone))
            {
                query = query.Where(r => r.CustomerPhone != null && r.CustomerPhone.Contains(filterByPhone));
            }

            if (filterByAppointmentTime.HasValue)
            {
                query = query.Where(r => r.AppointmentTime == filterByAppointmentTime.Value);
            }

            if (filterByCreationTime.HasValue)
            {
                query = query.Where(r => r.CreatedAt == filterByCreationTime.Value);
            }

            if (!string.IsNullOrWhiteSpace(filterByStatus))
            {
                if (Enum.TryParse<ReservationStatus>(filterByStatus, true, out var parsed))
                {
                    query = query.Where(r => r.Status == parsed);
                }
            }

            if (filterByBusinessId.HasValue)
            {
                query = query.Where(r => r.BusinessId == filterByBusinessId.Value);
            }

            if (filterByUserId.HasValue)
            {
                query = query.Where(r => r.CreatedBy == filterByUserId.Value);
            }

            if (filterByTableId.HasValue)
            {
                query = query.Where(r => r.TableId == filterByTableId.Value);
            }

            bool asc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
            var sortKey = (sortBy ?? "createdAt").ToLower();

            var total = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)limit);

            query = sortKey switch
            {
                "customername" => asc ? query.OrderBy(r => r.CustomerName) : query.OrderByDescending(r => r.CustomerName),
                "appointmenttime" => asc ? query.OrderBy(r => r.AppointmentTime) : query.OrderByDescending(r => r.AppointmentTime),
                "createdat" => asc ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt),
                "createdby" => asc ? query.OrderBy(r => r.CreatedBy) : query.OrderByDescending(r => r.CreatedBy),
                "status" => asc ? query.OrderBy(r => r.Status) : query.OrderByDescending(r => r.Status),
                "guestnumber" => asc ? query.OrderBy(r => r.GuestNumber) : query.OrderByDescending(r => r.GuestNumber),
                _ => asc ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt),
            };

            var items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            var result = new
            {
                data = items.Select(r => new {
                    id = r.Id,
                    businessId = r.BusinessId,
                    customerName = r.CustomerName,
                    customerPhone = r.CustomerPhone,
                    customerNote = r.CustomerNote,
                    appointmentTime = r.AppointmentTime,
                    createdAt = r.CreatedAt,
                    createdBy = r.CreatedBy,
                    status = r.Status.ToString().ToLowerInvariant(),
                    items = r.Items.Select(i => new {
                        id = i.Id,
                        itemId = i.ItemId,
                        quantity = i.Quantity,
                        discountId = i.DiscountId,
                        notes = i.Notes
                    }),
                    assignedEmployee = r.AssignedEmployee,
                    guestNumber = r.GuestNumber,
                    tableId = r.TableId
                }),
                page,
                limit,
                total,
                totalPages
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservationCreateDto dto)
        {
            if (dto == null) return BadRequest();

            var reservation = new Reservation
            {
                BusinessId = dto.BusinessId,
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                CustomerNote = dto.CustomerNote,
                AppointmentTime = dto.AppointmentTime,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = dto.CreatedBy,
                AssignedEmployee = dto.AssignedEmployee,
                GuestNumber = dto.GuestNumber,
                TableId = dto.TableId
            };

            if (!string.IsNullOrWhiteSpace(dto.Status) && Enum.TryParse<ReservationStatus>(dto.Status, true, out var statusParsed))
            {
                reservation.Status = statusParsed;
            }

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();

            var created = new {
                id = reservation.Id,
                businessId = reservation.BusinessId,
                customerName = reservation.CustomerName,
                customerPhone = reservation.CustomerPhone,
                customerNote = reservation.CustomerNote,
                appointmentTime = reservation.AppointmentTime,
                createdAt = reservation.CreatedAt,
                createdBy = reservation.CreatedBy,
                status = reservation.Status.ToString().ToLowerInvariant(),
                items = reservation.Items.Select(i => new {
                    id = i.Id,
                    itemId = i.ItemId,
                    quantity = i.Quantity,
                    discountId = i.DiscountId,
                    notes = i.Notes
                }),
                assignedEmployee = reservation.AssignedEmployee,
                guestNumber = reservation.GuestNumber,
                tableId = reservation.TableId
            };

            return CreatedAtAction(nameof(GetById), new { reservationId = reservation.Id }, created);
        }

        [HttpGet("{reservationId:long}")]
        public async Task<IActionResult> GetById([FromRoute] long reservationId)
        {
            var r = await _db.Reservations.FindAsync(reservationId);
            if (r == null) return NotFound();

            var result = new {
                id = r.Id,
                businessId = r.BusinessId,
                customerName = r.CustomerName,
                customerPhone = r.CustomerPhone,
                customerNote = r.CustomerNote,
                appointmentTime = r.AppointmentTime,
                createdAt = r.CreatedAt,
                createdBy = r.CreatedBy,
                status = r.Status.ToString().ToLowerInvariant(),
                items = r.Items.Select(i => new {
                    id = i.Id,
                    itemId = i.ItemId,
                    quantity = i.Quantity,
                    discountId = i.DiscountId,
                    notes = i.Notes
                }),
                assignedEmployee = r.AssignedEmployee,
                guestNumber = r.GuestNumber,
                tableId = r.TableId
            };

            return Ok(result);
        }

        [HttpPatch("{reservationId:long}")]
        public async Task<IActionResult> Patch([FromRoute] long reservationId, [FromBody] ReservationPatchDto dto)
        {
            if (dto == null) return BadRequest();

            var r = await _db.Reservations.FindAsync(reservationId);
            if (r == null) return NotFound();

            if (dto.BusinessId.HasValue) r.BusinessId = dto.BusinessId.Value;
            if (!string.IsNullOrWhiteSpace(dto.CustomerName)) r.CustomerName = dto.CustomerName;
            if (!string.IsNullOrWhiteSpace(dto.CustomerPhone)) r.CustomerPhone = dto.CustomerPhone;
            if (!string.IsNullOrWhiteSpace(dto.CustomerNote)) r.CustomerNote = dto.CustomerNote;
            if (dto.AppointmentTime.HasValue) r.AppointmentTime = dto.AppointmentTime.Value;
            if (dto.CreatedBy.HasValue) r.CreatedBy = dto.CreatedBy.Value;
            if (dto.AssignedEmployee.HasValue) r.AssignedEmployee = dto.AssignedEmployee;
            if (dto.GuestNumber.HasValue) r.GuestNumber = dto.GuestNumber.Value;
            if (dto.TableId.HasValue) r.TableId = dto.TableId;
            if (!string.IsNullOrWhiteSpace(dto.Status) && Enum.TryParse<ReservationStatus>(dto.Status, true, out var parsed))
            {
                r.Status = parsed;
            }

            await _db.SaveChangesAsync();

            var result = new {
                id = r.Id,
                businessId = r.BusinessId,
                customerName = r.CustomerName,
                customerPhone = r.CustomerPhone,
                customerNote = r.CustomerNote,
                appointmentTime = r.AppointmentTime,
                createdAt = r.CreatedAt,
                createdBy = r.CreatedBy,
                status = r.Status.ToString().ToLowerInvariant(),
                items = r.Items.Select(i => new {
                    id = i.Id,
                    itemId = i.ItemId,
                    quantity = i.Quantity,
                    discountId = i.DiscountId,
                    notes = i.Notes
                }),
                assignedEmployee = r.AssignedEmployee,
                guestNumber = r.GuestNumber,
                tableId = r.TableId
            };

            return Ok(result);
        }

        [HttpDelete("{reservationId:long}")]
        public async Task<IActionResult> Delete([FromRoute] long reservationId)
        {
            var r = await _db.Reservations.FindAsync(reservationId);
            if (r == null) return NotFound();

            _db.Reservations.Remove(r);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}