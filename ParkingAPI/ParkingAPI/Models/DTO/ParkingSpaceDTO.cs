using ParkingAPI.Models.Enums;
using System;


namespace ParkingAPI.Models.DTO
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
