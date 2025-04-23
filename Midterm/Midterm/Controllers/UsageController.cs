using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm.Data;
using Midterm.Dtos;
using Midterm.Models;

namespace Midterm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsageController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddUsage([FromBody] AddUsageDto dto)
        {

            var usage = new Usage
            {
                SubscriberNo = dto.SubscriberNo,
                UsageType = dto.UsageType,
                Month = dto.Month,
                Year = dto.Year
            };

            if (dto.UsageType.ToLower() == "phone")
            {
                usage.PhoneMinutes = 10;
            }
            else if (dto.UsageType.ToLower() == "internet")
            {
                usage.InternetMB = 1;
            }
            else
            {
                return BadRequest("UsageType must be either 'Phone' or 'Internet'");
            }

            _context.Usages.Add(usage);
            _context.SaveChanges();

            return Ok(new { message = "Usage recorded successfully." });
        }
    
    [HttpPost("calculate")]
        public IActionResult CalculateBill([FromBody] CalculateBillDto dto)
        {
            var usages = _context.Usages
                .Where(u => u.SubscriberNo == dto.SubscriberNo && u.Month == dto.Month && u.Year == dto.Year)
                .ToList();

            if (!usages.Any())
                return NotFound("No usage found for this subscriber in the given period.");

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
                Subscriber = dto.SubscriberNo,
                PhoneUsage = $"{totalMinutes} minutes",
                InternetUsage = $"{internetGB:F2} GB",
                PhoneBill = $"{phoneBill}$",
                InternetBill = $"{internetBill}$",
                TotalBill = $"{totalBill}$"
            });
        }

    }

}
