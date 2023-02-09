﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NuGet.Protocol;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace prjMSIT145_Final.Controllers
{
    public class CustomerMemberController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        private IWebHostEnvironment _eviroment;
        private readonly IConfiguration _config;
        public CustomerMemberController(ispanMsit145shibaContext context, IWebHostEnvironment eviroment, IConfiguration config)
        {
            _context = context;
            _eviroment = eviroment;
            _config=config;
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
                    if (x.IsSuspensed == 0) {
                        string json = JsonSerializer.Serialize(x);
                        HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);

                        return Redirect("~/Home/CIndex");
                    }
                   
                    return View();
                }
            }
            return View();
        }

        public ActionResult loginmailverify(CLoginViewModel vm)
        {
            NormalMember x = _context.NormalMembers.FirstOrDefault(c => c.Phone.Equals(vm.txtAccount) && c.Password.Equals(vm.txtPassword));
            if (x != null)
            {
                if (x.IsSuspensed == 1 && x.EmailCertified ==1)
                {
                    return Json("此帳號被停權");
                }
                else {
                    if (x.EmailCertified == 1 &&x.IsSuspensed==0)
                    {
                        return Json("");
                    }
                    else
                    {
                        return Json("尚未開通會員資格");
                    }
                }
               
                
            }

            return Json("帳號或密碼有錯");
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
                if (i == vm.Phone)
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
            if (vm.MemberName == null && vm.Password == null)
            {
                return View();
            }
            if (vm.Phone == null)
            {
                return View();
            }
            else
            {
                bool correct = Regex.IsMatch(vm.Phone, @"^09[0-9]{8}$");
                if (!(correct))
                {
                    return View();
                }
                
            }
            if (vm.Email == null)
            {
                return View();
            }
            else
            {
                bool correct = Regex.IsMatch(vm.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!(correct))
                {
                    return View();
                }
            }
            

            if (photo != null)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(_eviroment.WebRootPath, "images/Customer/Member", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
                vm.MemberPhotoFile = fileName;
            }
            Random rnd = new Random();
            vm.EmailCertified = rnd.Next(10000000, 90000000);
            _context.Add(vm.member);
            _context.SaveChanges();
            string url = $"https://localhost:7266/CustomerMember/Emailcheck/?Fid={vm.Fid}";
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
            string body =$"<h2>新會員你好請點此開通連結</h2><h3><br><a href={url}>請點此開通連結</a>並回表單輸入此驗證碼{vm.EmailCertified}</h3>";
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom,"日柴", System.Text.Encoding.UTF8);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                // 若你的內容是HTML格式，則為True
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, emailpassword);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }


            }
                return Redirect("~/Home/CIndex");
          
        }
        public IActionResult Verifyaccount(NormalMember vm)
        {
            if (vm.MemberName == null)
            {
                return Json("姓名欄位不能空值");
            }
            if (vm.Phone == null)
            {
                return Json("電話欄位不能空值");
            }

            if(vm.Email ==null)
            {
                return Json("Email欄位不能空值");
            }
            else
            {

                bool correct = Regex.IsMatch(vm.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!(correct))
                {
                    return Json("Email格式錯誤");
                }
            }

            if (vm.Password ==null)
            {
                return Json("密碼欄位不能空值");
            }
            
            

            var data = _context.NormalMembers.Select(c => c.Phone);
            foreach (var i in data)
            {
                if (i == vm.Phone)
                {
                    return Json("帳號重複");
                   
                }
            }
            return Json("已發送驗證信");
        }

        public IActionResult Emailcheck(int? Fid)
        {
            NormalMember member = _context.NormalMembers.FirstOrDefault(c => c.Fid == Fid);
            if (Fid == null || member==null)
            {
                return Redirect("~/Home/CIndex");
            }
            if (member.EmailCertified == 1)
            {
                return Redirect("~/Home/CIndex");
            }
            
            return View(member);
        }
        [HttpPost]
        public IActionResult Emailcheck(NormalMember member)
        {
            NormalMember x=_context.NormalMembers.FirstOrDefault(c => c.Fid==member.Fid);
            if (x != null && x.EmailCertified == member.EmailCertified)
            {
                x.IsSuspensed = 0;
                x.EmailCertified = 1;
                _context.SaveChanges();
                return Redirect("~/Home/CIndex");
            }
            
            return Redirect("~/Home/CIndex");

        }
        public IActionResult Emailcheckword(NormalMember member)
        {
          

            NormalMember x = _context.NormalMembers.FirstOrDefault(c => c.Fid == member.Fid);
            if (x != null && x.EmailCertified == member.EmailCertified)
            {
                return Json("");

            }
            return Json("驗證碼錯誤");
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
                if(memberedit.MemberName==null || memberedit.Email == null)
                {
                    return View(memberedit);
                }
                NormalMember x = _context.NormalMembers.FirstOrDefault(c => c.Fid == memberedit.Fid);
                
                if (memberedit.photo != null)
                {
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    string filePath = Path.Combine(_eviroment.WebRootPath, "images/Customer/Member", fileName);
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
        public IActionResult Editverify(CNormalMemberViewModel vm)
        {
            if (vm.member == null)
            {
                return Json("名子不能空值");
            };
            if (vm.Email==null)
            {
                return Json("信箱不能空值");
            }
            else { 
                bool correct = Regex.IsMatch(vm.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!(correct))
                         {
                return Json("Email格式錯誤");
                        }
            };
            return Json("修改完成"); 

        }

        public IActionResult Alterpassword()
        {
            string loginmember = "";
            loginmember = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            if (loginmember != null) {
            CNormalMemberViewModel x = JsonSerializer.Deserialize<CNormalMemberViewModel>(loginmember);
            return View(x);
            }
            return Redirect("~/Home/CIndex");
        }

        [HttpPost]
        public IActionResult Alterpassword(CNormalMemberViewModel member)
        {
            if (member.Password == null || member.Password!=member.Passwordcheck)
            {
                return View();
            }
            NormalMember x = _context.NormalMembers.FirstOrDefault(c => c.Fid == member.Fid);
            x.Password=member.Password;
            _context.SaveChanges();

            return RedirectToAction("Edit");

        }
        public IActionResult Alterpasswordverify(CNormalMemberViewModel vm)
        {

            NormalMember x=_context.NormalMembers.FirstOrDefault(c => c.Fid == vm.Fid);
            if (vm.OldPassword != x.Password)
            {
                return Json("舊密碼錯誤");
            }
          return Json("舊密碼正確");

        }
        public IActionResult Forgetpassword()
        {
            




            return View();

        }


        //public IActionResult Forgetpassword()
        //{

        //    return View();
        //}



        [HttpPost]
        public IActionResult Forgetpassword(CNormalMemberViewModel vm)
        {
            if (vm.Phone == null || vm.Email == null)
            {
                return View();
            }
            else
            {
                NormalMember member = _context.NormalMembers.FirstOrDefault(c => c.Phone == vm.Phone && c.Email == vm.Email);
                if (member == null)
                {
                    return View();
                }
                else
                {
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
                    string subject = "Hello";
                    //內容
                    string body = $"https://localhost:7266/CustomerMember/forgetalterpassword/?Fid={vm.Fid}";
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

                    //return Redirect("~/Home/CIndex");
                    return Redirect("~/Home/CIndex");

                }

            }

                 return Redirect("~/Home/CIndex");
        }

        public IActionResult forgetalterpassword(int? Fid)
        {
            if (Fid != null)
            {
                NormalMember member = _context.NormalMembers.FirstOrDefault(c => c.Fid == Fid);
                if(member!= null)
                {
                    return View(member);
                }

                return Redirect("~/Home/CIndex");


            }


            return Redirect("~/Home/CIndex");
        }
        [HttpPost]
        public IActionResult forgetalterpassword(NormalMember member)
        {
            if(member.Password == null)
            {
                return View();
            }
            else
            {
                NormalMember x=_context.NormalMembers.FirstOrDefault(c=>c.Fid==member.Fid);
                if (x == null)
                {
                    return View();
                }
                else
                {
                    x.Password = member.Password;
                    _context.SaveChanges();
                    return Redirect("~/Home/CIndex");
                }
               
            }





        }







        public IActionResult getCartOrderQty(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                int nfid = Convert.ToInt32(data);
                int ordersQty = _context.Orders.Where(o => o.NFid == nfid && o.OrderState=="0").Count();
                string showQty = ordersQty > 0 ? ordersQty.ToString() : "";
                return Json(showQty);
            }
            return Json("");
        }

        public IActionResult CCustomerServiceMailBox()
        {
            return View();
        }
        public IActionResult submitCustomerMail(CustomerServiceMailBoxViewModel mail)
        {
            var DemoMailServer = _config["DemoMailServer:pwd"];
            CSendMail cs = new CSendMail();
            string mailBody = $"收到了來自 {mail.txtSenderName} 先生/小姐來自網站客服信箱的意見。<br><br>詢問主題：<br>" +
                $"<label style='color:blue'>{mail.txtMailSubject}</label><br><br>" +
                $"詢問內容：<br>{mail.txtMailContent}<br><br>" +
                $"來信人電話：{mail.txtPhone}<br><br>" +
                $"來信人Email：{mail.txtEmailAddress}";
            string mailSubject = "網站客服信箱來信";
            string result = cs.sendMail("b9809004@gapps.ntust.edu.tw", mailBody, mailSubject, DemoMailServer.ToString());
            return Json(result);
        }
    }
}
