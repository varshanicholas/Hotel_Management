using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HotelManagement.Repositories;
using Hotel_Management.Model; // Assuming models are scaffolded

namespace HotelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private object _context;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            return Ok(bookings);
        }

        // GET: api/Booking/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<IActionResult> CreateBooking(Booking booking)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _bookingRepository.CreateBookingAsync(booking);

            if (!success)
                return BadRequest("Unable to create booking. Please check the input data or room availability.");

            return Ok("Booking created successfully.");
        }




        // PUT: api/Booking/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedBooking = await _bookingRepository.UpdateBookingAsync(id, booking);
            if (updatedBooking == null) return NotFound();
            return Ok(updatedBooking);
        }

        // DELETE: api/Booking/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var success = await _bookingRepository.DeleteBookingAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        // Task 2: Book a Room for a Guest
        [HttpPost("BorrowRoom")]
        public async Task<IActionResult> BorrowRoom(int guestId, int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var success = await _bookingRepository.BorrowRoomAsync(guestId, roomId, checkInDate, checkOutDate);
            if (!success) return BadRequest("Unable to book the room.");
            return Ok("Room booked successfully.");
        }

        // Task 5: View Current Bookings
        [HttpGet("CurrentBookings")]
        public async Task<IActionResult> GetCurrentBookings()
        {
            var bookings = await _bookingRepository.GetCurrentBookingsAsync();
            return Ok(bookings);
        }

        // Task 11: View Booking Details for a Specific Guest
        [HttpGet("GuestBookingHistory/{guestId}")]
        public async Task<IActionResult> GetGuestBookingHistory(int guestId)
        {
            var bookings = await _bookingRepository.GetGuestBookingHistoryAsync(guestId);
            if (bookings == null || !bookings.Any()) return NotFound("No bookings found for this guest.");
            return Ok(bookings);
        }

    }
}
