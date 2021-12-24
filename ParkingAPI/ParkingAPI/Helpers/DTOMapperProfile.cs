using AutoMapper;
using ParkingAPI.Models;
using ParkingAPI.Models.DTO;
using ParkingAPI.Models.Enums;
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
