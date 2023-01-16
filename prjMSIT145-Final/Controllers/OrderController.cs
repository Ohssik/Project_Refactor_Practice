using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;

namespace prjMSIT145_Final.Controllers
{
    public class OrderController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public OrderController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var o = _context.Orders.FirstOrDefault();
            return View();
        }
    }
}
