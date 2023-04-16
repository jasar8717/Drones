using Drones.Core;
using Drones.Core.Dto;
using Drones.Core.Repositories;
using Drones.Core.Utils;
using Drones.Core.Utils.Drone;
using Drones.Core.Utils.Pagination;
using Drones.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Data.Repositories
{
    public class DroneRepository : Repository<Drone>, IDroneRepository
    {
        public DroneRepository(DronesContext context)
            : base(context)
        { }

        private DronesContext DronesContext
        {
            get { return Context as DronesContext; }
        }

        public async Task<Drone> FindDroneWithMedicationBySerialNumber(string serialNumber, CancellationToken cancellationToken)
        {
            return await DronesContext.Drones.Include(d => d.Medications).Where(d => d.SerialNumber == serialNumber).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<DroneResource>> ListAvailableDronesForLoading(CancellationToken cancellationToken)
        {
            var result = (from Drone in DronesContext.Drones.AsNoTracking()
                          where Drone.Medications.Sum(m => m.Weight) < Drone.WeightLimit && (Drone.State == (int)DroneStateEnum.IDLE || Drone.State == (int)DroneStateEnum.LOADING)
                          select new DroneResource
                          {
                              SerialNumber = Drone.SerialNumber,
                              BatteryCapacity = Drone.BatteryCapacity,
                              Model = UtilsProject.GetEnumDescription((DroneModelEnum)Drone.Model),
                              State = UtilsProject.GetEnumDescription((DroneStateEnum)Drone.State),
                              WeightLimit = Drone.WeightLimit
                          });

            return await result.ToListAsync(cancellationToken);
        }

        public async Task<PagedResponse<IEnumerable<DroneDto>>> GetAllTransformedToDto(PaginationFilter pageInfo, CancellationToken cancellationToken)
        {
            var query = (from Drone in DronesContext.Drones.AsNoTracking()
                         select new DroneDto
                         {
                             SerialNumber = Drone.SerialNumber,
                             BatteryCapacity = Drone.BatteryCapacity,
                             Model = UtilsProject.GetEnumDescription((DroneModelEnum)Drone.Model),
                             State = UtilsProject.GetEnumDescription((DroneStateEnum)Drone.State),
                             WeightLimit = Drone.WeightLimit,
                             Medication = Drone.Medications.Select(m => new MedicationDto { Code = m.Code, Name = m.Name, Image = m.Image != null ? Convert.ToBase64String(m.Image) : null, Weight = m.Weight}).ToList(),
                         });

            var totalRecords = await query.CountAsync(cancellationToken);

            var queryFilter = await query.OrderByDescending(l => l.BatteryCapacity).Skip(pageInfo.PageNumber)
                        .Take(pageInfo.PageSize)
                        .ToListAsync(cancellationToken);

            PagedResponse<IEnumerable<DroneDto>> response = new PagedResponse<IEnumerable<DroneDto>>(queryFilter,
                pageInfo.PageNumber,
                pageInfo.PageSize,
                totalRecords);

            return response;
        }

        public async Task<int> CountDroneRegistered(CancellationToken cancellationToken)
        {
            return await DronesContext.Drones.CountAsync(cancellationToken);
        }
    }
}
