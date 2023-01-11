﻿using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;

namespace prjMSIT145_Final.Controllers
{
    public class BusinessMemberController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public BusinessMemberController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
