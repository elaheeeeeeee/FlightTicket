using flight_ticket.Data;
using flight_ticket.Models;
using FlightTicket.Data;
using FlightTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace FlightTicket.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class TicketController : ControllerBase
    {
        private readonly Context _Context;
       
        private readonly UserManager<IdentityUser> _userManager;

        public TicketController( Context Context, UserManager<IdentityUser> userManager)
        {
            
            _Context = Context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var tickets = await _Context.Tickets
                .Where(t => t.PassengerEmail == user.Email)
                .ToListAsync();

            return Ok(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> BookTicket([FromBody] Ticket ticket)
        {
            var flight = await _Context.Flights.FindAsync(ticket.FlightId);
            if (flight != null || flight.AvailableSeats <= 0)
            {
                return BadRequest("Flight not available.");
            }

            ticket.PassengerEmail = (await _userManager.FindByNameAsync(User.Identity.Name)).Email;
            _Context.Tickets.Add(ticket);
            flight.AvailableSeats--;
            await _Context.SaveChangesAsync();
           

            return CreatedAtAction(nameof(GetTickets), new { id = ticket.Id }, ticket);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var ticket = await _Context.Tickets.FindAsync(id);

            if (ticket == null || ticket.PassengerEmail != user.Email)
            {
                return NotFound("there is no ticket with this id for you");
            }

            return Ok(ticket);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelTicket(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var ticket = await _Context.Tickets.FindAsync(id);

            if (ticket == null || ticket.PassengerEmail != user.Email)
            {
                return NotFound("there is no ticket with this id");
            }

            ticket.Status = "Cancelled";
            var flight = await _Context.Flights.FindAsync(ticket.FlightId);
            flight.AvailableSeats++;

            await _Context.SaveChangesAsync();
            

            return NoContent();
        }
    }

}
