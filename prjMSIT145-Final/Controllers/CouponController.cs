using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;

namespace prjMSIT145_Final.Controllers
{
    public class CouponController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public CouponController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {           
            return View();
        }
    }
}
