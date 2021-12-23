using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParkingAPI.Helpers;
using ParkingAPI.Models;
using ParkingAPI.Models.Enums;
using ParkingAPI.Repositories.Contracts;
using System;

namespace ParkingAPI.Controllers
{
    [Route("api/parking")]
    public class ControllersParkingSpace : ControllerBase
    {
        private readonly IParkingSpaceRepository _repository;

        public ControllersParkingSpace(IParkingSpaceRepository repository)
        {
            _repository = repository;
        }


        [HttpGet("")]
        public ActionResult GetAll(ParkingSpaceUrlQuery query)
        {
            var items = _repository.GetAll(query);

            if (items.Pagination != null)
            {
                if (!items.Pagination.IsValidPage(query.PageNumber.Value))
                    return NotFound();
            }

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(items.Pagination));

            return Ok(items);

        }


        [HttpGet("{id}")]
        public ActionResult GetOne(int id)
        {
            ParkingSpace parkingSpace = _repository.GetOne(id);

            if (parkingSpace == null)
                return StatusCode(404);

            return Ok(parkingSpace);
        }

        [HttpPost("")]
        public ActionResult Register([FromBody] ParkingSpace parkingSpace)
        {
            parkingSpace.LastVacancy = DateTime.Now;

            _repository.Register(parkingSpace);

            return Created($"api/parking/{parkingSpace.Id}", parkingSpace);
        }

        [HttpDelete("{id}")]
        public ActionResult Remove(int id)
        {
            ParkingSpace parkingSpace = _repository.GetOne(id);

            if (parkingSpace == null)
                return StatusCode(404);

            _repository.Remove(id);

            return NoContent();
        }

        [HttpPut("{id}")]
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

        [HttpPatch("{id}")]
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
