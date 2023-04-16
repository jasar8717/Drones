using Drones.Entities.Models;

namespace Drones.Core.Services
{
    public interface IMedicationService
    {
        Task AddDronesMedication(IEnumerable<Medication> item, CancellationToken cancellationToken);
    }
}
