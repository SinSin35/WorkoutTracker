using UserService.Models;

namespace UserService.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUserByIdAsync(int id);

        public Task<List<User>> GetAllUsersAsync();

        public Task<User> AddUserAsync(User user);
    }
}
