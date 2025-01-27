using FlightTicket.Data;
using FlightTicket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightTicket.Controllers
{
    [Route("api/flights")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class FlightController : ControllerBase
    {
        
            private readonly Context _context;

            public FlightController(Context context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<IActionResult> GetFlights()
            {
                var flights = await _context.Flights.ToListAsync();
                return Ok(flights);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetFlight(int id)
            {
                var flight = await _context.Flights.FindAsync(id);
                if (flight == null)
                {
                    return NotFound("Your desired flight was not found");
                }
                return Ok(flight);
            }

            [HttpPost]
            public async Task<IActionResult> CreateFlight([FromBody] Flight flight)
            {
                if (flight == null)
                {
                    return BadRequest("Please complete the flight information");
                }

                _context.Flights.Add(flight);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetFlight), new { id = flight.Id }, flight);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateFlight(int id, [FromBody] Flight flight)
            {
                if (id != flight.Id)
                {
                    return BadRequest();
                }

                _context.Entry(flight).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(id))
                    {
                        return NotFound("This flight does not exist");
                    }
                    throw;
                }

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteFlight(int id)
            {
                var flight = await _context.Flights.FindAsync(id);
                if (flight == null)
                {
                    return NotFound();
                }

                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            private bool FlightExists(int id)
            {
                return _context.Flights.Any(i => i.Id == id);
            }
        
    }
}
