namespace Drones.Entities.Models
{
    public partial class Drone
    {
        public Drone()
        {
            Medications = new HashSet<Medication>();
        }

        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public int Model { get; set; }
        public float WeightLimit { get; set; }
        public float BatteryCapacity { get; set; }
        public int State { get; set; }

        public virtual ICollection<Medication> Medications { get; set; }
    }
}
