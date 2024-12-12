using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel_Management.Model;

namespace HotelManagement.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<bool> CreateBookingAsync(Booking booking);

        Task<Booking> UpdateBookingAsync(int id, Booking booking);
        Task<bool> DeleteBookingAsync(int id);
        Task<bool> BorrowRoomAsync(int guestId, int roomId, DateTime checkInDate, DateTime checkOutDate);
        Task<IEnumerable<object>> GetCurrentBookingsAsync();
        Task<IEnumerable<object>> GetGuestBookingHistoryAsync(int guestId);
    }
}