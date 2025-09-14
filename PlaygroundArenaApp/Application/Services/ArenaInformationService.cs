using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.DTO;
using PlaygroundArenaApp.Infrastructure.Data;

namespace PlaygroundArenaApp.Application.Services
{
    public class ArenaInformationService
    {
        private readonly PlaygroundArenaDbContext _context;
        private readonly IMapper _mapper;

        public ArenaInformationService(PlaygroundArenaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //All get services
        public async Task<List<GetArenaDTO>> GetArenasService()
        {
            var arenas = await _context.Arenas.ToListAsync();
            if (arenas.Count == 0)
                return new List<GetArenaDTO>();

            var arenasDTO = _mapper.Map<List<GetArenaDTO>>(arenas);
            return arenasDTO;
        }


        public async Task<List<UsersDTO>> GetUsersService()
        {
            var users = await _context.Users.ToListAsync();
            if (users.Count == 0)
                return new List<UsersDTO>();

            var usersDTO = _mapper.Map<List<UsersDTO>>(users);
            return usersDTO;
        }


        public async Task<List<CourtDetailsDTO>> GetCourtService()
        {
            var check = await _context.Courts.ToListAsync();
            if(check.Count == 0)
                return new List<CourtDetailsDTO>();

            var dto = _mapper.Map<List<CourtDetailsDTO>>(check);
            return dto;
        }


        public async Task<CourtDetailsDTO> GetCourtByIdService(int id)
        {
            var check = await _context.Courts.FindAsync(id);
            if (check == null)
                throw new KeyNotFoundException($"Court with Id:{id} not found");

            var dto = _mapper.Map<CourtDetailsDTO>(check);
            return dto;
        }


        public async Task<List<TimeSlotsDTO>> GetTimeSlotsService()
        {
            var timeSlots =  await _context.TimeSlots.ToListAsync();
            if (timeSlots.Count == 0)
                return new List<TimeSlotsDTO>();

            var dtos = _mapper.Map<List<TimeSlotsDTO>>(timeSlots);
            return dtos;
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

            return arenaDTO;
        }


    }
}
