using FlightTicket.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightTicket.Controllers
{
    [Route("api/flights/search")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class FlightSearchController : ControllerBase
    {
        private readonly Context _Context;

        public FlightSearchController(Context Context)
        {
            _Context = Context;
        }

        [HttpGet]
        public async Task<IActionResult> SearchFlights([FromQuery] string departureCity, [FromQuery] string arrivalCity, [FromQuery] DateTime? departureDate)
        {
            var flights = await _Context.Flights
                .Where(f =>
                    (string.IsNullOrEmpty(departureCity) || f.DepartureCity == departureCity) &&
                    (string.IsNullOrEmpty(arrivalCity) || f.ArrivalCity == arrivalCity) &&
                    (!departureDate.HasValue || f.DepartureTime.Date == departureDate.Value.Date))
                .ToListAsync();

            return Ok(flights);
        }
    }

}