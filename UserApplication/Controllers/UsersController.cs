using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserApplication.Models;
using UserApplication.Service;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Web;


namespace UserApplication.Controllers
{

    public class UsersController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly HttpContextAccessor _httpContextAccessor;


        private readonly IUserService _userService;
        public UsersController(IUserService userService,ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignUp(SignUp request)
        {
            if (ModelState.IsValid)
            {
                var response = _userService.Register(new Models.SignUp
                { UserName = request.UserName, Email = request.Email, Password = request.Password });

                if (response?.Data == true)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var e in response.Errors)
                        ModelState.AddModelError("", e.Message);
                    return View(request);
                }
            }
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard(User user)
        {
            return View(user);
        }

        public IActionResult UpdateMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var userdetails = await _userService.Login(model.Email, model.Password);

                if (userdetails != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userdetails.UserId.ToString()),
                    new Claim(ClaimTypes.Email, userdetails.Email),
                    
                };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                        IsPersistent = true,
                        IssuedUtc = DateTimeOffset.UtcNow,
                        RedirectUri = "https://localhost:7295/UserSocialURLs/Create"

                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Dashboard", "Users");
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid login attempt.");
                    return View("Login");
                }    
            }
            else
            {
                return View("Login");
            }
            
        }
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind("OldPassword,NewPassword,ConfirmPassword")] ChangePassword model)
        {

            if (ModelState.IsValid)
            {
                var Id = @User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user = await _userService.GetUserById(int.Parse(Id));
                if (user == null)
                {
                    return NotFound();
                }

                if(model.OldPassword != user.Password)
                {
                    ModelState.AddModelError(string.Empty, "Current Password is incorrect.");
                    return View(model);
                }

                user.Password = model.NewPassword;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("UpdateMessage", "Users");
            }
            return View(model);
        }
            public async void Logout()
            {

            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Redirect("Login");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            if (ModelState.IsValid) {
                var UserId = @User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user = await _userService.GetUserById(int.Parse(UserId));

                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            return View();
           
        }

       
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile([Bind("UserId,UserName,Email,Contact,Address,Password")] User user)
        {

            if (ModelState.IsValid)
            {
                var UserId = @User.FindFirstValue(ClaimTypes.NameIdentifier);
                try
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("UpdateMessage","Users");
            }
            return View(user);
        }

        public async Task<IActionResult> ViewDetails()
        {
            var Id = @User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            User user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == int.Parse(Id));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
