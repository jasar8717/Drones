namespace Drones.Core.Services
{
    public interface IBaseService
    {
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
