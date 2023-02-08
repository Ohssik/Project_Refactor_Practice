using Microsoft.AspNetCore.Http;
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
using static NuGet.Packaging.PackagingConstants;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using NuGet.Common;
using Newtonsoft.Json.Linq;
using System.Security.Policy;

namespace prjMSIT145_Final.Controllers
{
    public class AdminController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly IConfiguration _config;
        public AdminController(ispanMsit145shibaContext context, IWebHostEnvironment host, IConfiguration config)
        {
            _context = context;
            _host=host;
            _config=config;
        }
        
        public IActionResult sendAccountLockedNotice(string data)//寄送帳號停權/復權通知
        {
            var DemoMailServer = _config["DemoMailServer:pwd"];

            if (!string.IsNullOrEmpty(data))
            {                                                
                CASendEmailViewModel mail=JsonConvert.DeserializeObject<CASendEmailViewModel>(data);
                if (mail != null)
                {
                    if (mail.memberType == "B")
                    {
                        var user = _context.BusinessMembers.FirstOrDefault(t => t.Fid == mail.memberId);
                        user.IsSuspensed = (int)mail.IsSuspensed;
                    }
                    else if (mail.memberType == "N")
                    {
                        var user = _context.NormalMembers.FirstOrDefault(t => t.Fid == mail.memberId);
                        user.IsSuspensed = (int)mail.IsSuspensed;
                    }
                    _context.SaveChanges();

                    string changeType = (int)mail.IsSuspensed == 1 ? "停權" : "復權";
                    string mailBody = $"您好：<br>您的帳戶已被{changeType}，若有問題請洽詢網站管理員。" +
                        "<br><br>" +
                        $"{changeType}原因：<br>" +
                        mail.txtMessage +
                        "<br><br><hr><br>" +
                        "<br>此為系統通知，請勿直接回信，謝謝";

                    string mailSubject = $"帳號{changeType}通知";

                    string result = sendMail(mail.txtRecipient, mailBody, mailSubject);
                    return Json(result);
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
        {
            if (id == null)
                return RedirectToAction("ANormalMemberDetails");

            //var orderDatas = from order in _context.ViewShowFullOrders
            //                 join bm in _context.BusinessImgs on order.BFid equals bm.BFid
            //                 join bAdd in _context.BusinessMembers on order.BFid equals bAdd.Fid
            //                 join payTerm in _context.PaymentTermCategories on order.PayTermCatId equals payTerm.Fid
            //                 where order.OrderFid == (int)id
            //                 select new
            //                 {
            //                     order.BMemberName,
            //                     order.BMemberPhone,
            //                     order.OrderISerialId,
            //                     order.PickUpDate,
            //                     order.PickUpPerson,
            //                     order.PickUpPersonPhone,
            //                     order.PickUpTime,
            //                     order.PickUpType,
            //                     order.PayTermCatId,
            //                     order.Memo,
            //                     order.TotalAmount,
            //                     order.ProductName,
            //                     order.Options,
            //                     order.SubTotal,
            //                     order.Qty,
            //                     order.OrderState,
            //                     bm.LogoImgFileName,
            //                     bAdd.Address,
            //                     payTerm.PaymentType
            //                 };

            var orderDatas = from order in _context.Orders
                              join f in _context.ViewShowFullOrders on order.Fid equals f.OrderFid
                              into group7
                              from g7 in group7.DefaultIfEmpty()
                              join bm in _context.BusinessImgs on g7.BFid equals bm.Fid
                              into group2
                              from g2 in group2.DefaultIfEmpty()
                              join b in _context.BusinessMembers on order.BFid equals b.Fid
                              into group3
                              from g3 in group3.DefaultIfEmpty()
                              join pay in _context.PaymentTermCategories on order.PayTermCatId equals pay.Fid
                              into group4
                              from g4 in group4.DefaultIfEmpty()
                              where order.Fid == (int)id
                              select new
                              {
                                  g7.BMemberPhone,
                                  g7.BMemberName,
                                  order.OrderISerialId,
                                  order.PickUpDate,
                                  order.PickUpPerson,
                                  order.PickUpPersonPhone,
                                  order.PickUpTime,
                                  order.PickUpType,
                                  order.PayTermCatId,
                                  order.Memo,
                                  order.TotalAmount,
                                  g7.ProductName,
                                  order.OrderState,
                                  g7.Options,
                                  g7.Qty,
                                  g7.SubTotal,
                                  g2.LogoImgFileName,
                                  g3.Address,
                                  g4.PaymentType
                              };

            if (orderDatas != null)
            {
                CANormalMemberOrderViewModel n = new CANormalMemberOrderViewModel();
                List<CANormalMemberOrderDetailViewModel> items = new List<CANormalMemberOrderDetailViewModel>();
                foreach (var vsf in orderDatas.Distinct())
                {
                    CANormalMemberOrderDetailViewModel detail = new CANormalMemberOrderDetailViewModel();
                    detail.productName=vsf.ProductName;
                    detail.Options = vsf.Options + "/" + "$" + vsf.SubTotal + "/" + vsf.Qty + "份";
                    items.Add(detail);
                }
                n.details = items;
                n.BMemberName = orderDatas.Distinct().ToList()[0].BMemberName;
                n.BMemberPhone = orderDatas.Distinct().ToList()[0].BMemberPhone;
                n.OrderISerialId = orderDatas.Distinct().ToList()[0].OrderISerialId;
                n.PickUpDate = orderDatas.Distinct().ToList()[0].PickUpDate;
                n.PickUpTime = orderDatas.Distinct().ToList()[0].PickUpTime;
                n.TotalAmount = orderDatas.Distinct().ToList()[0].TotalAmount;
                n.PickUpType = orderDatas.Distinct().ToList()[0].PickUpType;
                n.PickUpPerson = orderDatas.Distinct().ToList()[0].PickUpPerson;
                n.PickUpPersonPhone = orderDatas.Distinct().ToList()[0].PickUpPersonPhone;
                n.Memo = orderDatas.Distinct().ToList()[0].Memo;
                n.businessImgFile = orderDatas.Distinct().ToList()[0].LogoImgFileName;
                n.businessAddress = orderDatas.Distinct().ToList()[0].Address;
                n.PayTermCatName = orderDatas.Distinct().ToList()[0].PaymentType;
                n.OrderState = orderDatas.Distinct().ToList()[0].OrderState;

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

            //var orderDatas = from order in _context.ViewShowFullOrders
            //                 join bm in _context.BusinessImgs on order.BFid equals bm.BFid
            //                 join bAdd in _context.BusinessMembers on order.BFid equals bAdd.Fid
            //                 join payTerm in _context.PaymentTermCategories on order.PayTermCatId equals payTerm.Fid
            //                 join nm in _context.NormalMembers on order.NFid equals nm.Fid
            //                 where order.OrderFid == (int)id
            //                 select new
            //                 {
            //                     order.BMemberName,
            //                     order.BMemberPhone,
            //                     order.OrderISerialId,
            //                     order.PickUpDate,
            //                     order.PickUpPerson,
            //                     order.PickUpPersonPhone,
            //                     order.PickUpTime,
            //                     order.PickUpType,
            //                     order.PayTermCatId,
            //                     order.Memo,
            //                     order.TotalAmount,
            //                     order.ProductName,
            //                     order.Options,
            //                     order.SubTotal,
            //                     order.Qty,
            //                     order.OrderState,
            //                     bm.LogoImgFileName,
            //                     bAdd.Address,
            //                     payTerm.PaymentType,
            //                     nm.MemberPhotoFile
            //                 };

            var orderDatas = from order in _context.Orders
                             join f in _context.ViewShowFullOrders on order.Fid equals f.OrderFid
                             into group7
                             from g7 in group7.DefaultIfEmpty()
                             join bm in _context.BusinessImgs on g7.BFid equals bm.Fid
                             into group2
                             from g2 in group2.DefaultIfEmpty()
                             join b in _context.BusinessMembers on order.BFid equals b.Fid
                             into group3
                             from g3 in group3.DefaultIfEmpty()
                             join pay in _context.PaymentTermCategories on order.PayTermCatId equals pay.Fid
                             into group4
                             from g4 in group4.DefaultIfEmpty()
                             join nm in _context.NormalMembers on order.NFid equals nm.Fid
                             into group5
                             from g5 in group5.DefaultIfEmpty()
                             where order.Fid == (int)id
                             select new
                             {
                                 g7.BMemberPhone,
                                 g7.BMemberName,
                                 order.OrderISerialId,
                                 order.PickUpDate,
                                 order.PickUpPerson,
                                 order.PickUpPersonPhone,
                                 order.PickUpTime,
                                 order.PickUpType,
                                 order.PayTermCatId,
                                 order.Memo,
                                 order.TotalAmount,
                                 g7.ProductName,
                                 order.OrderState,
                                 g7.Options,
                                 g7.Qty,
                                 g7.SubTotal,
                                 g2.LogoImgFileName,
                                 g3.Address,
                                 g4.PaymentType,
                                 g5.MemberPhotoFile
                             };

            if (orderDatas != null)
            {
                CANormalMemberOrderViewModel n = new CANormalMemberOrderViewModel();
                List<CANormalMemberOrderDetailViewModel> items = new List<CANormalMemberOrderDetailViewModel>();
                foreach (var vsf in orderDatas.Distinct())
                {
                    CANormalMemberOrderDetailViewModel detail = new CANormalMemberOrderDetailViewModel();
                    detail.productName=vsf.ProductName;
                    detail.Options = vsf.Options + "/" + "$" + vsf.SubTotal + "/" + vsf.Qty + "份";
                    items.Add(detail);
                }
                n.details = items;
                n.BMemberName = orderDatas.Distinct().ToList()[0].BMemberName;
                n.BMemberPhone = orderDatas.Distinct().ToList()[0].BMemberPhone;
                n.OrderISerialId = orderDatas.Distinct().ToList()[0].OrderISerialId;
                n.PickUpDate = orderDatas.Distinct().ToList()[0].PickUpDate;
                n.PickUpTime = orderDatas.Distinct().ToList()[0].PickUpTime;
                n.TotalAmount = orderDatas.Distinct().ToList()[0].TotalAmount;
                n.PickUpType = orderDatas.Distinct().ToList()[0].PickUpType;
                n.PickUpPerson = orderDatas.Distinct().ToList()[0].PickUpPerson;
                n.PickUpPersonPhone = orderDatas.Distinct().ToList()[0].PickUpPersonPhone;
                n.Memo = orderDatas.Distinct().ToList()[0].Memo;
                n.businessImgFile = orderDatas.Distinct().ToList()[0].LogoImgFileName;
                n.businessAddress = orderDatas.Distinct().ToList()[0].Address;
                n.PayTermCatName = orderDatas.ToList()[0].PaymentType;
                n.OrderState = orderDatas.Distinct().ToList()[0].OrderState;
                n.normalImgFile= orderDatas.Distinct().ToList()[0].MemberPhotoFile;

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
        
        public IActionResult loadImgInfo(string fid)//載入廣告圖片資訊
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
        
        public IActionResult changeAdOrderBy(string data)//移動廣告圖片時儲存圖片排序
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
        public IActionResult saveAdInfo(string data)//儲存小圖片的資訊
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
        public IActionResult AaddAdImg(string data)//上傳並儲存新廣告圖片
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
                ad.EndTime = DateTime.Now.AddYears(3);
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
        public IActionResult saveSmallImgData(string data)//上傳並儲存新小圖片
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
        public IActionResult ASetting()//管理者帳密頁面
        {
            CASettingViewModel admin = new CASettingViewModel();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_ADMIN))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_ADMIN);
                AdminMember member = System.Text.Json.JsonSerializer.Deserialize<AdminMember>(json);
                if (member != null)
                    admin.admin = member;
            }
            
            return View(admin);
        }
        
        public IActionResult saveAdminPwd(string data)//修改管理者帳密
        {
            string result = "0";
            
            if (!string.IsNullOrEmpty(data))
            {
                CASettingViewModel cas = JsonConvert.DeserializeObject<CASettingViewModel>(data);

                AdminMember updateItem = _context.AdminMembers.FirstOrDefault(a => a.Fid == cas.Fid);
                updateItem.Password = cas.txtPassword;           

                _context.SaveChanges();
                result = "1";
            }

            return Content(result);
        }
        public IActionResult AForgotAdminPwd(string data)//忘記密碼
        {
            string result = "0";
            if (!string.IsNullOrEmpty(data))
            {
                CForgetPwdViewModel fm = JsonConvert.DeserializeObject<CForgetPwdViewModel>(data);
                
                //一般會員
                if (fm.memberType == "N")
                {
                    var user = _context.NormalMembers.FirstOrDefault(u => u.Phone == fm.txtAccount && u.Email==fm.txtEmail);
                    if (user == null)
                        return Json("登入帳號或信箱不符");
                    
                    result=setForgetPwdMail(fm);                        
                    
                }
                //網站管理者
                else if (fm.memberType == "A")
                {
                    var user = _context.AdminMembers.FirstOrDefault(u => u.Account == fm.txtAccount && u.Email==fm.txtEmail);
                    if (user == null)
                        return Json("登入帳號或信箱不符");

                    result=setForgetPwdMail(fm);
                        #region ADO.NET測試
                        //using (SqlConnection conn = new SqlConnection(connStr))
                        //{
                        //    conn.Open();
                        //    using (SqlCommand cmd = new SqlCommand())
                        //    {
                        //        cmd.Connection = conn;
                        //        cmd.CommandText = "insert into ChangeRequestPassword(Token,Account,Email) values(@Token,@Account,@Email)";
                        //        cmd.Parameters.AddWithValue("Token", token);
                        //        cmd.Parameters.AddWithValue("Account", fm.txtAccount);
                        //        cmd.Parameters.AddWithValue("Email", fm.txtEmail);
                        //        try
                        //        {
                        //            cmd.ExecuteNonQuery();
                        //            result = "success";
                        //        }
                        //        catch (Exception err)
                        //        {
                        //            result = $"error:{err.Message}";
                        //        }
                        //    }
                        //}
                        #endregion
                        
                }
                
            }
            
            return Json(result);
        }

        private string setForgetPwdMail(CForgetPwdViewModel fm)//DB建立忘記密碼請求並發信
        {
            string result;            
            string token = Guid.NewGuid().ToString();
            string url = $"https://localhost:7266/Admin/ResetPwd?token={token}&acc={fm.txtAccount}&tp={fm.memberType}";

            #region ADO.NET測試
            //var connStr = _config["ConnectionStrings:localconnection"];
            //using (SqlConnection conn = new SqlConnection(connStr))
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.Connection = conn;
            //        cmd.CommandText = "insert into ChangeRequestPassword(Token,Account,Email) values(@Token,@Account,@Email)";
            //        cmd.Parameters.AddWithValue("Token", token);
            //        cmd.Parameters.AddWithValue("Account", fm.txtAccount);
            //        cmd.Parameters.AddWithValue("Email", fm.txtEmail);
            //        try
            //        {
            //            cmd.ExecuteNonQuery();
            //            result = "success";
            //        }
            //        catch (Exception err)
            //        {
            //            result = $"error:{err.Message}";
            //        }
            //    }
            //}
            #endregion
            ChangeRequestPassword request = new ChangeRequestPassword();
            request.Token=token;
            request.Account=fm.txtAccount;
            request.Email=fm.txtEmail;
            request.Expire=DateTime.Now.AddMinutes(10);
            _context.ChangeRequestPasswords.Add(request);
            try
            {
                _context.SaveChanges();
                result = "success";
            }
            catch (Exception err)
            {
                result = $"error:{err.Message}";
            }

            string mailBody = $"您好：<br>我們收到了您發送的忘記密碼通知。<br>" +
                                $"請確認是您本人發出的請求後，請在<label style='color:red'><b>10分鐘內</b></label>點擊以下網址連結到修改密碼的頁面後輸入新密碼。" +
                                "<br>如果您沒有發出請求，則可忽略此信。<br><br>" +
                                $"<a href='{url}' target='_blank'>★★★修改密碼★★★</a><br><br>" +
                                "<hr>" +
                                "<br><br>此為系統通知，請勿直接回信，謝謝";
            string mailSubject = $"修改密碼通知";
            result +=" "+sendMail(fm.txtEmail, mailBody, mailSubject);
            return result;
        }

        public IActionResult ResetPwd(string token,string acc,string tp)//忘記密碼的重設密碼頁
        {
            string expire = "";
            #region ADO.NET測試
            //var connStr = _config["ConnectionStrings:localconnection"];            
            //using (SqlConnection conn = new SqlConnection(connStr))
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.Connection = conn;
            //        cmd.CommandText = "select top(1) Expire from ChangeRequestPassword where Account=@Account order by Expire desc";
            //        cmd.Parameters.AddWithValue("Account", acc);
            //        SqlDataReader reader = cmd.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            expire = reader["Expire"].ToString();
            //        }
            //        reader.Close();
            //    }
            //}
            #endregion

            ChangeRequestPassword request = _context.ChangeRequestPasswords.FirstOrDefault(r => r.Token==token);
            if(request!=null)            
                expire = request.Expire.ToString();            

            if (expire == "" || DateTime.Now > Convert.ToDateTime(expire))
            {
                if (tp.ToUpper() == "A") ;
                    return RedirectToAction("ALogin");
                if (tp.ToUpper() == "N") ;
                    return RedirectToAction("Login", "CustomerMember");
            }
                

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_RESETPWD_EXPIRE))                
                HttpContext.Session.Remove(CDictionary.SK_RESETPWD_EXPIRE);                    
                
            HttpContext.Session.SetString(CDictionary.SK_RESETPWD_EXPIRE, expire);

            return View();
            
        }
        public IActionResult submitResetPwd(CResetPwdViewModel reset)//忘記密碼的送出重設密碼
        {
            string result = "";
            if (reset.txtPassword == reset.txtConfirmPwd)
            {                
                string expire= "";
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_RESETPWD_EXPIRE))                
                    expire = HttpContext.Session.GetString(CDictionary.SK_RESETPWD_EXPIRE);

                if (expire != "")
                {
                    DateTime expireTime = Convert.ToDateTime(expire);
                    if (DateTime.Now < expireTime)
                    {
                        if (reset.tp.ToUpper()=="N")
                        {
                            NormalMember user = _context.NormalMembers.FirstOrDefault(u => u.Phone == reset.txtAccount);
                            if (user != null)
                            {
                                user.Password = reset.txtPassword;
                                try
                                {
                                    _context.SaveChanges();
                                    result += "success";
                                }
                                catch (Exception err)
                                {
                                    result += $"error:{err.Message}";
                                }

                                #region ADO.NET測試
                                //var connStr = _config["ConnectionStrings:localconnection"];
                                //using (SqlConnection conn = new SqlConnection(connStr))
                                //{
                                //    conn.Open();
                                //    using (SqlCommand cmd = new SqlCommand())
                                //    {
                                //        cmd.Connection = conn;
                                //        cmd.CommandText = "delete from ChangeRequestPassword where Expire<=@Expire";
                                //        cmd.Parameters.AddWithValue("Expire", Convert.ToDateTime(expire));
                                //        try
                                //        {
                                //            cmd.ExecuteNonQuery();
                                //            result += "success";
                                //        }
                                //        catch(Exception err)
                                //        {
                                //            result += $"error:{err.Message}";
                                //        }

                                //    }
                                //}
                                #endregion

                                result+= " "+deleteChangePwdRequest(expireTime, reset.token, reset.txtAccount);

                            }
                            else
                                result="error:Wrong Account";
                        }
                        else if (reset.tp.ToUpper()=="A")
                        {
                            AdminMember user = _context.AdminMembers.FirstOrDefault(u => u.Account == reset.txtAccount);
                            if (user != null)
                            {
                                user.Password = reset.txtPassword;
                                try
                                {
                                    _context.SaveChanges();
                                    result += "success";
                                }
                                catch (Exception err)
                                {
                                    result += $"error:{err.Message}";
                                }

                                result+= " "+deleteChangePwdRequest(expireTime, reset.token, reset.txtAccount);

                            }
                            else
                                result="error:Wrong Account";
                        }

                    }
                    HttpContext.Session.Remove(CDictionary.SK_RESETPWD_EXPIRE);
                }
                else
                    result="error:Link Expired";

            }
            
            return Json(result);
        }

        private string deleteChangePwdRequest(DateTime expireTime,string token,string acc)//重設密碼後刪除DB的忘記密碼請求
        {
            string result = "";
            ChangeRequestPassword request = _context.ChangeRequestPasswords.FirstOrDefault(r =>r.Token==token);
            if (request!=null)
            {
                var deleteItem = _context.ChangeRequestPasswords.Where(r => r.Account==acc);
                _context.ChangeRequestPasswords.RemoveRange(deleteItem.ToList());
                try
                {
                    _context.SaveChanges();
                    result += "success";
                }
                catch (Exception err)
                {
                    result += $"error:{err.Message}";
                }

            }

            return result;
        }

        private string sendMail(string email,string mailBody,string mailSubject)//發信
        {            
            var DemoMailServer = _config["DemoMailServer:pwd"];
            MailMessage MyMail = new MailMessage();
            MyMail.From = new MailAddress("ShibaAdmin@msit145shiba.com.tw", "日柴", System.Text.Encoding.UTF8);
            MyMail.To.Add(email);
            
            MyMail.Subject = mailSubject;            
            MyMail.Body = mailBody; //設定信件內容
            MyMail.IsBodyHtml = true; //是否使用html格式

            SmtpClient MySMTP = new SmtpClient();
            //MySMTP.UseDefaultCredentials = true;
            MySMTP.Credentials = new System.Net.NetworkCredential("b9809004@gapps.ntust.edu.tw", DemoMailServer); //這裡要填正確的帳號跟密碼
            MySMTP.Host = "smtp.gmail.com"; //設定smtp Server
            MySMTP.Port = /*587*/25;
            MySMTP.EnableSsl = true; //gmail預設開啟驗證
            

            try
            {
                MySMTP.Send(MyMail);                
                return "success";
            }
            catch (Exception ex)
            {
                return $"Mail error:{ex.ToString()}";                
            }
            finally
            {
                MyMail.Dispose(); //釋放資源
            }
            
        }

    }
}
