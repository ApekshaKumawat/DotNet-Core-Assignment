using Microsoft.EntityFrameworkCore;
using UserApplication.Models;

namespace UserApplication.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int AddUser(User user)
        {
            int result;

            if (!_context.Users.Any(x => x.Email == user.Email))
            {
                user.UserId = _context.Users.Count() + 1;
                _context.Users.Add(user);
                result = _context.SaveChanges();
            }
            else
                throw new Exception("User already exist");

            return result;
        }


        public async Task<User> AuthenticateUser(string Email, string Password)
        {
             return await _context.Users.SingleOrDefaultAsync(m => m.Email == Email && m.Password == Password);         
        }

        public async Task<User> GetUser(int Id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == Id);
        }
    }
}
