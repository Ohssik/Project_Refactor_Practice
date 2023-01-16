using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using System.Text.Json;

namespace prjMSIT145_Final.Controllers
{
    public class AddMaterialController : Controller
    {
        ispanMsit145shibaContext _context;
        public AddMaterialController(ispanMsit145shibaContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            return View();
        }
    }
}
