using PlaygroundArenaApp.Infrastructure.Data;

namespace PlaygroundArenaApp.Application.Services
{
    public class AdminArenaService
    {
        private readonly PlaygroundArenaDbContext _context;
        private readonly ILogger<AdminArenaService> _logger;

        public AdminArenaService(PlaygroundArenaDbContext context , ILogger<AdminArenaService> logger)
        {
            _context = context;
            _logger = logger;
        }



        //All write/add services


    }
}
