using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaygroundArenaApp.Application.Services;

namespace PlaygroundArenaApp.Presentation.Controllers
{
    [Route("admin/[controller]")]
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





    }
}
