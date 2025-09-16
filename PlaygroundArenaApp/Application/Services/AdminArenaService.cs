using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.DTO;
using PlaygroundArenaApp.Core.Models;
using PlaygroundArenaApp.Infrastructure.Data;

namespace PlaygroundArenaApp.Application.Services
{
    public class AdminArenaService
    {
        private readonly PlaygroundArenaDbContext _context;
        private readonly ILogger<AdminArenaService> _logger;
        public AdminArenaService(PlaygroundArenaDbContext context , ILogger<AdminArenaService> logger)
        {
            _context = context;
            _logger = logger;
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
            _logger.LogInformation("Arena {ID} added to the Database at {Time}", arena.ArenaId , DateTime.UtcNow);
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
            _logger.LogInformation("User {ID} added to the Database at {Time}", user.UserId, DateTime.UtcNow);
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
            _logger.LogInformation("Court {ID} added to the Database at {Time}", court.CourtId, DateTime.UtcNow);
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
            _logger.LogInformation("TimeSlots for Court {ID} added to the Database at {Time}", timeslot.CourtId, DateTime.UtcNow);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> DeleteArenaService(int id)
        {
            var arena = await _context.Arenas
                        .Include(c => c.Courts)
                            .ThenInclude(t => t.TimeSlots)
                        .FirstOrDefaultAsync(a => a.ArenaId == id);

            if (arena == null)
                return false;

            //Deleting the timeslot first 
            foreach(var court in arena.Courts)
            {
                _context.TimeSlots.RemoveRange(court.TimeSlots);
            }

             _context.Courts.RemoveRange(arena.Courts);
             _context.Arenas.Remove(arena);
            await _context.SaveChangesAsync();
            return true;
        }





        // Step 1: Create Booking
        public async Task<BookingResponseDTO> CreateBookingAsync(AddBookingDTO dto)
        {
            var courtExists = await _context.Courts.AnyAsync(c => c.CourtId == dto.CourtId);
            if (!courtExists)
                throw new KeyNotFoundException("Court not found");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
            if (!userExists)
                throw new KeyNotFoundException("User not found");

       
            var slots = await _context.TimeSlots
                    .Where(s => dto.SlotIds.Contains(s.TimeSlotId) && s.IsAvailable)
                    .ToListAsync();

            if (slots.Count != dto.SlotIds.Count)
                throw new BadHttpRequestException("One or more slots are unavailable");


            var booking = new Booking
            {
                UserId = dto.UserId,
                CourtId = dto.CourtId,
                StartTime = slots.Min(s => s.Date.Add(s.StartTime)),
                EndTime = slots.Max(s => s.Date.Add(s.EndTime)),
                BookingStatus = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            foreach (var slot in slots)
            {
                slot.BookingId = booking.BookingId;
                slot.IsAvailable = false; 
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Booking {BookingId} created for user{UserId} at {Time}", booking.BookingId, booking.UserId, DateTime.UtcNow);

            var dto = new BookingResponseDTO
            {
                Message = "Booking Pending",
                BookingId = booking.BookingId,
                UserId = booking.UserId,
                CourtId = booking.CourtId
            };

            return dto;
        }



        public async Task<bool> MakePaymentAsync(MakePaymentDTO dto)
        {

            var booking = await _context.Bookings
                .Include(b => b.TimeSlots)
                .FirstOrDefaultAsync(b => b.BookingId == dto.BookingId && b.UserId == dto.UserId);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            if (booking.BookingStatus == "Booked")
                throw new InvalidOperationException("Booking is already confirmed\nPayment has been already done");

            var totalAmount = booking.TimeSlots.Sum(s => s.Price);

            var payment = new Payment
            {
                BookingId = booking.BookingId,
                UserId = booking.UserId,
                Amount = totalAmount,
                PaymentStatus = "Paid",
                PaymentDate = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            booking.BookingStatus = "Booked";
            await _context.SaveChangesAsync();

            _logger.LogInformation("Payment {PaymentId} made for Booking {BookingId} at {Time}", payment.PaymentId, booking.BookingId, DateTime.UtcNow);

            return true;
        }



    }
}
