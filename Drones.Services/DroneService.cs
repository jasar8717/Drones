using Drones.Core;
using Drones.Core.Dto;
using Drones.Core.Services;
using Drones.Core.Utils.Drone;
using Drones.Core.Utils.Pagination;
using Drones.Entities.Models;

namespace Drones.Services
{
    public class DroneService : IDroneService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DroneService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Drone> FindBySerialNumber(string serialNumber, CancellationToken cancellationToken)
        {
            return await _unitOfWork.DroneRepository.FirstOrDefaultAsync(d => d.SerialNumber == serialNumber, cancellationToken);
        }

        public async Task<Drone> FindDroneWithMedicationBySerialNumber(string serialNumber, CancellationToken cancellationToken)
        {
            return await _unitOfWork.DroneRepository.FindDroneWithMedicationBySerialNumber(serialNumber, cancellationToken);
        }

        public async Task<IEnumerable<DroneResource>> ListAvailableDronesForLoading(CancellationToken cancellationToken) 
        {
            return await _unitOfWork.DroneRepository.ListAvailableDronesForLoading(cancellationToken);
        }

        public async Task Create(Drone item, CancellationToken cancellationToken)
        {
            await _unitOfWork.DroneRepository.AddAsync(item, cancellationToken);
        }

        public bool CheckIfSerialNumberExist(string serialNumber, CancellationToken cancellationToken)
        {
            return _unitOfWork.DroneRepository.Find(p => p.SerialNumber == serialNumber, cancellationToken).Any();
        }

        public void Update(Drone item, CancellationToken cancellationToken)
        {
            _unitOfWork.DroneRepository.Update(item, cancellationToken);
        }

        public Task<PagedResponse<IEnumerable<DroneDto>>> GetAllTransformedToDto(PaginationFilter pageInfo, CancellationToken cancellationToken)
        {
            return _unitOfWork.DroneRepository.GetAllTransformedToDto(pageInfo, cancellationToken);
        }

        public string CheckBatteryLevel(CancellationToken cancellationToken)
        {
            var countDrones = _unitOfWork.DroneRepository.Find(d => d.BatteryCapacity < 25, cancellationToken).Count();
            
            return $"There are {countDrones} with battery levels below 25%";
        }

        public async Task<int> CountDroneRegistered(CancellationToken cancellationToken)
        {
            return await _unitOfWork.DroneRepository.CountDroneRegistered(cancellationToken);
        }

        public void UnregisterDrone(Drone drone, CancellationToken cancellationToken)
        {
            _unitOfWork.MedicationRepository.RemoveRange(drone.Medications, cancellationToken);
            _unitOfWork.DroneRepository.Remove(drone, cancellationToken);
        }
    }
}
