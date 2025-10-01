using PlaygroundArenaApp.Core.DTO;
using PlaygroundArenaApp.Core.Models;

namespace PlaygroundArenaApp.Infrastructure.Repository.TimeSlotRepository
{
    public interface ITimeSlotRepository
    {
        Task<bool> CheckSlots(AddTimeSlotsByCourtIdDTO d);
        Task AddSlot(TimeSlot t);
        Task<List<TimeSlot>> GetAllSlotsWithIds(AddBookingDTO d);
        Task<List<TimeSlot>> GetSlotsWithCourtIdWithDate(int court_Id ,  DateTime date);

    }
}
