using Drones.Core.Repositories;

namespace Drones.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IDroneRepository DroneRepository { get; }
        IMedicationRepository MedicationRepository { get; }
        Task<int> CommitAsync(CancellationToken cancellationToken);
    }
}
