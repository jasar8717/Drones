namespace Drones.Entities.Models
{
    public partial class Medication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public string Code { get; set; }
        public byte[] Image { get; set; }
        public int? DroneId { get; set; }

        public virtual Drone? Drone { get; set; }
    }
}
