﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingAPI.Data;
using ParkingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingAPI.Models.Enums;

namespace ParkingAPI.Controllers
{
    [Route("api/parking")]
    public class ControllersParkingSpace : ControllerBase
    {
        private readonly ParkingSpaceContext _dataBase;

        public ControllersParkingSpace(ParkingSpaceContext dataBase)
        {
            _dataBase = dataBase;
        }

        [HttpGet("")]
        public ActionResult GetAll(int? numPagina, int? qtdRegistros)
        {
            var item = _dataBase.Parking.AsQueryable();

            if(numPagina.HasValue)
            {
                item = item.Skip((numPagina.Value - 1) * qtdRegistros.Value).Take(qtdRegistros.Value);
            }

            return Ok(item);
        }

        [HttpGet("{id}")]
        public ActionResult GetOne(int id)
        {
            ParkingSpace parkingSpace = _dataBase.Parking.Find(id);

            if (parkingSpace == null)
                return StatusCode(404);

            return Ok(_dataBase.Parking.Find(id));
        }

        [HttpPost("")]
        public ActionResult Register([FromBody]ParkingSpace parkingSpace)
        {
            parkingSpace.LastVacancy = DateTime.Now;
            _dataBase.Parking.Add(parkingSpace);
            _dataBase.SaveChanges();
            return Created($"api/parking/{parkingSpace.Id}", parkingSpace);
        }

        [HttpDelete("{id}")]
        public ActionResult Remove(int id)
        {
            ParkingSpace parkingSpace = _dataBase.Parking.Find(id);

            if (parkingSpace == null)
                return StatusCode(404);

            parkingSpace.ParkingSapceActive = false;

            _dataBase.Parking.Update(parkingSpace);
            _dataBase.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] ParkingSpace parkingSpace)
        {
            ParkingSpace parkingSpaceUpdate = _dataBase.Parking.AsNoTracking().FirstOrDefault(a => a.Id == id);

            if (parkingSpaceUpdate == null)
                return StatusCode(404);

            parkingSpace.Id = id;

            parkingSpace.LastOccupation = parkingSpaceUpdate.LastOccupation;
            parkingSpace.LastVacancy = parkingSpaceUpdate.LastVacancy;
            parkingSpace.ParkingStatus = parkingSpaceUpdate.ParkingStatus;

            _dataBase.Parking.Update(parkingSpace);
            _dataBase.SaveChanges();

            return Ok();
        }

        [HttpPatch("{id}")]
        public ActionResult ControlPark(int id)
        {
            ParkingSpace parkingSpace = _dataBase.Parking.Find(id);

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

            _dataBase.Parking.Update(parkingSpace);
            _dataBase.SaveChanges();

            return Ok();
        }
    }
}
