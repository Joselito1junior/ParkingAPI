using Microsoft.EntityFrameworkCore;
using ParkingAPI.V1.Models;

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
