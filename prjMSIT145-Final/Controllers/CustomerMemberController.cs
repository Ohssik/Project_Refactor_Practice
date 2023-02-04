﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Data.SqlClient;
using NuGet.Protocol;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text.Json;

namespace prjMSIT145_Final.Controllers
{
    public class CustomerMemberController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        private IWebHostEnvironment _eviroment;
        public CustomerMemberController(ispanMsit145shibaContext context, IWebHostEnvironment eviroment)
        {
            _context = context;
            _eviroment = eviroment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(CLoginViewModel vm)
        {


            NormalMember x = _context.NormalMembers.FirstOrDefault(t => t.Phone.Equals(vm.txtAccount) && t.Password.Equals(vm.txtPassword));
            if (x != null)
            {
                if (x.Password.Equals(vm.txtPassword) && x.Phone.Equals(vm.txtAccount))
                {
                    string json = JsonSerializer.Serialize(x);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                    //return RedirectToAction("Edit");
                    //return RedirectToAction("Index");
                    return Redirect("~/Home/CIndex");
                }
            }
            return View();
        }
        public ActionResult memberheadphoto()
        {
            

            return Json(CDictionary.SK_LOGINED_USER);
        }


        public ActionResult Loginout()
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGINED_USER);

            return Redirect("~/Home/CIndex");
            //return RedirectToAction("Index", "CustomerMember");
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Register(CNormalMemberViewModel vm, IFormFile photo)
        {
            var data = _context.NormalMembers.Select(c => c.Phone);
            foreach (var i in data)
            {
                if (i == vm.Phone || i == "")
                {
                    ViewBag.Name = vm.MemberName;
                    ViewBag.Phone = vm.Phone;
                    ViewBag.Email = vm.Email;
                    ViewBag.Gender= vm.Gender;
                    ViewBag.city = vm.AddressCity;
                    ViewBag.area = vm.AddressArea;
                    ViewBag.birthday = vm.Birthday;
                    ViewBag.MemberPhotoFile = vm.MemberPhotoFile;
                    ViewBag.password = vm.Password;
                    return View();
                   
                }
            }

            if (vm.Birthday==null)
            {
              return Content("請輸入生日");
            }
            
            if (photo != null)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(_eviroment.WebRootPath, "images", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
                vm.MemberPhotoFile = fileName;
            }
            
            _context.Add(vm.member);
            _context.SaveChanges();

            string smtpAddress = "smtp.gmail.com";
            //設定Port
            int portNumber = 587;
            bool enableSSL = true;
            //填入寄送方email和密碼
            string emailFrom = "a29816668@gmail.com";
            string emailpassword = "joksdquaswjdyzpu";
            //收信方email 可以用逗號區分多個收件人
            string emailTo = vm.Email;
            //主旨
            string subject = "註冊驗證信";
            //內容
            string body = $"https://localhost:7266/CustomerMember/Emailcheck/?Fid={vm.Fid}";
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                // 若你的內容是HTML格式，則為True
                mail.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, emailpassword);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }


            }
                return Redirect("~/Home/CIndex");
          
        }
        public IActionResult Verifyaccount(CNormalMemberViewModel vm)
        {
            var data = _context.NormalMembers.Select(c => c.Phone);
            foreach (var i in data)
            {
                if (i == vm.Phone || i == "")
                {
                    return Json("帳號重複");
                   
                }
            }
          return Json ("");
        }

        public IActionResult Emailcheck(int Fid)
        {
            NormalMember member = _context.NormalMembers.FirstOrDefault(c => c.Fid == Fid);
           

            return View(member);
        }
        [HttpPost]
        public IActionResult Emailcheck(NormalMember member)
        {
            NormalMember x=_context.NormalMembers.FirstOrDefault(c => c.Fid==member.Fid);
                x.IsSuspensed =0;
                x.EmailCertified =1;
            _context.SaveChanges();


            return Redirect("~/Home/CIndex");
        }




        public IActionResult memberview()
        {
            string loginmember = "";
            loginmember = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            if (loginmember == null)
                return RedirectToAction("Login");
            NormalMember x = JsonSerializer.Deserialize<NormalMember>(loginmember);

            if (x.MemberPhotoFile == null)
            {
                x.MemberPhotoFile = "167126861274498_P19696185.jpg";
            }
            return View(x);
        }
        public IActionResult Edit()
        {
           
            string loginmember = "";
            loginmember = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);



            //NormalMember x = JsonSerializer.Deserialize<NormalMember>(loginmember);

            CNormalMemberViewModel x = JsonSerializer.Deserialize<CNormalMemberViewModel>(loginmember);
            //NormalMember y = _context.NormalMembers.FirstOrDefault(c => c.Fid == x.Fid);
            //x.MemberPhotoFile = y.MemberPhotoFile;
            return View(x);
        }
        [HttpPost]
        public IActionResult Edit(CNormalMemberViewModel memberedit)
        {
           
            if (memberedit!= null)
            {
                NormalMember x = _context.NormalMembers.FirstOrDefault(c => c.Fid == memberedit.Fid);
                if (memberedit.photo != null)
                {
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    string filePath = Path.Combine(_eviroment.WebRootPath, "images", fileName);
                    //檔案上傳到uploads資料夾中

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        memberedit.photo.CopyTo(fileStream);
                    }
                    memberedit.MemberPhotoFile = fileName;
                }
                          x.MemberName=memberedit.MemberName;
                          x.MemberPhotoFile=memberedit.MemberPhotoFile;
                          x.Birthday=memberedit.Birthday;
                          x.Phone = memberedit.Phone;
                          x.Email = memberedit.Email;
                          x.Gender= memberedit.Gender;
                          x.AddressCity=memberedit.AddressCity;
                          x.AddressArea=memberedit.AddressArea;
                _context.SaveChanges();
              
                string json = JsonSerializer.Serialize(x);
                HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                return RedirectToAction("Edit");

            }
            else
            {
                return RedirectToAction("login");
            }
            
    }
        public IActionResult Alterpassword()
        {
            string loginmember = "";
            loginmember = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            CNormalMemberViewModel x = JsonSerializer.Deserialize<CNormalMemberViewModel>(loginmember);
            return View(x);
        }

        [HttpPost]
        public IActionResult Alterpassword(CNormalMemberViewModel member)
        {
            NormalMember x = _context.NormalMembers.FirstOrDefault(c => c.Fid == member.Fid);
            x.Password=member.Password;
            _context.SaveChanges();

            return RedirectToAction("Edit");

        }
        









        public IActionResult Forgetpassword()
        {

            return View();
        }



        [HttpPost]
        public IActionResult Forgetpassword(string Email)
        {
            string smtpAddress = "smtp.gmail.com";
            //設定Port
            int portNumber = 587;
            bool enableSSL = true;
            //填入寄送方email和密碼
            string emailFrom = "a29816668@gmail.com";
            string emailpassword = "joksdquaswjdyzpu";
            //收信方email 可以用逗號區分多個收件人
            string emailTo = Email;
            //主旨
            string subject = "Hello";
            //內容
            string body = "https://localhost:7266/CustomerMember/Register";
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                // 若你的內容是HTML格式，則為True
                mail.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, emailpassword);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }


            }



            return Content("已寄出");
        }

    }
}
