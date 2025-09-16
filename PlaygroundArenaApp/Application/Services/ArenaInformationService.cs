using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.DTO;
using PlaygroundArenaApp.Infrastructure.Data;

namespace PlaygroundArenaApp.Application.Services
{
    public class ArenaInformationService
    {
        private readonly PlaygroundArenaDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ArenaInformationService> _logger;

        public ArenaInformationService(PlaygroundArenaDbContext context, IMapper mapper, ILogger<ArenaInformationService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        //All get services
        public async Task<List<GetArenaDTO>> GetArenasService()
        {
            var arenas = await _context.Arenas.ToListAsync();
            if (arenas.Count == 0)
                return new List<GetArenaDTO>();

            var arenasDTO = _mapper.Map<List<GetArenaDTO>>(arenas);
            _logger.LogInformation("Getting All Arenas Details at {Time}", DateTime.UtcNow);
            return arenasDTO;
        }


        public async Task<List<UsersDTO>> GetUsersService()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Count == 0)
                return new List<UsersDTO>();

            var usersDTO = _mapper.Map<List<UsersDTO>>(users);
            _logger.LogInformation("Getting All Users Details at {Time}", DateTime.UtcNow);
            return usersDTO;
        }


        public async Task<List<CourtDetailsDTO>> GetCourtService()
        {
            var check = await _context.Courts.ToListAsync();
            if(check.Count == 0)
                return new List<CourtDetailsDTO>();

            var dto = _mapper.Map<List<CourtDetailsDTO>>(check);
            _logger.LogInformation("Getting All Courts Details at {Time}", DateTime.UtcNow);
            return dto;
        }


        public async Task<CourtDetailsDTO> GetCourtByIdService(int id)
        {
            var check = await _context.Courts.FindAsync(id);
            if (check == null)
                throw new KeyNotFoundException($"Court with Id:{id} not found");

            var dto = _mapper.Map<CourtDetailsDTO>(check);
            _logger.LogInformation("Getting Court {Id} Details at {Time}", dto.CourtId ,DateTime.UtcNow);
            return dto;
        }


        public async Task<List<TimeSlotsDTO>> GetTimeSlotsService()
        {
            var timeSlots =  await _context.TimeSlots.ToListAsync();
            if (timeSlots.Count == 0)
                return new List<TimeSlotsDTO>();

            var dtos = _mapper.Map<List<TimeSlotsDTO>>(timeSlots);
            _logger.LogInformation("Getting All TimeSlots Details at {Time}", DateTime.UtcNow);
            return dtos;
        }


        public async Task<List<TimeSlotsDTO>> GetAvailableTimeSlotsService()
        {
            var timeSlots = await _context.TimeSlots
                        .Where(t => t.IsAvailable == true)
                        .OrderBy(t => t.Date)
                        .ToListAsync();

            if (timeSlots.Count == 0)
                return new List<TimeSlotsDTO>();

            var slotsDto = _mapper.Map<List<TimeSlotsDTO>>(timeSlots);
            _logger.LogInformation("Getting All Available TimeSlots Details at {Time}", DateTime.UtcNow);
            return slotsDto;
        }


        public async Task<CourtWithTimeSlotsDTO> GetCourtWithSlotsService(int id)
        {
            var courtTimeSlots = await _context.Courts
                                .Include(t =>  t.TimeSlots)
                                .FirstOrDefaultAsync(c => c.CourtId == id);

            if (courtTimeSlots == null)
                throw new KeyNotFoundException("Court dont exist");

            var Courtdto = new CourtWithTimeSlotsDTO
            {
                CourtId = courtTimeSlots.CourtId,
                Name = courtTimeSlots.Name,
                Type = courtTimeSlots.CourtType,
                TimeSlots = courtTimeSlots.TimeSlots.Select(t =>
                new TimeSlotsDTO
                {
                    TimeSlotId = t.TimeSlotId,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    IsAvailable = t.IsAvailable,
                    Price = t.Price,
                    CourtId = t.CourtId,
                    Date = t.Date
                })
                .ToList()
            };

            _logger.LogInformation("Getting Court {Id} Time Slot Details at {Time}", Courtdto.CourtId ,DateTime.UtcNow);

            return Courtdto;
        }



        public async Task<ArenaDetailsDTO> GetArenaWithCourtService(int id)
        {
            var arenaCourt = await _context.Arenas
                            .Include(a => a.Courts)
                            .FirstOrDefaultAsync(a => a.ArenaId == id);

            if (arenaCourt == null)
                throw new KeyNotFoundException("Arena not found");

            var arenaDTO = new ArenaDetailsDTO
            {
                ArenaId = arenaCourt.ArenaId,
                Name = arenaCourt.Name,
                Location = arenaCourt.Location,
                CourtDetails = arenaCourt.Courts.Select(c =>
                    new CourtDetailsDTO
                    {
                        CourtId = c.CourtId,
                        Name = c.Name,
                        CourtType = c.CourtType
                    }
                ).ToList()
            };

            _logger.LogInformation("Getting Arena {Id} with all of its Court Details at {Time}", arenaDTO.ArenaId ,DateTime.UtcNow);
            return arenaDTO;
        }




        public async Task<List<GetBookingDetailsDTO>> GetBookingsService()
        {
            var bookings = await _context.Bookings
                            .Include(s => s.TimeSlots)
                            .Include(p => p.Payment)
                            .ToListAsync();


            if (bookings.Count == 0)
                return new List<GetBookingDetailsDTO>();


            var dto = bookings
                    .Select(b => new GetBookingDetailsDTO
                    {
                        BookingId = b.BookingId,
                        UserId = b.UserId,
                        CourtId = b.CourtId,
                        BookingDate = b.BookingDate,
                        BookingStatus = b.BookingStatus,
                        TimeSlots = b.TimeSlots.Select(t => new TimeSlotsDTO
                        {
                            TimeSlotId = t.TimeSlotId,
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            IsAvailable = t.IsAvailable,
                            Date = t.Date,
                            Price = t.Price,
                            CourtId = t.CourtId
                        }).ToList(),
                        Payments = b.Payment == null ? null : new PaymentDTO
                        {
                            PaymentId = b.Payment.PaymentId,
                            Amount = b.Payment.Amount,
                            PaymentStatus = b.Payment.PaymentStatus
                        }
                    }).ToList();

            return dto;
        }



        


    }
}
