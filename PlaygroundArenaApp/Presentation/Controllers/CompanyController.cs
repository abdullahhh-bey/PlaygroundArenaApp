using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaygroundArenaApp.Core.Models;
using PlaygroundArenaApp.Infrastructure.Data;

namespace PlaygroundArenaApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyDbContext _context;

        public CompanyController(CompanyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            return Ok(_context.Employees.ToList());
        }

        [HttpPost]
        public IActionResult AddEmployee(Employee emp)
        {
            _context.Employees.Add(emp);
            _context.SaveChanges();
            return Ok(emp);
        }
    }
}
