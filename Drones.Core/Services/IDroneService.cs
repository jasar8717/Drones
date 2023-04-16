using Drones.Core.Dto;
using Drones.Core.Utils.Drone;
using Drones.Core.Utils.Pagination;
using Drones.Entities.Models;

namespace Drones.Core.Services
{
    public interface IDroneService
    {
        Task<Drone> FindBySerialNumber(string serialNumber, CancellationToken cancellationToken);
        Task<Drone> FindDroneWithMedicationBySerialNumber(string serialNumber, CancellationToken cancellationToken);
        Task<IEnumerable<DroneResource>> ListAvailableDronesForLoading(CancellationToken cancellationToken);
        Task Create(Drone item, CancellationToken cancellationToken);
        bool CheckIfSerialNumberExist(string serialNumber, CancellationToken cancellationToken);
        void Update(Drone item, CancellationToken cancellationToken);
        Task<PagedResponse<IEnumerable<DroneDto>>> GetAllTransformedToDto(PaginationFilter pageInfo, CancellationToken cancellationToken);
        string CheckBatteryLevel(CancellationToken cancellationToken);
        Task<int> CountDroneRegistered(CancellationToken cancellationToken);
        void UnregisterDrone(Drone drone, CancellationToken cancellationToken);
    }
}
