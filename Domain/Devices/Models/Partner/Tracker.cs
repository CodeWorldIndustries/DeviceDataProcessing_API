namespace Domain.Devices.Models.Partners
{
    /// <summary>
    /// The Tracker data for partners
    /// </summary>
    public class Tracker
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public DateTime ShipmentStartDtm { get; set; }
        public List<Sensor> Sensors { get; set; }
    }
}
