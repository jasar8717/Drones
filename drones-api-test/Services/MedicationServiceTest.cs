using Drones.Core.Services;
using Drones.Entities.Models;

namespace drones_api_test.Services
{
    public class MedicationServiceTest : IMedicationService
    {
        private readonly List<Medication> _medication;
        public MedicationServiceTest()
        {
            _medication = new List<Medication>()
            {
                new Medication(){Id = 4, Code = "ABC1", Name = "Hydrocodone", Weight = 10, DroneId = 1 },
                new Medication(){Id = 5, Code = "DEF2", Name = "Metformin", Weight = 20, DroneId = 2 },
                new Medication(){Id = 6, Code = "HIJ3", Name = "Losartan", Weight = 30, DroneId = 3 }
            };
        }

        Task IMedicationService.AddDronesMedication(IEnumerable<Medication> item, CancellationToken cancellationToken)
        {
            _medication.AddRange(item);
            return Task.CompletedTask;
        }
    }
}