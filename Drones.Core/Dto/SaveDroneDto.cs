namespace Drones.Core.Dto
{
    public sealed class SaveDroneDto
    {
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public float WeightLimit { get; set; }
        public float BatteryCapacity { get; set; }
        public string State { get; set; }
        public List<SaveMedicationDto>? Medications { get; set; }
    }
}