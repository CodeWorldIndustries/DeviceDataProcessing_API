namespace Domain.Devices.Models.Partners
{
    /// <summary>
    /// The Sensor data for partners
    /// </summary>
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Crumbs> Crumbs { get; set; }
    }
}
