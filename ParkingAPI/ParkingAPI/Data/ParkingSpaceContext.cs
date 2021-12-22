using Microsoft.EntityFrameworkCore;
using ParkingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.Data
{
    public class ParkingSpaceContext : DbContext
    {
        public ParkingSpaceContext(DbContextOptions<ParkingSpaceContext> options) : base(options)
        {

        }

        public DbSet<ParkingSpace> Parking { get; set; }
    }
}
