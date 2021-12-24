using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParkingAPI.Helpers;
using ParkingAPI.Models;
using ParkingAPI.Models.DTO;
using ParkingAPI.Models.Enums;
using ParkingAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;

namespace ParkingAPI.Controllers
{
    [Route("api/parking")]
    public class ControllersParkingSpace : ControllerBase
    {
        private readonly IParkingSpaceRepository _repository;
        private readonly IMapper _mapper;

        public ControllersParkingSpace(IParkingSpaceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("", Name = "NameGetAll")]
        public ActionResult GetAll([FromQuery] ParkingSpaceUrlQuery query)
        {
            var items = _repository.GetAll(query);

            if (items.Results.Count == 0)
                return NotFound();

            var lista = _mapper.Map<PaginationList<ParkingSpace>, PaginationList<ParkingSpaceDTO>>(items);

            foreach(ParkingSpaceDTO parkingSpaceDTO in lista.Results)
            {
                parkingSpaceDTO.Links = new List<LinkDTO>();
                parkingSpaceDTO.Links.Add(new LinkDTO("self", Url.Link("NameGetOne", new { id = parkingSpaceDTO.Id }), "GET"));
            }

            lista.Links.Add(new LinkDTO("self", Url.Link("NameGetAll", query), "GET"));

            if (items.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(items.Pagination));

                if (!items.Pagination.IsValidPage(query.PageNumber.Value))
                    return NotFound();
                else
                {
                    if((query.PageNumber + 1) <= items.Pagination.TotalPages)
                    {
                        ParkingSpaceUrlQuery nextPage = new ParkingSpaceUrlQuery { PageNumber = query.PageNumber + 1, RegisterQtt = query.RegisterQtt };
                        lista.Links.Add(new LinkDTO("next", Url.Link("NameGetAll", nextPage), "GET"));
                    }
                    
                    if((query.PageNumber - 1) > 0)
                    {
                        ParkingSpaceUrlQuery prevPage = new ParkingSpaceUrlQuery { PageNumber = query.PageNumber - 1, RegisterQtt = query.RegisterQtt };
                        lista.Links.Add(new LinkDTO("prev", Url.Link("NameGetAll", prevPage), "GET"));
                    }

                }
            }

            return Ok(lista);
        }

        [HttpGet("{id}", Name = "NameGetOne")]
        public ActionResult GetOne(int id)
        {
            ParkingSpace parkingSpace = _repository.GetOne(id);

            if (parkingSpace == null)
                return StatusCode(404);

            ParkingSpaceDTO parkingSpaceDTO = _mapper.Map<ParkingSpace, ParkingSpaceDTO>(parkingSpace);

            parkingSpaceDTO.Links = new List<LinkDTO>();
            parkingSpaceDTO.Links.Add(new LinkDTO("self", Url.Link("NameGetOne", new { id = parkingSpaceDTO.Id }), "GET"));
            parkingSpaceDTO.Links.Add(new LinkDTO("remove", Url.Link("NameRemove", new { id = parkingSpaceDTO.Id }), "DELETE"));
            parkingSpaceDTO.Links.Add(new LinkDTO("update", Url.Link("NameUpdate", new { id = parkingSpaceDTO.Id }), "PUT"));
            parkingSpaceDTO.Links.Add(new LinkDTO("parkControl", Url.Link("NameControlPark", new { id = parkingSpaceDTO.Id }), "PATCH"));

            return Ok(parkingSpaceDTO);
        }

        [HttpPost("")]
        public ActionResult Register([FromBody] ParkingSpace parkingSpace)
        {
            parkingSpace.LastVacancy = DateTime.Now;

            _repository.Register(parkingSpace);

            return Created($"api/parking/{parkingSpace.Id}", parkingSpace);
        }

        [HttpDelete("{id}", Name = "NameRemove")]
        public ActionResult Remove(int id)
        {
            ParkingSpace parkingSpace = _repository.GetOne(id);

            if (parkingSpace == null)
                return StatusCode(404);

            _repository.Remove(id);

            return NoContent();
        }

        [HttpPut("{id}", Name = "NameUpdate")]
        public ActionResult Update(int id, [FromBody] ParkingSpace parkingSpace)
        {
            ParkingSpace parkingSpaceUpdate = _repository.GetOne(id);

            if (parkingSpaceUpdate == null)
                return StatusCode(404);

            parkingSpace.Id = id;

            parkingSpace.LastOccupation = parkingSpaceUpdate.LastOccupation;
            parkingSpace.LastVacancy = parkingSpaceUpdate.LastVacancy;
            parkingSpace.ParkingStatus = parkingSpaceUpdate.ParkingStatus;

            _repository.Update(parkingSpace);

            return Ok();
        }

        [HttpPatch("{id}", Name = "NameControlPark")]
        public ActionResult ControlPark(int id)
        {
            ParkingSpace parkingSpace = _repository.GetOne(id);

            if (parkingSpace == null || parkingSpace.ParkingSapceActive == false)
                return StatusCode(404);

            if (parkingSpace.ParkingStatus == ParkingStatus.Free)
            {
                parkingSpace.ParkingStatus = ParkingStatus.Busy;
                parkingSpace.LastOccupation = DateTime.Now;
            }
            else
            {
                parkingSpace.ParkingStatus = ParkingStatus.Free;
                parkingSpace.LastVacancy = DateTime.Now;
            }

            _repository.ControlPark(parkingSpace);

            return Ok();
        }
    }
}
