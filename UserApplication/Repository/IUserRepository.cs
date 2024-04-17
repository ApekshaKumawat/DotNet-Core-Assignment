using UserApplication.Models;

namespace UserApplication.Repository
{
    public interface IUserRepository
    {
        public int AddUser(User user);

        public Task<User> AuthenticateUser(string Email, string Password);
        public Task<User> GetUser(int Id);
    }
}
