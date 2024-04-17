using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserApplication.Models;
using UserApplication.Service;

namespace UserApplication.Controllers
{
    public class UserSocialURLsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        public UserSocialURLsController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        { 
            var userId = @User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await _context.Users.FindAsync(int.Parse(userId));
            var socialURLs = await _context.UserSocialURLs.Where(u => u.UserId == int.Parse(userId)).ToListAsync();
            var viewModel = new DashboardModel
            {
                User = user,
                SocialURLs = socialURLs
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSocialURLs = await _context.UserSocialURLs
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserSocialURLsId == id);
            if (userSocialURLs == null)
            {
                return NotFound();
            }

            return View(userSocialURLs);
        }


        public IActionResult Create()
        {
            var Id = @User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View();
        }

        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserSocialURLs userSocialURLs)
        {
            
            if (ModelState.IsValid){
                var Id = @User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user = await _userService.GetUserById(int.Parse(Id));
                userSocialURLs.UserId = int.Parse(Id);
                userSocialURLs.User = user;
                List<UserSocialURLs> urls = new List<UserSocialURLs>();
                urls.Add(userSocialURLs);
                user.SocialMediaProfiles= urls;
               
                _context.UserSocialURLs.Add(userSocialURLs);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index","UserSocialURLs");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSocialURLs = await _context.UserSocialURLs.FindAsync(id);
            if (userSocialURLs == null)
            {
                return NotFound();
            }
           
            return View(userSocialURLs);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserSocialURLsId,UserId,SocialMediaPlatformName,SocialMediaPlatformURL")] UserSocialURLs userSocialURLs)
        {
            if (id != userSocialURLs.UserSocialURLsId)
            {
                return NotFound();
            }

            
            if (ModelState.IsValid)
            {
                UserSocialURLs url = await _context.UserSocialURLs.FindAsync(id);
                url.SocialMediaPlatformURL = userSocialURLs.SocialMediaPlatformURL;
                _context.UserSocialURLs.Update(url);
                await _context.SaveChangesAsync();
                _context.Update(url);
                await _context.SaveChangesAsync();
  
               
                return RedirectToAction(nameof(Index));
            }

            return View(userSocialURLs);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userSocialURLs = await _context.UserSocialURLs
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserSocialURLsId == id);
            if (userSocialURLs == null)
            {
                return NotFound();
            }

            return View(userSocialURLs);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userSocialURLs = await _context.UserSocialURLs.FindAsync(id);
            if (userSocialURLs != null)
            {
                _context.UserSocialURLs.Remove(userSocialURLs);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserSocialURLsExists(int id)
        {
            return _context.UserSocialURLs.Any(e => e.UserSocialURLsId == id);
        }
    }
}
