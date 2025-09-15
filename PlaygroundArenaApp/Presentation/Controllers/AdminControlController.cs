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
        private readonly ILogger<AdminControlController> _logger;

        public AdminControlController(AdminArenaService adminservice , ILogger<AdminControlController> logger)
        {
            _adminservice = adminservice;
            _logger = logger;
        }


        //Add User
        [HttpPost("add-user")]
        public async Task<IActionResult> CreateUserAPI( AddUserDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Invalid null value!");

            var check = await _adminservice.CreateUserService(dto);
            if (!check)
                throw new BadHttpRequestException("Something went wrong!");

            return Ok("User Created");
        }


        [HttpPost("add-arena")]
        public async Task<IActionResult> CreateArenaAPI(AddArenaDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Invalid null value!");

            var check = await _adminservice.CreateArenaService(dto);
            return Ok("Arena Created");
        }



        [HttpPost("add-court")]
        public async Task<IActionResult> CreateCourtAPI( AddCourtByArenaIdDTO dto )
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Invalid null value!");

            var check = await _adminservice.CreateCourtService(dto);
            if (!check)
                throw new KeyNotFoundException("Arena doesn't exist");

            return Ok($"Court for Arena:{dto.ArenaId} Created");
        }



        [HttpPost("add-slots-by-courtid")]
        public async Task<IActionResult> CreateCourtTimeSlotsAPI(AddTimeSlotsByCourtIdDTO dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentNullException("Invalid null value!");

            var check = await _adminservice.CreateCourtTimeSlotsService(dto);
            if (!check)
                throw new KeyNotFoundException("Court doesn't exist");

            return Ok($"Time slot for Court:{dto.CourtId} created");
        }




    }
}
