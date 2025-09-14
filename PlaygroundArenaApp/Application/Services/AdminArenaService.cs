using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.DTO;
using PlaygroundArenaApp.Core.Models;
using PlaygroundArenaApp.Infrastructure.Data;
using System.Web.Mvc;

namespace PlaygroundArenaApp.Application.Services
{
    public class AdminArenaService
    {
        private readonly PlaygroundArenaDbContext _context;

        public AdminArenaService(PlaygroundArenaDbContext context)
        {
            _context = context;
        }



        //All write/add services


        public async Task<bool> CreateArenaService( AddArenaDTO dto )
        {
            // I allowed same emails for multiple arenas 
            //so that If someone has arenas in multiple cities
            //he can manage all of them with single email

            var arena = new Arena
            {
                Name = dto.Name,
                Email = dto.Email,
                Location = dto.Location
            };

            await _context.Arenas.AddAsync(arena);

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> CreateUserService( AddUserDTO dto)
        {
            //User cannot have same Email or same Phone Numbers
            var checkEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (checkEmail != null)
                throw new BadHttpRequestException("Email already in use!");

            var checkPhone = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
            if (checkPhone != null)
                throw new BadHttpRequestException("Phone Number already in use!");

            var user = new User
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }





    }
}
