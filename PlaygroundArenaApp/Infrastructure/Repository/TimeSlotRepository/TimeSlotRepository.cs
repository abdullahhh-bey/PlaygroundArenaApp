using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.DTO;
using PlaygroundArenaApp.Core.Models;
using PlaygroundArenaApp.Infrastructure.Data;
using System.Web.Helpers;

namespace PlaygroundArenaApp.Infrastructure.Repository.TimeSlotRepository
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly PlaygroundArenaDbContext _context;

        public TimeSlotRepository(PlaygroundArenaDbContext context)
        {
            _context = context;
        }

        public async Task AddAllSlots(ICollection<TimeSlot> t)
        {
            await _context.TimeSlots.AddRangeAsync(t);
        }

        public async Task AddSlot(TimeSlot t)
        {
            await _context.TimeSlots.AddAsync(t);
        }

        public async Task<bool> CheckSlots(AddTimeSlotsByCourtIdDTO d)
        {
            return await _context.TimeSlots.AnyAsync(t =>
            (t.StartTime == d.StartTime && t.EndTime == d.EndTime) &&
            (t.Date == d.Date && t.CourtId == d.CourtId));
        }

        public async Task<List<TimeSlot>> GetAllAvailableSlots(DateTime d)
        {
            return await _context.TimeSlots
                        .Where(t => t.IsAvailable == true && t.Date >= d.Date)
                        .OrderBy(t => t.Date)
                        .ToListAsync();
        }


        public async Task<List<TimeSlot>> GetAllSlots()
        {
            return await _context.TimeSlots.ToListAsync();
        }

        public async Task<List<TimeSlot>> GetAllSlotsWithIds(AddBookingDTO d)
        {
            return await _context.TimeSlots
                .Where(s => d.TimeSlotId.Contains(s.TimeSlotId) && s.IsAvailable && s.CourtId == d.CourtId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<TimeSlot>> GetSlotsWithCourtIdWithDate(int court_Id, DateTime date)
        {
           return await _context.TimeSlots
                .Where(s => s.CourtId == court_Id && s.Date.Date == date.Date)
                .ToListAsync();
        }

    }
}
