using ParkingAPI.V1.Models.Enums;
using System;

namespace ParkingAPI.V1.Models.DTO
{
    public class ParkingSpaceDTO : BaseDTO
    {
        public int Id { get; set; }
        public bool ParkingSapceActive { get; set; }
        public ParkingStatus ParkingStatus { get; set; }
        public DateTime? LastOccupation { get; set; }
        public DateTime? LastVacancy { get; set; }
    }
}
