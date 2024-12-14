using Hotel_Management.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementNew.Repository
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelManagementContext _context;

        // Constructor injection of the database context
        public GuestRepository(HotelManagementContext context)
        {
            _context = context;
        }

        // Retrieve all guests with their bookings

        public async Task<ActionResult<IEnumerable<Guest>>> GetAllGuest()
{
    if (_context != null)
    {
        return await _context.Guests
            .Include(b => b.Bookings)
            .ToListAsync();
    }

    // Return an empty list if context is null
    return new List<Guest>();
}



        //public async Task<ActionResult<IEnumerable<Guest>>> GetAllGuest()
        //{
        //    if (_context != null)
        //    {
        //        return await _context.Guests
        //            .Include(b => b.Bookings)
        //            .ToListAsync();
        //    }

        //    // Return an empty list if context is null
        //    return new List<Guest>();
        //}

        // Retrieve a specific guest by ID with their bookings
        public async Task<ActionResult<Guest>> GetGuestById(int id)
        {
            try
            {
                if (_context != null)
                {
                    var guest = await _context.Guests
                        .Include(g => g.Bookings)
                        .FirstOrDefaultAsync(g => g.GuestId == id);
                    return guest;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Add a new guest and return the guest record with their bookings
        public async Task<ActionResult<Guest>> PostGuestReturnRecord(Guest guest)
        {
            try
            {
                if (guest == null)
                {
                    throw new ArgumentNullException(nameof(guest), "Guest data is null");
                }

                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized.");
                }

                await _context.Guests.AddAsync(guest);
                await _context.SaveChangesAsync();

                var addedGuest = await _context.Guests
                    .Include(g => g.Bookings)
                    .FirstOrDefaultAsync(g => g.GuestId == guest.GuestId);
                return addedGuest;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Add a new guest and return the guest ID
        public async Task<ActionResult<int>> PostGuestReturnId(Guest guest)
        {
            try
            {
                if (guest == null)
                {
                    throw new ArgumentNullException(nameof(guest), "Guest data is null");
                }

                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized.");
                }

                await _context.Guests.AddAsync(guest);
                var changesRecord = await _context.SaveChangesAsync();
                if (changesRecord > 0)
                {
                    return guest.GuestId;
                }
                else
                {
                    throw new Exception("Failed to save guest record to the database.");
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Update an existing guest
        public async Task<ActionResult<Guest>> PutGuest(int id, Guest guest)
        {
            try
            {
                if (guest == null)
                {
                    throw new ArgumentNullException(nameof(guest), "Guest data is null");
                }

                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized.");
                }

                var existingGuest = await _context.Guests.FindAsync(id);
                if (existingGuest == null)
                {
                    return null;
                }

                existingGuest.GuestName = guest.GuestName;
                existingGuest.PhoneNumber = guest.PhoneNumber;
                existingGuest.Email = guest.Email;
                existingGuest.Address = guest.Address;

                await _context.SaveChangesAsync();

                var updatedGuest = await _context.Guests
                    .Include(g => g.Bookings)
                    .FirstOrDefaultAsync(g => g.GuestId == guest.GuestId);
                return updatedGuest;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Delete a guest by ID
        public JsonResult DeleteGuest(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Invalid Guest id"
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                if (_context == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Database context is not initialized."
                    })
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }

                var existingGuest = _context.Guests.Find(id);
                if (existingGuest == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Guest not found."
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                _context.Guests.Remove(existingGuest);
                _context.SaveChangesAsync();

                return new JsonResult(new
                {
                    success = true,
                    message = "Guest deleted successfully."
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Error occurred while deleting the guest."
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
