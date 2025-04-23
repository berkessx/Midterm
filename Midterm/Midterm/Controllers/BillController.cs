using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm.Data;
using Midterm.Dtos;

namespace Midterm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BillController(AppDbContext context)
        {
            _context = context;
        }


       

        [HttpGet]
        public IActionResult QueryBill([FromQuery] string subscriberNo, [FromQuery] int month, [FromQuery] int year)
        {
            var usages = _context.Usages
                .Where(u => u.SubscriberNo == subscriberNo && u.Month == month && u.Year == year)
                .ToList();

            if (!usages.Any())
                return NotFound("No usage found.");

            int totalMinutes = usages.Sum(u => u.PhoneMinutes);
            int totalMB = usages.Sum(u => u.InternetMB);

            int extraMinutes = Math.Max(0, totalMinutes - 1000);
            double phoneBill = (extraMinutes / 1000) * 10;

            double internetGB = totalMB / 1024.0;
            double internetBill = 0;

            if (internetGB <= 20)
                internetBill = 50;
            else
                internetBill = 50 + (Math.Ceiling((internetGB - 20) / 10)) * 10;

            double totalBill = phoneBill + internetBill;

            return Ok(new
            {
                Subscriber = subscriberNo,
                Month = month,
                Year = year,
                BillTotal = $"{totalBill}$",
                Paid = false
            });
        }
        [Authorize]
        [HttpGet("detail")]
        public IActionResult QueryBillDetailed([FromQuery] string subscriberNo, [FromQuery] int month, [FromQuery] int year)
        {
            var usages = _context.Usages
                .Where(u => u.SubscriberNo == subscriberNo && u.Month == month && u.Year == year)
                .ToList();

            if (!usages.Any())
                return NotFound("No usage found.");

            int totalMinutes = usages.Sum(u => u.PhoneMinutes);
            int totalMB = usages.Sum(u => u.InternetMB);

            int extraMinutes = Math.Max(0, totalMinutes - 1000);
            double phoneBill = (extraMinutes / 1000) * 10;

            double internetGB = totalMB / 1024.0;
            double internetBill = 0;

            if (internetGB <= 20)
                internetBill = 50;
            else
                internetBill = 50 + (Math.Ceiling((internetGB - 20) / 10)) * 10;

            double totalBill = phoneBill + internetBill;

            return Ok(new
            {
                Subscriber = subscriberNo,
                Month = month,
                Year = year,
                BillTotal = $"{totalBill}$",
                PhoneBill = $"{phoneBill}$",
                InternetBill = $"{internetBill}$",
                PhoneUsage = $"{totalMinutes} minutes",
                InternetUsage = $"{internetGB:F2} GB"
            });
        }
    
    [HttpPost("pay")]
        public IActionResult PayBill([FromBody] PayBillDto dto)
        {
            var usages = _context.Usages
                .Where(u => u.SubscriberNo == dto.SubscriberNo && u.Month == dto.Month && u.Year == dto.Year)
                .ToList();

            if (!usages.Any())
                return NotFound("No usage found to pay for.");

            int totalMinutes = usages.Sum(u => u.PhoneMinutes);
            int totalMB = usages.Sum(u => u.InternetMB);

            int extraMinutes = Math.Max(0, totalMinutes - 1000);
            double phoneBill = (extraMinutes / 1000) * 10;

            double internetGB = totalMB / 1024.0;
            double internetBill = 0;

            if (internetGB <= 20)
                internetBill = 50;
            else
                internetBill = 50 + (Math.Ceiling((internetGB - 20) / 10)) * 10;

            double totalBill = phoneBill + internetBill;

            if (dto.AmountPaid >= totalBill)
            {
                return Ok(new
                {
                    message = "Payment successful. Bill is fully paid.",
                    paid = dto.AmountPaid,
                    remaining = 0
                });
            }
            else
            {
                return Ok(new
                {
                    message = "Partial payment received. Remaining amount saved.",
                    paid = dto.AmountPaid,
                    remaining = totalBill - dto.AmountPaid
                });
            }
        }

    }
}
