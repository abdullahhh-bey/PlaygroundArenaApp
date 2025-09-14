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
            if(!ModelState.IsValid)
                return BadRequest("Incomplete Information");

            var check = await _adminservice.CreateUserService(dto);
            if (!check)
                return BadRequest("Something went wrong");

            return Ok("User Created");
        }


        [HttpPost("add-arena")]
        public async Task<IActionResult> CreateArenaAPI(AddArenaDTO dto)
        {
            if (!ModelState.IsValid) 
                return BadRequest("Incomplete Information");

            var check = await _adminservice.CreateArenaService(dto);
            return Ok("Arena Created");
        }


    }
}
