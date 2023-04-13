

namespace Drones.Core
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
    }
}
