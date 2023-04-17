namespace Domain.Devices.Models.Partners
{
    /// <summary>
    /// The partner class
    /// </summary>
    public class PartnerData
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public List<Tracker> Trackers { get; set; }
    }
}
