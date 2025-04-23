namespace Midterm.Dtos
{
    public class PayBillDto
    {
        public string? SubscriberNo { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double AmountPaid { get; set; }
    }
}
