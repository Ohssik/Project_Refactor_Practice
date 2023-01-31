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
        public IActionResult CList(int? NFid)
        {
            if(NFid==null)
                return Redirect("/CustomerMember/Login");
            return View();
        }
    }
}
