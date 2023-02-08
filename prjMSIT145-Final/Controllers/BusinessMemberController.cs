    using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;
using System.Diagnostics.Metrics;
using System.Linq;
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
            ViewBag.emailmessage = "";
            ViewBag.passwordmessage = "";
            return View();
        }
        [HttpPost]
        public IActionResult Blogin(CLoginViewModel cLoginViewModel)
        {
            if (cLoginViewModel.fEmailRegister == null)
            {
                //帳戶帳號密碼確認
                BusinessMember b = _context.BusinessMembers.FirstOrDefault(b => b.Email.Equals(cLoginViewModel.fEmail) && b.Password.Equals(cLoginViewModel.fPassword));
                if (b != null)
                {
                    if (b.Email.Equals(cLoginViewModel.fEmail) && b.Password.Equals(cLoginViewModel.fPassword))
                    {
                        //if (b.ChatroomUserid == null)
                        //{
                        //    ChatroomUser chatroomUser = new ChatroomUser();
                        //    chatroomUser.UserType = 0;//0是客戶 1是商家 2 是平台 欄位改成INT
                        //    chatroomUser.Memberfid = b.Fid;
                        //    _context.SaveChanges();
                        //    var member = _context.ChatroomUsers.FirstOrDefault(u => u.UserType = 0 && u.Memberfid = b.Fid);
                        //    b.ChatroomUserid = member.ChatroomUserid;
                        //    _context.SaveChanges();
                        //}
                        string json = JsonSerializer.Serialize(b);
                        HttpContext.Session.SetString(CDictionary.SK_LOGINED_Business, json);

                        return RedirectToAction("BList", "Order");
                    }
                }
                BusinessMember Email = _context.BusinessMembers.FirstOrDefault(b => b.Email.Equals(cLoginViewModel.fEmail));
                if (Email == null)
                {
                  ViewBag.emailmessage = "找不到用戶Email";
                    return View();
                }
               
                ViewBag.passwordmessage = "密碼錯誤";
                return View();
            }
            Gmail gmail = new Gmail();
            gmail.sendGmail(cLoginViewModel.fEmailRegister);
            return View();

        }
        
        public IActionResult RegisterVerification(string? Email)
        {
            BusinessMember b = _context.BusinessMembers.FirstOrDefault(b => b.Email.Equals(Email));
            if (b==null)
            {

                return Json("這個Email可以使用");

            }
            return Json("這個Email已被使用");

        }


        public  IActionResult Register(string? email)
        {
            ViewBag.email = email;
            return PartialView();
        }
        [HttpPost]
        public IActionResult Register(BusinessMember member)
        {
            
            _context.BusinessMembers.Add(member);
            _context.SaveChanges();
            BusinessImg businessImg = new BusinessImg();
            var Newmember = _context.BusinessMembers.FirstOrDefault(m=>m.Email==member.Email);
            businessImg.BFid = Newmember.Fid;
            _context.BusinessImgs.Add(businessImg);
            _context.SaveChanges();
            return RedirectToAction("Blogin");
        }

        public IActionResult BRevise()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
                return RedirectToAction("Blogin");
            var json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_Business);
            BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
            return View(member);
        }
        [HttpPost]
        public IActionResult BRevise(BusinessMember member)
        {
            _context.BusinessMembers.Update(member);
            _context.SaveChanges();
            return RedirectToAction("BList", "Order");
        }



        public IActionResult Index()
        {
            return View();
        }
        




    }
}
