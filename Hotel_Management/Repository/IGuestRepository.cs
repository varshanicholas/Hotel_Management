using Hotel_Management.Model;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementNew.Repository
{
    public interface IGuestRepository
    {
        Task<ActionResult<IEnumerable<Guest>>> GetAllGuest();
        Task<ActionResult<Guest>> GetGuestById(int id);
        Task<ActionResult<Guest>> PostGuestReturnRecord(Guest guest);
        Task<ActionResult<int>> PostGuestReturnId(Guest guest);
        Task<ActionResult<Guest>> PutGuest(int id, Guest guest);
        JsonResult DeleteGuest(int id);
    }
}
