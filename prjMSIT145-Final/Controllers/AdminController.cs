using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.Mail;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace prjMSIT145_Final.Controllers
{
    public class AdminController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        private readonly IWebHostEnvironment _host;
        public AdminController(ispanMsit145shibaContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host=host;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult sendAccountLockedNotice(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                CASendEmailViewModel mail=JsonConvert.DeserializeObject<CASendEmailViewModel>(data);
                if (mail.memberType=="B")
                {
                    var user = _context.BusinessMembers.FirstOrDefault(t => t.Fid == mail.memberId);
                    user.IsSuspensed = (int)mail.IsSuspensed;
                }
                else if (mail.memberType=="N")
                {
                    var user = _context.NormalMembers.FirstOrDefault(t => t.Fid == mail.memberId);
                    user.IsSuspensed = (int)mail.IsSuspensed;
                }
                _context.SaveChanges();

                MailMessage MyMail = new MailMessage();
                MyMail.From = new MailAddress("日柴 <ShibaAdmin@msit145shiba.com.tw>", "日柴", System.Text.Encoding.UTF8);
                //foreach(string receiver in ReceiveMail)
                //{
                //    MyMail.To.Add(receiver); //設定收件者Email
                //}
                MyMail.To.Add(mail.txtRecipient);                
                //MyMail.Bcc.Add("jftoes@gmail.com,yrrek9120@gmail.com"); //加入密件副本的Mail

                string changeType = (int)mail.IsSuspensed==1 ? "停權" : "復權";
                MyMail.Subject = $"帳號{changeType}通知";

                MyMail.Body = $"您好：<br>您的帳戶已被{changeType}，若有問題請洽詢網站管理員。" +
                    "<br><br>" +
                    $"{changeType}原因：<br>"+
                    mail.txtMessage +
                    "<br><br>=====================================================================<br>" +
                    "<br>此為系統通知，請勿直接回信，謝謝"; //設定信件內容

                MyMail.IsBodyHtml = true; //是否使用html格式
                

                SmtpClient MySMTP = new SmtpClient();

                MySMTP.Credentials = new System.Net.NetworkCredential("b9809004@gapps.ntust.edu.tw", "Gknt824nut"); //這裡要填正確的帳號跟密碼
                MySMTP.Host = "smtp.gmail.com"; //設定smtp Server

                MySMTP.Port = 25;
                MySMTP.EnableSsl = true; //gmail預設開啟驗證
                
                try
                {
                    MySMTP.Send(MyMail);

                }
                catch (Exception ex)
                {
                    return Content(ex.ToString());
                }
                finally
                {
                    MyMail.Dispose(); //釋放資源

                }
            }
            
            return NoContent();

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

            string json = System.Text.Json.JsonSerializer.Serialize(member);
            HttpContext.Session.SetString(CDictionary.SK_LOGINED_ADMIN, json);
           
            return RedirectToAction("ANormalMemberList");
        }
        public IActionResult ALogout()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_ADMIN))
            {
                HttpContext.Session.Remove(CDictionary.SK_LOGINED_ADMIN);
            }
            return RedirectToAction("ALogin");
        }
        public IActionResult ANormalMemberList()
        {
            List<CANormalMemberViewModel> list = new List<CANormalMemberViewModel>();
            
            IEnumerable<NormalMember> normalMembers = from member in _context.NormalMembers
                   select member;
            
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
                //user.IsSuspensed = (int)n.IsSuspensed;
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
            
            IEnumerable<BusinessMember> businessMembers = from member in _context.BusinessMembers
                                                      select member;
            
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
                        orderby img.OrderBy
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
            List<CAAdImg> ads = null;
            if (!string.IsNullOrEmpty(data))
            {                
                ads=JsonConvert.DeserializeObject<List<CAAdImg>>(data);
                
                //_context.AdImgs.RemoveRange(ads);
                foreach (CAAdImg ad in ads)
                {
                    AdImg updateItem = _context.AdImgs.FirstOrDefault(a => a.Fid==Convert.ToInt32(ad.sFid));                    
                    updateItem.OrderBy=ad.OrderBy;
                    
                }
                
                _context.SaveChanges();
                result="1";
            }
            
            return Content(result);
        }
        public IActionResult saveAdInfo(string data)
        {
            string result = "0";
            CAAdImg ad = null;
            if (!string.IsNullOrEmpty(data))
            {
                ad=JsonConvert.DeserializeObject<CAAdImg>(data);

                AdImg updateItem = _context.AdImgs.FirstOrDefault(a => a.Fid==Convert.ToInt32(ad.sFid));
                updateItem.StartTime=ad.StartTime;
                updateItem.EndTime=ad.EndTime;                
                updateItem.Hyperlink=ad.Hyperlink;                
                
                _context.SaveChanges();
                result="1";
            }

            return Content(result);
        }
        public IActionResult deleteAd(string data)
        {
            string result = "0";
            //CAAdImg ad = null;
            if (!string.IsNullOrEmpty(data))
            {
                AdImg deleteItem = _context.AdImgs.FirstOrDefault(a => a.Fid==Convert.ToInt32(data));
                _context.AdImgs.Remove(deleteItem);

                _context.SaveChanges();
                result="1";
            }

            return Content(result);
        }
        public IActionResult AaddAdImg(string data)
        {            
            CAAdImg ad = null;
            AdImg returnAd = null;
            if (!string.IsNullOrEmpty(data))
            {
                ad=JsonConvert.DeserializeObject<CAAdImg>(data);
                byte[] imageBytes = ad.icon;
                string[] fileTypeArr = ad.fileType.Split('/');
                string fileType = "."+fileTypeArr[1];

                string fName = Guid.NewGuid().ToString()+fileType;
                MemoryStream buf = new MemoryStream(imageBytes);
                string filePath = Path.Combine(_host.WebRootPath, "adminImg/adDisplay", fName);                                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    buf.WriteTo(fileStream);                   
                }
                
                buf.Close();
                buf.Dispose();

                ad.ImgName=fName;
                var lastAd = _context.AdImgs.OrderBy(a => a.OrderBy).LastOrDefault();
                int orderBy = 1;
                if (lastAd!=null)
                    orderBy = (int)lastAd.OrderBy+1;
                ad.OrderBy=orderBy;
                _context.AdImgs.Add(ad.adImg);

                _context.SaveChanges();
                
                returnAd = _context.AdImgs.FirstOrDefault(a => a.OrderBy==orderBy);
            }

            return Json(returnAd);
        }
        public IActionResult saveSmallImgData(string data)
        {
            CAAdImg ad = null;
            AdImg returnAd = null;
            if (!string.IsNullOrEmpty(data))
            {
                ad=JsonConvert.DeserializeObject<CAAdImg>(data);
                if (!string.IsNullOrEmpty(ad.fileType))
                {
                    byte[] imageBytes = ad.icon;
                    string[] fileTypeArr = ad.fileType.Split('/');
                    string fileType = "."+fileTypeArr[1];

                    string fName = Guid.NewGuid().ToString()+fileType;
                    MemoryStream buf = new MemoryStream(imageBytes);
                    string filePath = Path.Combine(_host.WebRootPath, "adminImg/adDisplay", fName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        buf.WriteTo(fileStream);
                    }
                    buf.Close();
                    buf.Dispose();

                    ad.ImgName=fName;
                }
                
                var lastAd = _context.AdImgs.FirstOrDefault(a=>a.Fid==ad.Fid);
                
                if (lastAd!=null)
                {
                    lastAd.Hyperlink=ad.Hyperlink;
                    if (!string.IsNullOrEmpty(ad.fileType))
                        lastAd.ImgName=ad.ImgName;                    
                    _context.SaveChanges();
                }
              
                returnAd = lastAd;
            }

            return Json(returnAd);
            
        }


    }
}
