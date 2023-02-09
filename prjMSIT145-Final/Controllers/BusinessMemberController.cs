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
        private IWebHostEnvironment _eviroment;
        private readonly ispanMsit145shibaContext _context;
        public BusinessMemberController(ispanMsit145shibaContext context, IWebHostEnvironment p)
        {
            _eviroment = p;
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
            if (b == null)
            {

                return Json("這個Email可以使用");

            }
            return Json("這個Email已被使用");

        }


        public IActionResult Register(string? email)
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
            businessImg.BFid = member.Fid;
            _context.BusinessImgs.Add(businessImg);
            ChatroomUser chatroomUser = new ChatroomUser();
            chatroomUser.UserType = 1;//0是客戶 1是商家 2 是平台 欄位改成INT
            chatroomUser.Memberfid = member.Fid;
            _context.ChatroomUsers.Add(chatroomUser);
            _context.SaveChanges();
            member.ChatroomUserid = chatroomUser.ChatroomUserid;
            _context.SaveChanges();


           
            return RedirectToAction("Blogin");
        }

        public IActionResult BRevise()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
                return RedirectToAction("Blogin");
            var json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_Business);
            BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
            BusinessImg Img = _context.BusinessImgs.FirstOrDefault(u => u.BFid == member.Fid);
            if (Img == null)
            {
                BusinessImg NewImg = new BusinessImg();
                NewImg.BFid = member.Fid;
                _context.BusinessImgs.Add(NewImg);
                _context.SaveChanges();
                Img = NewImg;
            }
            CBBusinessMember vm = new CBBusinessMember();
            vm._businessMember = member;
            vm.Fid = member.Fid;
            vm.MemberName = member.MemberName;
            vm.Brand = member.Brand;
            vm.Phone = member.Phone;
            vm.Password = member.Password;
            vm.OpenTime = member.OpenTime;
            vm.CloseTime = member.CloseTime;
            vm.Address = member.Address;
            vm.Email = member.Email;
            vm.ShopType = member.ShopType;
            vm.Gps = member.Gps;
            vm.ImgFid = Img.Fid;
            vm.LogoImgFileName = Img.LogoImgFileName;
            vm.SighImgFileName = Img.SighImgFileName;
            vm.BannerImgFileName1 = Img.BannerImgFileName1;
            vm.BannerImgFileName2 = Img.BannerImgFileName2;
            vm.BannerImgFileName3 = Img.BannerImgFileName3;
            return View(vm);
        }
        [HttpPost]
        public IActionResult BRevise(CBBusinessMember vm)
        {
            BusinessMember member = _context.BusinessMembers.FirstOrDefault(m => m.Fid == vm.Fid);
            BusinessImg img = _context.BusinessImgs.FirstOrDefault(i => i.BFid == vm.Fid);
            if (member == null)
                return RedirectToAction("Blogin");
            if (img == null)
            {
                BusinessImg NewImg = new BusinessImg();
                NewImg.BFid = member.Fid;
                _context.BusinessImgs.Add(NewImg);
                _context.SaveChanges();
                img = NewImg;
            }
            if (vm.LogoImgFile != null)
            {
                string oldPath = _eviroment.WebRootPath + "/images/" + img.LogoImgFileName;
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string path = _eviroment.WebRootPath + "/images/" + photoName;
                img.LogoImgFileName = photoName;
                vm.LogoImgFile.CopyTo(new FileStream(path, FileMode.Create));
            }
            if (vm.SighImgFile != null)
            {
                string oldPath = _eviroment.WebRootPath + "/images/" + img.SighImgFileName;
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string path = _eviroment.WebRootPath + "/images/" + photoName;
                img.SighImgFileName = photoName;
                vm.SighImgFile.CopyTo(new FileStream(path, FileMode.Create));
            }
           
            member.MemberName=vm.MemberName;
            member.Brand=vm.Brand;
            member.Phone=vm.Phone;
            member.Password=vm.Password;
            member.OpenTime=vm.OpenTime;
            member.CloseTime=vm.CloseTime;
            member.Address=vm.Address;
            member.Email=vm.Email;
            member.ShopType=vm.ShopType;
            member.ContactPerson=vm.ContactPerson;
            _context.SaveChanges();
            return RedirectToAction("BList", "Order");
        }



        public IActionResult Index()
        {
            return View();
        }





    }
}
