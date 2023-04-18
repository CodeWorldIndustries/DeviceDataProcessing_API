namespace Application.DeviceData.Contracts
{
    public class GetDataSummaryRequest
    {
        public DateTime? From { get; set; } = DateTime.UtcNow.AddDays(-7);
        public DateTime? To { get; set; } = DateTime.UtcNow;
    }
}
