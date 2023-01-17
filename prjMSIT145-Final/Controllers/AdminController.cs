using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Collections.Generic;
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
            List<CANormalMemberViewModel> list = new List<CANormalMemberViewModel>();
            //if (k == null)
            //{
            
            IEnumerable<NormalMember> normalMembers = from member in _context.NormalMembers
                   select member;
            //}
            if(normalMembers != null)
            {
                foreach(NormalMember n in normalMembers)
                {
                    CANormalMemberViewModel cvm = new CANormalMemberViewModel();
                    cvm.normalMember = n;
                    list.Add(cvm);
                }

            }
            
            return View(list);
            
        }

        public IActionResult ANormalMemberDetails(int? id)
        {

            if (id == null)
                return RedirectToAction("ANormalMemberList");
            
            var personalDatas = _context.NormalMembers.FirstOrDefault(c => c.Fid == (int)id);
            IEnumerable<Order> orderDatas = from order in _context.Orders
                                            where order.NFid == (int)id
                                            select order;
            
            if (personalDatas != null)
            {
                CANormalMemberViewModel n = new CANormalMemberViewModel();
                n.normalMember = personalDatas;
                if (orderDatas != null)
                {
                    n.orders = orderDatas;
                }
                return View(n);
            }

            return RedirectToAction("ANormalMemberList");
        }
        [HttpPost]
        public IActionResult ANormalMemberDetails(CANormalMemberViewModel n)
        {
            if (n != null)
            {
                var user = _context.NormalMembers.FirstOrDefault(t => t.Fid == n.Fid);
                if (!string.IsNullOrEmpty(n.txtPassword) && (n.txtPassword.Trim() == n.txtConfirmPwd))
                {                    
                    if (user != null)
                    {
                        user.Password = n.txtPassword;                        
                    }
                }
                user.IsSuspensed = (int)n.IsSuspensed;
                _context.SaveChanges();
            }

            return RedirectToAction("ANormalMemberList");
        }

        public IActionResult ABusinessMemberList()
        {
            List<CABusinessMemberViewModel> list = new List<CABusinessMemberViewModel>();
            //if (k == null)
            //{

            IEnumerable<BusinessMember> businessMembers = from member in _context.BusinessMembers
                                                      select member;
            //}
            if (businessMembers != null)
            {
                foreach (BusinessMember b in businessMembers)
                {
                    CABusinessMemberViewModel cvm = new CABusinessMemberViewModel();
                    cvm.businessMember = b;
                    list.Add(cvm);
                }

            }

            return View(list);
        }

        public IActionResult ANormalMemberOrder(int? id)
        {
            if (id == null)
                return RedirectToAction("ANormalMemberDetails");

            //var personalDatas = _context.ViewShowFullOrders.FirstOrDefault(c => c.OrderFid == (int)id);
            IEnumerable<ViewShowFullOrder> orderDatas = from order in _context.ViewShowFullOrders
                                                        join bm in _context.BusinessImgs on order.BFid equals bm.BFid
                                                        join bAdd in _context.BusinessMembers on order.BFid equals bAdd.Fid
                                                        where order.OrderFid == (int)id
                                                        select order;

            if (orderDatas != null)
            {
                CANormalMemberOrderViewModel n = new CANormalMemberOrderViewModel();
                List<CANormalMemberOrderDetailViewModel> items = new List<CANormalMemberOrderDetailViewModel>();
                foreach(ViewShowFullOrder vsf in orderDatas)
                {
                    CANormalMemberOrderDetailViewModel detail = new CANormalMemberOrderDetailViewModel();
                    detail.productName=vsf.ProductName;
                    detail.Options = vsf.Options + "/" + "$" + vsf.SubTotal + "/" + vsf.Qty + "份";
                    items.Add(detail);
                }
                n.details = items;
                n.BMemberName = orderDatas.ToList()[0].BMemberName;
                n.BMemberPhone = orderDatas.ToList()[0].BMemberPhone;
                n.OrderISerialId = orderDatas.ToList()[0].OrderISerialId;
                n.PickUpDate = orderDatas.ToList()[0].PickUpDate;
                n.PickUpTime = orderDatas.ToList()[0].PickUpTime;
                n.TotalAmount = orderDatas.ToList()[0].TotalAmount;
                n.PayTermCatId = orderDatas.ToList()[0].PayTermCatId;
                n.PickUpType = orderDatas.ToList()[0].PickUpType;
                n.PickUpPerson = orderDatas.ToList()[0].PickUpPerson;
                n.PickUpPersonPhone = orderDatas.ToList()[0].PickUpPersonPhone;
                n.Memo = orderDatas.ToList()[0].Memo;

                return View(n);
            }

            return RedirectToAction("ANormalMemberDetails");
        }
    }
}
