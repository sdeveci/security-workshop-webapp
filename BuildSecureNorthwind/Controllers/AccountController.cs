using BuildSecureNorthwind.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BuildSecureNorthwind.Controllers
{
    public class AccountController : Controller
    {
        private readonly NorthwindContext _context;
        private readonly ILogger<AccountController> _logger;
        public AccountController(NorthwindContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult AccessDenied()
        {
            _logger.LogCritical($"{HttpContext.User.Identity.Name} kullanıcısı yetkili olmayan işlem gerçekleştirmeye çalıştı");
            return Content("Bu işlemi gerçekleştirmeye yetkiniz bulunmamaktadır");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string mail, string password, string returnUrl)
        {
            var user = _context.Users.SingleOrDefault(t => t.Mail == mail && t.Password == password);

            if(user == null)
            {
                ViewBag.Message = "Kullanıcı adı veya şifre hatalı";
                return Login();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Mail),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Mail),
                new Claim(ClaimTypes.GivenName,$"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role,"admin"),
                new Claim(ClaimTypes.Role,"moderator"),
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrinciple = new ClaimsPrincipal(claimIdentity);

            await HttpContext.SignInAsync(claimPrinciple);

            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Product");

            return LocalRedirect(returnUrl);
        }

        
    }
}
