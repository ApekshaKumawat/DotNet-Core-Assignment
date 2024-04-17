using Microsoft.EntityFrameworkCore;
using UserApplication.Models;
using UserApplication.Models.Response;
using UserApplication.Repository;

namespace UserApplication.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public ResponseTemplate<bool> Register(SignUp signUp)
        {

            ResponseTemplate<bool> responseTemplate = new ResponseTemplate<bool>();
            try
            {
                var result = _userRepository.AddUser(new User { UserName = signUp.UserName, Email = signUp.Email, Password = signUp.Password });
                responseTemplate.Data = result > 0;
            }
            catch (Exception ex)
            {
                responseTemplate.Errors = new List<Models.Response.Error> { new Models.Response.Error { Code = "RUEX", Message = ex.Message } };
            }
            return responseTemplate;
        }

        public async Task<User> Login(string Email, string Password)
        {
            return await _userRepository.AuthenticateUser(Email, Password);
        }

        public async Task<User> GetUserById(int Id)
        {
            return await _userRepository.GetUser(Id);
        }
    }
}
