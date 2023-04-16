using Drones.Core;
using Drones.Core.Services;
using Drones.Entities.Models;

namespace Drones.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicationService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task AddDronesMedication(IEnumerable<Medication> item, CancellationToken cancellationToken)
        {
            await _unitOfWork.MedicationRepository.AddRangeAsync(item, cancellationToken);
        }
    }
}
