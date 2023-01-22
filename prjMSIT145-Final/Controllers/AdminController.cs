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

        public IActionResult ANormalMemberOrder(int? id)
        {//todo 停權前發送email通知
            if (id == null)
                return RedirectToAction("ANormalMemberDetails");

            var orderDatas = from order in _context.ViewShowFullOrders
                             join bm in _context.BusinessImgs on order.BFid equals bm.BFid
                             join bAdd in _context.BusinessMembers on order.BFid equals bAdd.Fid
                             join payTerm in _context.PaymentTermCategories on order.PayTermCatId equals payTerm.Fid
                             where order.OrderFid == (int)id
                             select new
                             {
                                 order.BMemberName,
                                 order.BMemberPhone,
                                 order.OrderISerialId,
                                 order.PickUpDate,
                                 order.PickUpPerson,
                                 order.PickUpPersonPhone,
                                 order.PickUpTime,
                                 order.PickUpType,
                                 order.PayTermCatId,
                                 order.Memo,
                                 order.TotalAmount,
                                 order.ProductName,
                                 order.Options,
                                 order.SubTotal,
                                 order.Qty,
                                 order.OrderState,
                                 bm.LogoImgFileName,
                                 bAdd.Address,
                                 payTerm.PaymentType
                             };

            if (orderDatas != null)
            {
                CANormalMemberOrderViewModel n = new CANormalMemberOrderViewModel();
                List<CANormalMemberOrderDetailViewModel> items = new List<CANormalMemberOrderDetailViewModel>();
                foreach (var vsf in orderDatas)
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
                n.PickUpType = orderDatas.ToList()[0].PickUpType;
                n.PickUpPerson = orderDatas.ToList()[0].PickUpPerson;
                n.PickUpPersonPhone = orderDatas.ToList()[0].PickUpPersonPhone;
                n.Memo = orderDatas.ToList()[0].Memo;
                n.businessImgFile = orderDatas.ToList()[0].LogoImgFileName;
                n.businessAddress = orderDatas.ToList()[0].Address;
                n.PayTermCatName = orderDatas.ToList()[0].PaymentType;
                n.OrderState = orderDatas.ToList()[0].OrderState;

                return View(n);
            }

            return RedirectToAction("ANormalMemberDetails");
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

        public IActionResult ABusinessMemberDetails(int? id)
        {
            if (id == null)
                return RedirectToAction("ABusinessMemberList");

            var businessDatas = _context.BusinessMembers.FirstOrDefault(c => c.Fid == (int)id);
            IEnumerable<Order> orderDatas = from order in _context.Orders
                                            where order.BFid == (int)id
                                            select order;

            if (businessDatas != null)
            {
                CABusinessMemberViewModel b = new CABusinessMemberViewModel();
                b.businessMember = businessDatas;
                if (orderDatas != null)
                {
                    b.orders = orderDatas;
                }
                return View(b);
            }

            return RedirectToAction("ABusinessMemberList");
            
        }
        [HttpPost]
        public IActionResult ABusinessMemberDetails(CABusinessMemberViewModel b)
        {
            if (b != null)
            {
                var user = _context.BusinessMembers.FirstOrDefault(t => t.Fid == b.Fid);
                if (!string.IsNullOrEmpty(b.txtPassword) && (b.txtPassword.Trim() == b.txtConfirmPwd))
                {
                    if (user != null)
                    {
                        user.Password = b.txtPassword;
                    }
                }
                user.IsSuspensed = (int)b.IsSuspensed;
                _context.SaveChanges();
            }

            return RedirectToAction("ABusinessMemberList");
            
        }
        public IActionResult ABusinessMemberOrder(int? id)
        {
            if (id == null)
                return RedirectToAction("ABusinessMemberDetails");

            var orderDatas = from order in _context.ViewShowFullOrders
                             join bm in _context.BusinessImgs on order.BFid equals bm.BFid
                             join bAdd in _context.BusinessMembers on order.BFid equals bAdd.Fid
                             join payTerm in _context.PaymentTermCategories on order.PayTermCatId equals payTerm.Fid
                             join nm in _context.NormalMembers on order.NFid equals nm.Fid
                             where order.OrderFid == (int)id
                             select new
                             {
                                 order.BMemberName,
                                 order.BMemberPhone,
                                 order.OrderISerialId,
                                 order.PickUpDate,
                                 order.PickUpPerson,
                                 order.PickUpPersonPhone,
                                 order.PickUpTime,
                                 order.PickUpType,
                                 order.PayTermCatId,
                                 order.Memo,
                                 order.TotalAmount,
                                 order.ProductName,
                                 order.Options,
                                 order.SubTotal,
                                 order.Qty,
                                 order.OrderState,
                                 bm.LogoImgFileName,
                                 bAdd.Address,
                                 payTerm.PaymentType,
                                 nm.MemberPhotoFile
                             };

            if (orderDatas != null)
            {
                CANormalMemberOrderViewModel n = new CANormalMemberOrderViewModel();
                List<CANormalMemberOrderDetailViewModel> items = new List<CANormalMemberOrderDetailViewModel>();
                foreach (var vsf in orderDatas)
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
                n.PickUpType = orderDatas.ToList()[0].PickUpType;
                n.PickUpPerson = orderDatas.ToList()[0].PickUpPerson;
                n.PickUpPersonPhone = orderDatas.ToList()[0].PickUpPersonPhone;
                n.Memo = orderDatas.ToList()[0].Memo;
                n.businessImgFile = orderDatas.ToList()[0].LogoImgFileName;
                n.businessAddress = orderDatas.ToList()[0].Address;
                n.PayTermCatName = orderDatas.ToList()[0].PaymentType;
                n.OrderState = orderDatas.ToList()[0].OrderState;
                n.normalImgFile= orderDatas.ToList()[0].MemberPhotoFile;

                return View(n);
            }

            return RedirectToAction("ABusinessMemberDetails");
        }

        public IActionResult ADisplayImgManage()
        {
            var datas = from img in _context.AdImgs
                        select img;
            List<CAAdImg> list=new List<CAAdImg>();
            foreach(var data in datas)
            {
                CAAdImg cAAd = new CAAdImg();
                cAAd.adImg = data;
                list.Add(cAAd);
            }
            if (list.Count>0)
            {
                var ader = _context.BusinessMembers.FirstOrDefault(a => a.Fid==list[0].BFid);
                if (ader!=null)
                    list[0].ImgBelongTo=ader.MemberName;
            }
            
            return View(list.AsEnumerable());
        }
        public IActionResult loadImgInfo(string fid)
        {
            CAAdImg cAAdImg = new CAAdImg();
            AdImg img = _context.AdImgs.FirstOrDefault(i => i.Fid==Convert.ToInt32(fid));
            if (img!=null)
            {
                var ader=_context.BusinessMembers.FirstOrDefault(i => i.Fid==Convert.ToInt32(img.BFid));
                if (ader!=null)
                    cAAdImg.ImgBelongTo=ader.MemberName;
                cAAdImg.adImg=img;
            }            
            
            return Json(cAAdImg);            
        }
        
        public IActionResult changeAdOrderBy(string data)
        {
            string result = "0";
            List<AdImg> ads = null;
            if (!string.IsNullOrEmpty(data))
            {                
                ads=JsonSerializer.Deserialize<List<AdImg>>(data);
                
                _context.AdImgs.RemoveRange(ads);
                foreach (AdImg ad in ads)
                {
                    AdImg removeItem = _context.AdImgs.FirstOrDefault(a => a.Fid==ad.Fid);
                    _context.AdImgs.Remove(removeItem);
                }
                //todo 刪除再新增_context.AdImgs的資料
                _context.SaveChanges();
                result="1";
            }
            
            
            return Content(result);
        }
        //public IActionResult checkFormData(Member member, IFormFile file)
        //{
        //    string filePath = Path.Combine(_host.WebRootPath, "img", file.FileName);
        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        file.CopyTo(fileStream);
        //    }
        //    //return Content($"Hello,{member.Name}.You are {member.Age} and email is {member.Email}", "text/plain", Encoding.UTF8);


        //    byte[]? imgByte = null;
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        imgByte = memoryStream.ToArray();
        //        member.FileData = imgByte;
        //    }

        //    member.FileName = file.FileName;

        //    _db.Members.Add(member);
        //    _db.SaveChanges();

        //    return Content($"{filePath}");
        //}
    }
}
