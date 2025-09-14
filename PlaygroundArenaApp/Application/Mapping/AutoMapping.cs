using AutoMapper;
using PlaygroundArenaApp.Core.DTO;
using PlaygroundArenaApp.Core.Models;

namespace PlaygroundArenaApp.Application.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {

            CreateMap<Arena , GetArenaDTO>();
            CreateMap<User, UsersDTO>();
            CreateMap<TimeSlot , TimeSlotsDTO>();
            CreateMap<Court , CourtDetailsDTO>();
        }
    }
}
