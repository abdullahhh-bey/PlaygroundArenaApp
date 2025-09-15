using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.DTO;
using PlaygroundArenaApp.Core.Models;
using PlaygroundArenaApp.Infrastructure.Data;
using System.Web.Mvc;

namespace PlaygroundArenaApp.Application.Services
{
    public class AdminArenaService
    {
        private readonly PlaygroundArenaDbContext _context;

        public AdminArenaService(PlaygroundArenaDbContext context)
        {
            _context = context;
        }



        //All write/add services


        public async Task<bool> CreateArenaService( AddArenaDTO dto )
        {
            // I allowed same emails for multiple arenas 
            //so that If someone has arenas in multiple cities
            //he can manage all of them with single email

            var arena = new Arena
            {
                Name = dto.Name,
                Email = dto.Email,
                Location = dto.Location
            };

            await _context.Arenas.AddAsync(arena);

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> CreateUserService( AddUserDTO dto)
        {
            //User cannot have same Email or same Phone Numbers
            var checkEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (checkEmail != null)
                throw new BadHttpRequestException("Email already in use!");

            var checkPhone = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
            if (checkPhone != null)
                throw new BadHttpRequestException("Phone Number already in use!");

            var user = new User
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> CreateCourtService(AddCourtByArenaIdDTO dto)
        {
            var check = await _context.Arenas.FirstOrDefaultAsync(a => a.ArenaId == dto.ArenaId);
            if (check == null)
                return false;

            var court = new Court
            {
                ArenaId = dto.ArenaId,
                Name = dto.Name,
                CourtType = dto.CourtType
            };

            await _context.Courts.AddAsync(court);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> CreateCourtTimeSlotsService(AddTimeSlotsByCourtIdDTO dto)
        {
            var check = await _context.Courts.AnyAsync(c => c.CourtId == dto.CourtId);
            if (!check)
                return false;

            if (dto.StartTime >= dto.EndTime)
                throw new BadHttpRequestException("Start time must be before End time");


            var slotCheck = await _context.TimeSlots.AnyAsync( t => 
            (t.StartTime == dto.StartTime && t.EndTime == dto.EndTime) &&
            (t.Date == dto.Date && t.CourtId == dto.CourtId));

            if (slotCheck)
                throw new BadHttpRequestException("Slot Timings already mentioned");

            var timeslot = new TimeSlot 
            {
                CourtId = dto.CourtId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Date = dto.Date,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable
            };

            await _context.TimeSlots.AddAsync(timeslot);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
