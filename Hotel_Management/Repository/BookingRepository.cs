using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel_Management.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelManagementContext _context;

        public BookingRepository(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        //post
        public async Task<bool> CreateBookingAsync(Booking booking)
        {
            try
            {
                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "BorrowRoom"; // Name of your stored procedure
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Add parameters to the stored procedure
                var guestIdParam = new SqlParameter("@GuestId", booking.GuestId);
                var roomIdParam = new SqlParameter("@RoomId", booking.RoomId);
                var checkInDateParam = new SqlParameter("@CheckInDate", booking.CheckInDate);
                var checkOutDateParam = new SqlParameter("@CheckOutDate", booking.CheckOutDate);

                command.Parameters.Add(guestIdParam);
                command.Parameters.Add(roomIdParam);
                command.Parameters.Add(checkInDateParam);
                command.Parameters.Add(checkOutDateParam);

                await _context.Database.OpenConnectionAsync();
                await command.ExecuteNonQueryAsync();
                await _context.Database.CloseConnectionAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error while creating booking: {ex.Message}");
                return false;
            }
        }

        public async Task<Booking> UpdateBookingAsync(int id, Booking booking)
        {
            var existingBooking = await _context.Bookings.FindAsync(id);
            if (existingBooking == null) return null;

            existingBooking.CheckInDate = booking.CheckInDate;
            existingBooking.CheckOutDate = booking.CheckOutDate;
            existingBooking.BookingStatus = booking.BookingStatus;

            _context.Bookings.Update(existingBooking);
            await _context.SaveChangesAsync();
            return existingBooking;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BorrowRoomAsync(int guestId, int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var booking = new Booking
                {
                    GuestId = guestId,
                    RoomId = roomId,
                    CheckInDate = checkInDate,
                    CheckOutDate = checkOutDate,
                    BookingStatus = "Active"
                };

                _context.Bookings.Add(booking);

                var room = await _context.Rooms.FindAsync(roomId);
                if (room == null || room.AvailabilityStatus != "Available") return false;

                room.AvailabilityStatus = "Occupied";
                _context.Rooms.Update(room);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<IEnumerable<object>> GetCurrentBookingsAsync()
        {
            return await _context.CurrentBookings.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetGuestBookingHistoryAsync(int guestId)
        {
            return await _context.GuestBookingHistories
                .Where(history =>
                    _context.Bookings.Any(b =>
                        b.GuestId == guestId &&
                        b.RoomId == history.RoomId &&
                        b.CheckInDate == history.CheckInDate &&
                        b.CheckOutDate == history.CheckOutDate))
                .Select(history => new
                {
                    history.GuestName,
                    history.RoomId,
                    history.CheckInDate,
                    history.CheckOutDate
                })
                .ToListAsync();
        }


    }
}