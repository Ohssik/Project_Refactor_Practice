using Microsoft.AspNetCore.Mvc;
using prjMSIT145Final.Infrastructure.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;
using prjMSIT145Final.Web.ViewModel;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using System.Collections.Generic;
using prjMSIT145Final.Web.ViewModels;
using System.Data;

namespace prjMSIT145Final.Web.Controllers
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
