using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.V1.Models.DTO
{
    public class BaseDTO
    {
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
