using Hotel_Management.Model;
using HotelManagementNew.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : Controller
    {
        private readonly IGuestRepository _repository;

        // Constructor injection of the repository
        public GuestsController(IGuestRepository repository)
        {
            _repository = repository;
        }

        // Retrieve all guests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guest>>> GetAllGuests()
        {
            var guests = await _repository.GetAllGuest();
            if (guests == null || !guests.Value.Any())
            {
                return NotFound("No guests found.");
            }
            return Ok(guests);
        }

        // Retrieve a specific guest by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Guest>> GetGuestById(int id)
        {
            var guest = await _repository.GetGuestById(id);
            if (guest == null)
            {
                return NotFound("No guest found.");
            }
            return Ok(guest);
        }

        // Add a new guest and return the guest record with their bookings
        [HttpPost]
        public async Task<ActionResult<Guest>> PostGuestReturnRecord(Guest guest)
        {
            if (ModelState.IsValid)
            {
                var newGuest = await _repository.PostGuestReturnRecord(guest);
                if (newGuest != null)
                {
                    return Ok(newGuest);
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        // Add a new guest and return the guest ID
        [HttpPost("return-id")]
        public async Task<ActionResult<int>> PostGuestReturnId(Guest guest)
        {
            if (ModelState.IsValid)
            {
                var newGuestId = await _repository.PostGuestReturnId(guest);
                if (newGuestId != null)
                {
                    return Ok(newGuestId);
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        // Update an existing guest
        [HttpPut("{id}")]
        public async Task<ActionResult<Guest>> PutGuest(int id, Guest guest)
        {
            if (ModelState.IsValid)
            {
                var updatedGuest = await _repository.PutGuest(id, guest);
                if (updatedGuest != null)
                {
                    return Ok(updatedGuest);
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        // Delete a guest by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteGuest(int id)
        {
            try
            {
                var result = _repository.DeleteGuest(id);

                if (result == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Guest could not be deleted or not found."
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                Console.WriteLine(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "An unexpected error occurred." });
            }
        }
    }
}
