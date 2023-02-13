using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;
using prjMSIT145_Final.ViewModel;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using System.Collections.Generic;
using prjMSIT145_Final.ViewModels;
using System.Data;

namespace prjMSIT145_Final.Controllers
{
    public class ChatRoomController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public ChatRoomController(ispanMsit145shibaContext context)
        {
            
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        //尋找MEMBER
        public IActionResult ChatUserChangeApi(int?fid)
        {
            NormalMember n = _context.NormalMembers.FirstOrDefault(n => n.Fid == fid);
            return Json(n);
            
           
        }

    }
}
