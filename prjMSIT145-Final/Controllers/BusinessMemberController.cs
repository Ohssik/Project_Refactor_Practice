﻿using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;
using System.Text.Json;

namespace prjMSIT145_Final.Controllers
{
    public class BusinessMemberController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public BusinessMemberController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
        public IActionResult Blogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Blogin(CLoginViewModel cLoginViewModel)
        {
            //帳戶帳號密碼確認
            BusinessMember b= _context.BusinessMembers.FirstOrDefault(b=>b.Email.Equals(cLoginViewModel.fEmail)&&b.Password.Equals(cLoginViewModel.fPassword));
            if (b != null)
            {
                if (b.Email.Equals(cLoginViewModel.fEmail) && b.Password.Equals(cLoginViewModel.fPassword))
                {
                    string json = JsonSerializer.Serialize(b);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                    return RedirectToAction("BList", "Order");
                }
            }
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }
        
    }
}