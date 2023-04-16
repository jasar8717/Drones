using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones.Core.Dto
{
    public sealed class DroneDto
    {
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public float WeightLimit { get; set; }
        public float BatteryCapacity { get; set; }
        public string State { get; set; }
        public List<MedicationDto> Medication { get; set; }
    }
}
