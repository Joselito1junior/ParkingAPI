using AutoMapper;
using ParkingAPI.V1.Models;
using ParkingAPI.V1.Models.DTO;
using ParkingAPI.V1.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.Helpers
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<ParkingSpace, ParkingSpaceDTO>();
            CreateMap<PaginationList<ParkingSpace>, PaginationList<ParkingSpaceDTO>>();
        }
    }
}
