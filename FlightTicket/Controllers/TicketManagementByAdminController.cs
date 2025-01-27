using FlightTicket.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightTicket.Controllers
{
    [Route("api/admin/tickets")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TicketManagementByAdminController : ControllerBase
    {
        private readonly Context _Context;

        public TicketManagementByAdminController(Context Context)
        {
            _Context = Context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = _Context.Tickets.ToListAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = _Context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                return Ok(ticket);
            }
            return BadRequest();

        }
    }
}