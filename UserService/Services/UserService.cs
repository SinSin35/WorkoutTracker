using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Interfaces;
using UserService.Models;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(UserContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<User> AddUserAsync(RegistrationModel model) {

            User user = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                throw new Exception("Error at user creation");
            }

            return user;
        }

        public async Task<User?> GetUserByIdAsync(Guid id) => await _context.Users.FindAsync(id);

        public async Task<List<User>> GetAllUsersAsync() => await _context.Users.ToListAsync();
    }
}
