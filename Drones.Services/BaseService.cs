using Drones.Core.Services;
using Drones.Core;

namespace Drones.Services
{
    public class BaseService : IBaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task SaveChanges(CancellationToken cancellationToken)
        {
            return _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
