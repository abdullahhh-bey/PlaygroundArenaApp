using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaygroundArenaApp.Application.Services;
using PlaygroundArenaApp.Core.DTO;

namespace PlaygroundArenaApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminControlController : ControllerBase
    {
        private readonly AdminArenaService _adminservice;

        public AdminControlController(AdminArenaService adminservice)
        {
            _adminservice = adminservice;
        }


        //Add User
        [HttpPost("users")]
        public async Task<IActionResult> CreateUserAPI( AddUserDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Invalid null value!");

            var check = await _adminservice.CreateUserService(dto);
            if (!check)
                throw new BadHttpRequestException("Something went wrong!");

            return Ok("User Created");
        }


        [HttpPost("arenas")]
        public async Task<IActionResult> CreateArenaAPI(AddArenaDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Invalid null value!");

            var check = await _adminservice.CreateArenaService(dto);
            return Ok("Arena Created");
        }



        [HttpPost("courts")]
        public async Task<IActionResult> CreateCourtAPI( AddCourtByArenaIdDTO dto )
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Invalid null value!");

            var check = await _adminservice.CreateCourtService(dto);
            if (!check)
                throw new KeyNotFoundException("Arena doesn't exist");

            return Ok($"Court for Arena:{dto.ArenaId} Created");
        }



        [HttpPost("courts/id/slots")]
        public async Task<IActionResult> CreateCourtTimeSlotsAPI([FromBody] AddTimeSlotsByCourtIdDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Invalid null value!");

            var check = await _adminservice.CreateCourtTimeSlotsService(dto);
            if (!check)
                throw new KeyNotFoundException("Court doesn't exist");

            return Ok($"Time slot for Court:{dto.CourtId} created");
        }




        [HttpDelete("arenas/{id}")]
        public async Task<IActionResult> DeleteArenaById(int id)
        {
            var check = await _adminservice.DeleteArenaService(id);
            if (!check)
                throw new KeyNotFoundException("Arena dont exist");

            return NoContent();
        }




        [HttpPost("bookings")]
        public async Task<IActionResult> AddBookingAPI( AddBookingDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Incomplete Information");

            var Bookingdto = await _adminservice.CreateBookingAsync(dto);
            return Ok(Bookingdto);
        }




        [HttpPost("payments")]
        public async Task<IActionResult> MakePaymentAPI(MakePaymentDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Incomplete Information");

            var payment = await _adminservice.MakePaymentAsync(dto);
            return Ok("Payment has been received!\nBooking Confirmed");
        }



        [HttpPost("courts/id/timeslots")]
        public async Task<IActionResult> AddSlotsByTimeRangeAPI(AddSlotsEWithTimeRangeDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Incomplete Information");

            var check = await _adminservice.CreateSlotsWithRangeService(dto);
            return Ok($"TimeSlot from {dto.Start} to {dto.End} CREATED for Court:{dto.CourtId}");
        }




        [HttpPost("courts/rules")]
        public async Task<IActionResult> CreateCourtRulesAPI( AddCourtRulesDTO dto)
        {
            if(!ModelState.IsValid)
                throw new ArgumentNullException("Incomplete Information");

            var check = await _adminservice.CreateCourtRulesService(dto);
            return Ok("Court Rules Created!");
        }



    }
}
