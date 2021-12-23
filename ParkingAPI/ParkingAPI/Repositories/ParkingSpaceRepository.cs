using Microsoft.EntityFrameworkCore;
using ParkingAPI.Data;
using ParkingAPI.Helpers;
using ParkingAPI.Models;
using ParkingAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAPI.Repositories
{
    public class ParkingSpaceRepository : IParkingSpaceRepository
    {

        private readonly ParkingSpaceContext _database;

        public ParkingSpaceRepository(ParkingSpaceContext database)
        {
            _database = database;
        }

        public PaginationList<ParkingSpace> GetAll(ParkingSpaceUrlQuery query)
        {
            PaginationList<ParkingSpace> list = new PaginationList<ParkingSpace>();

            var items = _database.Parking.AsNoTracking().AsQueryable();
            
            Pagination pagination = null;

            if (query.PageNumber.HasValue && query.RegisterQtt.HasValue)
            {
                pagination = new Pagination(query.PageNumber.Value, query.RegisterQtt.Value, items.Count());

                items = items.Skip((query.PageNumber.Value - 1) * query.RegisterQtt.Value).Take(query.RegisterQtt.Value);

            }

            list.Pagination = pagination;

            list.AddRange(items.ToList());

            return list;
        }

        public ParkingSpace GetOne(int id)
        {
            return _database.Parking.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }

        public void Register(ParkingSpace parkingSpace)
        {
            _database.Parking.Add(parkingSpace);
            _database.SaveChanges();
        }

        public void Remove(int id)
        {
            ParkingSpace parkingSpace = GetOne(id);

            parkingSpace.ParkingSapceActive = false;

            _database.Parking.Update(parkingSpace);
            _database.SaveChanges();
        }

        public void Update(ParkingSpace parkingSpace)
        {
            
            _database.Parking.Update(parkingSpace);
            _database.SaveChanges();
        }

        public void ControlPark(ParkingSpace parkingSpace)
        {
            _database.Parking.Update(parkingSpace);
            _database.SaveChanges();
        }
    }
}
