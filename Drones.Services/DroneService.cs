using Drones.Core;
using Drones.Core.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Services
{
    public class DroneService : IDroneService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DroneService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
    }
}
