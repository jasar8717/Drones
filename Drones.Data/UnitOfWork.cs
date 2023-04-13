using Drones.Core;
using Drones.Entities.Models;

namespace Drones.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DronesContext _context;
        
        public UnitOfWork(DronesContext context)
        {
            this._context = context;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
