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
        private readonly ILogger<ArenaInformationController> _logger;

        public ArenaInformationController(ArenaInformationService arenaService, ILogger<ArenaInformationController> logger)
        {
            _arenaService = arenaService;
            _logger = logger;
        }


        [HttpGet("arena-info")]
        public async Task<IActionResult> GetArenasAPI()
        {
            var arenas = await _arenaService.GetArenasService();
            if (arenas.Count == 0)
                return NotFound("No Arena Registered");

            return Ok(arenas);
        }


        [HttpGet("user-info")]
        public async Task<IActionResult> GetUsersAPI()
        {
            var users = await _arenaService.GetUsersService();
            if (users.Count == 0)
                return NotFound("No Users Registered");

            return Ok(users);
        }


    }
}
