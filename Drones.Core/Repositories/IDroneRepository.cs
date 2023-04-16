using Drones.Core.Dto;
using Drones.Core.Utils.Drone;
using Drones.Core.Utils.Pagination;
using Drones.Entities.Models;

namespace Drones.Core.Repositories
{
    public interface IDroneRepository : IRepository<Drone>
    {
        Task<Drone> FindDroneWithMedicationBySerialNumber(string serialNumber, CancellationToken cancellationToken);
        Task<IEnumerable<DroneResource>> ListAvailableDronesForLoading(CancellationToken cancellationToken);
        Task<PagedResponse<IEnumerable<DroneDto>>> GetAllTransformedToDto(PaginationFilter pageInfo, CancellationToken cancellationToken);
    }
}
