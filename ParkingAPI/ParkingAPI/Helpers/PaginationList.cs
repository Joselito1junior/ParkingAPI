using ParkingAPI.V1.Models.DTO;
using System.Collections.Generic;


namespace ParkingAPI.Helpers
{
    public class PaginationList<T>
    {
        public List<T> Results { get; set; } = new List<T>();
        public Pagination Pagination { get; set; }
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
