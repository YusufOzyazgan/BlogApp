using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (isUser != null)
                {
                    var userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser.imageUrl ?? ""));


                    if (isUser.Email == "yucufer@gmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    // Eğer varsa kayıtlı cookie'yi siler
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    //Yeni cookie eklemek için
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                        );
                    return RedirectToAction("Index", "Post");

                }
                else
                {
                    ModelState.AddModelError("", "Email Veya Parolar Hatalı!");
                }
            }


            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user =await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
                if (user != null)
                {
                    ModelState.AddModelError("", "Username is already registered, please change it!");
                    return View(model);
                }
                user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
                if (user != null)
                {
                    ModelState.AddModelError("", "Email is already registered, please check it!");
                    return View(model);
                }
                var userModel = new User()
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Password = model.Password,
                    Email = model.Email,
                    imageUrl = "noUser2.jpg"
                };
                
                _userRepository.CreateUser(userModel);
                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}
