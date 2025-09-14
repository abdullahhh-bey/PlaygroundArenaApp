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



        public async Task<List<TimeSlotsDTO>> GetTimeSlotsByCourtIdService()
        {
            var timeSlots =  await _context.TimeSlots.ToListAsync();
            if (timeSlots.Count == 0)
                return new List<TimeSlotsDTO>();

            var dtos = _mapper.Map<List<TimeSlotsDTO>>(timeSlots);
            return dtos;
        }


    }
}
