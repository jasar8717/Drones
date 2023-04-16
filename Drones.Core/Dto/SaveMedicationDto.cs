using Microsoft.AspNetCore.Http;

namespace Drones.Core.Dto
{
    public sealed class SaveMedicationDto
    {
        public string Name { get; set; }
        public float Weight { get; set; }
        public string Code { get; set; }
        public IFormFile? Image { get; set; } = null;
    }
}
