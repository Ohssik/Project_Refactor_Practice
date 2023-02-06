using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using System.Runtime.CompilerServices;

namespace prjMSIT145_Final.Controllers
{
    public class ChatRoomController : Controller
    {

        
        public IActionResult Index()
        {
            return View();
        }
        
       
    }
}
