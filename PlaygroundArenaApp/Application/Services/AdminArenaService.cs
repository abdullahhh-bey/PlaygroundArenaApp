using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.DTO;
using PlaygroundArenaApp.Core.Models;
using PlaygroundArenaApp.Infrastructure.Data;
using PlaygroundArenaApp.Infrastructure.Repository.UOW;

namespace PlaygroundArenaApp.Application.Services
{
    public class AdminArenaService
    {
        private readonly IUnitOfWork _unit;
        private readonly PlaygroundArenaDbContext _context;
        private readonly ILogger<AdminArenaService> _logger;
        private readonly IMapper _mapper;
        public AdminArenaService(IUnitOfWork unit, PlaygroundArenaDbContext context , ILogger<AdminArenaService> logger, IMapper mapper)
        {
            _unit = unit;
            _context = context;
            _logger = logger;
            _mapper = mapper;
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

            await _unit.Arena.AddArena(arena);
            _logger.LogInformation("Arena {ID} added to the Database at {Time}", arena.ArenaId , DateTime.UtcNow);
            await _unit.SaveAsync();

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
            var check = await _unit.Arena.GetArenaById(dto.ArenaId);
            if (check == null)
                return false;

            var courtCheck = await _unit.Court.CheckCourtByNameAndArenaId(dto.Name, dto.ArenaId);
            if (courtCheck)
                throw new BadHttpRequestException("Court with this name already exist");

            var court = new Court
            {
                ArenaId = dto.ArenaId,
                Name = dto.Name,
                CourtType = dto.CourtType
            };

            await _unit.Court.AddCourtAsync(court);
            _logger.LogInformation("Court {ID} added to the Database at {Time}", court.CourtId, DateTime.UtcNow);
            await _unit.SaveAsync();
            return true;
        }


        public async Task<bool> CreateCourtTimeSlotsService(AddTimeSlotsByCourtIdDTO dto)
        {
            var check = await _unit.Court.IsCourtExists(dto.CourtId);
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
            var arena = await _unit.Arena.GetArenaByIdWithCourtAndSlots(id);
            if (arena == null)
                return false;

            //Deleting the timeslot first 
            foreach(var court in arena.Courts)
            {
                _context.TimeSlots.RemoveRange(court.TimeSlots);
            }

            await _unit.Court.DeleteCourtWithSlots(arena.Courts);
            await _unit.Arena.DeleteArena(arena);
            await _unit.SaveAsync();
            return true;
        }





        public async Task<BookingResponseDTO> CreateBookingAsync(AddBookingDTO d)
        {
            var courtExists = await _unit.Court.IsCourtExists(d.CourtId);
            if (!courtExists)
                throw new KeyNotFoundException("Court not found");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == d.UserId);
            if (!userExists)
                throw new KeyNotFoundException("User not found");

            var courtRules = await _context.CourtRules.FirstOrDefaultAsync(r => r.CourtId == d.CourtId);
            if (courtRules == null)
                throw new BadHttpRequestException("Court rules not here");

            var slots = await _context.TimeSlots
                .Where(s => d.TimeSlotId.Contains(s.TimeSlotId) && s.IsAvailable && s.CourtId == d.CourtId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();

            if (slots.Count == 0)
                throw new BadHttpRequestException($"Invalid slots for Court: {d.CourtId}");

            if (slots.Count != d.TimeSlotId.Count)
                throw new BadHttpRequestException("One or more slots are unavailable");

            if (slots.Count < courtRules.MinimumSlotsBooking)
                throw new BadHttpRequestException($"You must book at least {courtRules.MinimumSlotsBooking} slots");

            //gpt
            var interval = TimeSpan.FromMinutes(courtRules.TimeInterval);
            for (int i = 0; i < slots.Count - 1; i++)
            {
                if (slots[i + 1].StartTime - slots[i].EndTime != TimeSpan.Zero)
                    throw new BadHttpRequestException("Slots must be continuous without gaps");
            }

            var booking = new Booking
            {
                UserId = d.UserId,
                CourtId = d.CourtId,
                StartTime = slots.First().StartTime,
                EndTime = slots.Last().EndTime,
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

            _logger.LogInformation("Booking {BookingId} created for user {UserId} at {Time}",
                booking.BookingId, booking.UserId, DateTime.UtcNow);

            return new BookingResponseDTO
            {
                Message = "Booking Pending",
                BookingId = booking.BookingId,
                UserId = booking.UserId,
                CourtId = booking.CourtId
            };
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




        public async Task<bool> CreateSlotsWithRangeService( AddSlotsEWithTimeRangeDTO dto )
        {      
            if (dto.Date.Date < DateTime.UtcNow.Date)
                throw new BadHttpRequestException("Date cannot be in the past");

            if (dto.Start >= dto.End || dto.Start < 0 || dto.End > 24)
                throw new BadHttpRequestException("Invalid start/end time");

            if (!await _unit.Court.IsCourtExists(dto.CourtId))
                throw new BadHttpRequestException("Court not found");

            var rules = await _context.CourtRules.FirstOrDefaultAsync(r => r.CourtId == dto.CourtId);
            if (rules == null)
                throw new BadHttpRequestException("Court rules not defined");


            var existingSlots = await _context.TimeSlots
                .Where(s => s.CourtId == dto.CourtId && s.Date.Date == dto.Date.Date)
                .ToListAsync();


            var slots = new List<TimeSlot>();

            var start = TimeSpan.FromHours(dto.Start);
            var end = TimeSpan.FromHours(dto.End);


            //gpt
            for (var t = start; t < end; t = t.Add(TimeSpan.FromMinutes(rules.TimeInterval)))
            {
                var slotEnd = t.Add(TimeSpan.FromMinutes(rules.TimeInterval));
                if (slotEnd > end) break;

                if (existingSlots.Any(s => s.StartTime < slotEnd && s.EndTime > t))
                    continue;

                slots.Add(new TimeSlot
                {
                    CourtId = dto.CourtId,
                    Date = dto.Date.Date,
                    StartTime = t,
                    EndTime = slotEnd,
                    Price = dto.Price,
                    IsAvailable = true
                });
            }

            if (!slots.Any())
                throw new BadHttpRequestException("No valid slots to add");

            await _context.TimeSlots.AddRangeAsync(slots);
            await _context.SaveChangesAsync();
            return true;
        }     


        public async Task<bool> CreateCourtRulesService(AddCourtRulesDTO dto)
        {
            var check = await _unit.Court.GetCourtById(dto.CourtId);
            if (check == null)
                throw new KeyNotFoundException("Court don't exist");
            
            if(dto.MinimumSlotsBooking < 1)
            {
                throw new BadHttpRequestException("Invalid Minimum Booking");
            }

            var court = _mapper.Map<CourtRules>(dto);
            await _context.CourtRules.AddAsync(court);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
