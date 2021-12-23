using ParkingAPI.Helpers;
using ParkingAPI.Models;

namespace ParkingAPI.Repositories.Contracts
{
    public interface IParkingSpaceRepository
    {
        PaginationList<ParkingSpace> GetAll(ParkingSpaceUrlQuery query);
        ParkingSpace GetOne(int id);
        void Register(ParkingSpace parkingSpace);
        void Remove(int id);
        void Update(ParkingSpace parkingSpace);
        void ControlPark(ParkingSpace parkingSpace);
    }
}
