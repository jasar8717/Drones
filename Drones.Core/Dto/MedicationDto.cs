using Microsoft.AspNetCore.Http;

namespace Drones.Core.Dto
{
    public sealed class MedicationDto
    {
        public string Name { get; set; }
        public float Weight { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
    }
}
