namespace Drones.Core.Dto
{
    public sealed class UpdateDroneDto
    {
        public string SerialNumber { get; set; }
        public List<SaveMedicationDto> MedicationDtoList { get; set; }
    }
}
