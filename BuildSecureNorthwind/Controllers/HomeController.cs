using BuildSecureNorthwind.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BuildSecureNorthwind.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var message = "Üzgünüz, Sistemde beklenmedik bir hata oluştu";

            var error = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (error != null && error.Error is BusinessRuleException)
                message = error.Error.Message;

            _logger.LogError(message, error?.Error);

            return View(new ErrorViewModel
            {
                Message = message
            });
        }
    }
}