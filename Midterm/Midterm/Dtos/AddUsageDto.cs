namespace Midterm.Dtos
{
    public class AddUsageDto
    {
        public string? SubscriberNo { get; set; }
        public string? UsageType { get; set; } 
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
