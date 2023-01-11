using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using System.Diagnostics;

namespace prjMSIT145_Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ispanMsit145shibaContext _context;
        public HomeController(ILogger<HomeController> logger, ispanMsit145shibaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}