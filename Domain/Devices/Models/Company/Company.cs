namespace Domain.Devices.Models.Company
{
    public class CompanyData
    {
        public int CompanyId { get; set; }
        public string Company { get; set; }
        public List<Device> Devices { get; set; }
    }
}
