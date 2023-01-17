using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Text.Json;

namespace prjMSIT145_Final.Controllers
{
    public class AdminController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public AdminController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult checkPwd(string account, string pwd)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(pwd))
                return Content("請輸入完整的帳號和密碼");
            else
            {
                string result = "0";
                AdminMember member = _context.AdminMembers.FirstOrDefault(u => u.Account == account && u.Password == pwd);
                if (member != null)
                    result = "1";

                return Content(result);

            }
        }
        public IActionResult ALogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ALogin(AdminMember admin)
        {
            AdminMember member = _context.AdminMembers.FirstOrDefault(u => u.Account == admin.Account && u.Password == admin.Password);
            if (member == null)
            {
                ViewData["checkAccountResult"]="帳號或密碼有誤";
                return View();
            }

            string json = JsonSerializer.Serialize(member);
            HttpContext.Session.SetString(CDictionary.SK_LOGINED_ADMIN, json);
            //return RedirectToAction("ANormalMemberList");
            return RedirectToAction("ANormalMemberList");
        }

        public IActionResult ANormalMemberList()
        {
            List<CNormalMemberViewModel> list = new List<CNormalMemberViewModel>();
            //if (k == null)
            //{
            
            IEnumerable<NormalMember> normalMembers = from member in _context.NormalMembers
                   select member;
            //}
            if(normalMembers != null)
            {
                foreach(NormalMember n in normalMembers)
                {
                    CNormalMemberViewModel cvm = new CNormalMemberViewModel();
                    cvm.normalMember = n;
                    list.Add(cvm);
                }

            }
            
            return View(list);
            
        }

        public IActionResult ABusinessMemberList()
        {
            List<CBusinessMemberViewModel> list = new List<CBusinessMemberViewModel>();
            //if (k == null)
            //{

            IEnumerable<BusinessMember> businessMembers = from member in _context.BusinessMembers
                                                      select member;
            //}
            if (businessMembers != null)
            {
                foreach (BusinessMember b in businessMembers)
                {
                    CBusinessMemberViewModel cvm = new CBusinessMemberViewModel();
                    cvm.businessMember = b;
                    list.Add(cvm);
                }

            }

            return View(list);
        }
    }
}
