using Microsoft.AspNetCore.Mvc;
using v1Remastered.Services;
using v1Remastered.Dto;

namespace v1Remastered.Controllers
{
    [Route("Booking")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{userid}")]
        public IActionResult Booking()
        {
            return View();
        }

        [HttpPost("{userid}")]
        public IActionResult Booking(BookingDetailsDto_SlotBook submittedDetails, [FromRoute] string userid)
        {
            ViewBag.userId = userid;

            int bookingSaveStatus = _bookingService.BookSlot(submittedDetails, userid);

            return bookingSaveStatus > 0 
                ? RedirectToAction("UserProfile", "UserProfile", new { userid = userid }) 
                : Ok(new { systemMessage = "Slot booking not successful" });
        }
    }
}