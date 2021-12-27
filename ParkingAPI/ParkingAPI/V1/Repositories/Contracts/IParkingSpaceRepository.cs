using ParkingAPI.Helpers;
using ParkingAPI.V1.Models;

namespace ParkingAPI.V1.Repositories.Contracts
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
