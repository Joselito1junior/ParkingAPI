using Microsoft.EntityFrameworkCore;
using ParkingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.Data
{
    public class ParkingContext : DbContext
    {
        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {

        }

        public DbSet<Parking> Parking { get; set; }
    }
}
