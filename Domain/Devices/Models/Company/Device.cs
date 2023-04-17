namespace Domain.Devices.Models.Company
{
    public class Device
    {
        public int DeviceID { get; set; }
        public string Name { get; set; }
        public string StartDateTime { get; set; }
        public List<Sensor> SensorData { get; set; }
    }
}
