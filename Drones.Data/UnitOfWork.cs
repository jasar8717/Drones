using Drones.Core;
using Drones.Core.Repositories;
using Drones.Data.Repositories;
using Drones.Entities.Models;

namespace Drones.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DronesContext _context;
        private DroneRepository _droneRepository;
        private MedicationRepository _medicationRepository;

        public IDroneRepository DroneRepository => _droneRepository = _droneRepository ?? new DroneRepository(_context);
        public IMedicationRepository MedicationRepository => _medicationRepository = _medicationRepository ?? new MedicationRepository(_context);

        public UnitOfWork(DronesContext context)
        {
            this._context = context;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
