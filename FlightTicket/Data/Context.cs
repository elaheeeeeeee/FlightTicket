using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Sockets;
using FlightTicket.Models;

namespace FlightTicket.Data
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        public Context(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}