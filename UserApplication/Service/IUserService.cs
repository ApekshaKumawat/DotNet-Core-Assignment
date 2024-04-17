using UserApplication.Models;
using UserApplication.Models.Response;

namespace UserApplication.Service
{
    public interface IUserService
    {
        ResponseTemplate<bool> Register(SignUp signUp);

        public Task<User> Login(string Email, string Password);
        public Task<User> GetUserById(int Id);
    }
}
