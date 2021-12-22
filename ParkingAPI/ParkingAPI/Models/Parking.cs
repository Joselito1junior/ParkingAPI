using ParkingAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.Models
{
    public class Parking
    {
        public int Id { get; set; }
        public bool ParkingSapceStatus { get; set; }
        public ParkingStatus ParkingStatus { get; set; }
        public DateTime LastOccupation { get; set; }
        public DateTime LastVacancy { get; set; }
    }
}
