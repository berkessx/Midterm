
namespace Midterm.Models
{
    public class Usage
    {
        public int Id { get; set; }
        public string SubscriberNo { get; set; }
        public string UsageType { get; set; } 
        public int Month { get; set; }
        public int Year { get; set; }
        public int PhoneMinutes { get; set; } = 0;
        public int InternetMB { get; set; } = 0;
    }
}
