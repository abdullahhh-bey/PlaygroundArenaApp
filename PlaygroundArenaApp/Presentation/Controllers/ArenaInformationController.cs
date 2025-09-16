using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaygroundArenaApp.Application.Services;

namespace PlaygroundArenaApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArenaInformationController : ControllerBase
    {
        private readonly ArenaInformationService _arenaService;
        public ArenaInformationController(ArenaInformationService arenaService)
        {
            _arenaService = arenaService;
        }



        [HttpGet("arena-info")]
        public async Task<IActionResult> GetArenasAPI()
        {
            var arenas = await _arenaService.GetArenasService();
            if (arenas.Count == 0)
                throw new KeyNotFoundException("Arena doesn't exist");

            return Ok(arenas);
        }


        [HttpGet("user-info")]
        public async Task<IActionResult> GetUsersAPI()
        {
            var users = await _arenaService.GetUsersService();
            if (users.Count == 0)
                throw new KeyNotFoundException("User doesn't exist");

            return Ok(users);
        }



        [HttpGet("court-info")]
        public async Task<IActionResult> GetCourtAPI()
        {
            var check = await _arenaService.GetCourtService();
            if (check.Count == 0)
                throw new KeyNotFoundException("Court doesn't exist");

            return Ok(check);
        }



        [HttpGet("court-info/{id}")]
        public async Task<IActionResult> GetCourtByIdAPI(int id)
        {
            var check = await _arenaService.GetCourtByIdService(id);
            if (check == null)
                throw new KeyNotFoundException("Court doesn't exist");

            return Ok(check);
        }




        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlotsAPI()
        {
            var slots = await _arenaService.GetAvailableTimeSlotsService();
            if (slots.Count == 0)
                throw new KeyNotFoundException("No Available Slots");

            return Ok(slots);
        }





        [HttpGet("time-slots")]
        public async Task<IActionResult> GetTimeSlotsAPI()
        {
            var slots = await _arenaService.GetTimeSlotsService();
            if (slots.Count == 0)
                throw new KeyNotFoundException("Slot doesn't exist");

            return Ok(slots);
        }



        [HttpGet("time-slots-by-court-id/{id}")]
        public async Task<IActionResult> GetTimeSlotsByCourtIdAPI(int id)
        {
            var CourtSlot = await _arenaService.GetCourtWithSlotsService(id);
            if (CourtSlot.TimeSlots.Count == 0)
                return Ok(new
                {
                    court = CourtSlot,
                    slots = "This Court has no slots yet"
                });

            return Ok(CourtSlot);
        }




        [HttpGet("courts-by-arenaid/{id}")]
        public async Task<IActionResult> GetArenaWithCourtsAPI(int id)
        {
            var check = await _arenaService.GetArenaWithCourtService(id);
           
            return Ok(check);
        }




        [HttpGet("get-bookings")]
        public async Task<IActionResult> GetBookingsAPI()
        {
            var bookings = await _arenaService.GetBookingsService();
            if (bookings.Count == 0)
                throw new KeyNotFoundException("No Bookings");

            return Ok(bookings);
        }



    }
}
