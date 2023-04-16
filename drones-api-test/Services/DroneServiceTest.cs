using Drones.Core;
using Drones.Core.Dto;
using Drones.Core.Services;
using Drones.Core.Utils;
using Drones.Core.Utils.Drone;
using Drones.Core.Utils.Pagination;
using Drones.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drones_api_test.Services
{
    public class DroneServiceTest : IDroneService
    {
        private readonly List<Drone> _drone;
        public DroneServiceTest()
        {
            _drone = new List<Drone>()
            {
                new Drone(){Id = 1, SerialNumber = "123", BatteryCapacity = 100, Model = 0, State = 0, WeightLimit = 500, Medications = new List<Medication>(){ new Medication() {Id = 1, Code = "ABC1", Name = "Hydrocodone", Weight = 10, DroneId = 1 } } },
                new Drone(){Id = 2, SerialNumber = "456", BatteryCapacity = 20, Model = 1, State = 1, WeightLimit = 450, Medications = new List<Medication>(){ new Medication() {Id = 2, Code = "DEF2", Name = "Metformin", Weight = 20, DroneId = 2 } } },
                new Drone(){Id = 3, SerialNumber = "789", BatteryCapacity = 99, Model = 2, State = 2, WeightLimit = 500, Medications = new List<Medication>(){ new Medication() {Id = 3, Code = "HIJ3", Name = "Losartan", Weight = 30, DroneId = 3 } } }
            };
        }

        string IDroneService.CheckBatteryLevel(CancellationToken cancellationToken)
        {
            var countDrones = _drone.Where(d => d.BatteryCapacity < 25).Count();

            return $"There are {countDrones} with battery levels below 25%";
        }

        bool IDroneService.CheckIfSerialNumberExist(string serialNumber, CancellationToken cancellationToken)
        {
            return _drone.Where(d => d.SerialNumber == serialNumber).Any();
        }

        Task IDroneService.Create(Drone item, CancellationToken cancellationToken)
        {
            _drone.Add(item);
            return Task.CompletedTask;
        }

        Task<Drone> IDroneService.FindBySerialNumber(string serialNumber, CancellationToken cancellationToken)
        {
            return Task.FromResult(_drone.FirstOrDefault(d => d.SerialNumber == serialNumber));
        }

        Task<Drone> IDroneService.FindDroneWithMedicationBySerialNumber(string serialNumber, CancellationToken cancellationToken)
        {
            return Task.FromResult(_drone.FirstOrDefault(d => d.SerialNumber == serialNumber));
        }

        Task<PagedResponse<IEnumerable<DroneDto>>> IDroneService.GetAllTransformedToDto(PaginationFilter pageInfo, CancellationToken cancellationToken)
        {
            var query = (from Drone in _drone
                         select new DroneDto
                         {
                             SerialNumber = Drone.SerialNumber,
                             BatteryCapacity = Drone.BatteryCapacity,
                             Model = UtilsProject.GetEnumDescription((DroneModelEnum)Drone.Model),
                             State = UtilsProject.GetEnumDescription((DroneStateEnum)Drone.State),
                             WeightLimit = Drone.WeightLimit,
                             Medication = Drone.Medications.Select(m => new MedicationDto { Code = m.Code, Name = m.Name, Image = m.Image != null ? Convert.ToBase64String(m.Image) : null, Weight = m.Weight }).ToList(),
                         });

            var totalRecords = query.Count();

            var queryFiltered = query.OrderByDescending(l => l.BatteryCapacity).Skip(pageInfo.PageNumber)
                        .Take(pageInfo.PageSize)
                        .ToList();

            PagedResponse<IEnumerable<DroneDto>> response = new PagedResponse<IEnumerable<DroneDto>>(queryFiltered,
                pageInfo.PageNumber,
                pageInfo.PageSize,
                totalRecords);

            return Task.FromResult(response);
        }

        Task<IEnumerable<DroneResource>> IDroneService.ListAvailableDronesForLoading(CancellationToken cancellationToken)
        {
            var result = (from Drone in _drone
                          where Drone.Medications.Sum(m => m.Weight) < Drone.WeightLimit && (Drone.State == (int)DroneStateEnum.IDLE || Drone.State == (int)DroneStateEnum.LOADING)
                          select new DroneResource
                          {
                              SerialNumber = Drone.SerialNumber,
                              BatteryCapacity = Drone.BatteryCapacity,
                              Model = UtilsProject.GetEnumDescription((DroneModelEnum)Drone.Model),
                              State = UtilsProject.GetEnumDescription((DroneStateEnum)Drone.State),
                              WeightLimit = Drone.WeightLimit
                          }).ToList();

            return Task.FromResult((IEnumerable<DroneResource>)result);
        }

        void IDroneService.Update(Drone item, CancellationToken cancellationToken)
        {
            //
        }
    }
}
