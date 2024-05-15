using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Web.ViewModels;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace prjMSIT145Final.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        private readonly IConfiguration _config;
        public CouponController(ispanMsit145shibaContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public IActionResult Index()
        {           
            return View();
        }
        
        public IActionResult ACouponList()
        {
            List<CACouponViewModel> list = new List<CACouponViewModel>();            

            var coupons = from c in _context.Coupons
                          join cn in _context.Coupon2NormalMembers on c.Fid equals cn.CouponId
                          into sGroup
                          from s in sGroup.DefaultIfEmpty()
                          join n in _context.NormalMembers on s.MemberId equals n.Fid
                          into sg2
                          from sg in sg2.DefaultIfEmpty()
                          select new
                          {
                              c.Fid,
                              c.Price,
                              c.CouponCode,
                              c.Title,
                              c.Memo,
                              c.IsUsed,
                              NmemberID = s.MemberId,
                              NmemberName = sg.MemberName
                          };

            if (coupons != null)
            {
                foreach (var c in coupons.ToList())
                {
                    CACouponViewModel cvm = new CACouponViewModel();
                    cvm.Fid = c.Fid;
                    cvm.Price = c.Price;
                    cvm.CouponCode = c.CouponCode;
                    cvm.Memo = c.Memo;
                    cvm.Title = c.Title;
                    cvm.IsUsed = c.IsUsed;
                    cvm.NmemberID=c.NmemberID;
                    cvm.NmemberName=c.NmemberName;
                    list.Add(cvm);
                }

            }

            return View(list);
            
        }
        public IActionResult ACouponEdit(string data)
        {
            string result = "0";
            CACouponViewModel cac=JsonConvert.DeserializeObject<CACouponViewModel>(data);
            if (cac != null)
            {
                if (cac.Fid > 0)
                {
                    Coupon coup = _context.Coupons.FirstOrDefault(t => t.Fid == cac.Fid);
                    if (coup != null)
                    {
                        coup.Price = cac.Price;
                        coup.Memo = cac.Memo;
                        coup.Title = cac.Title;
                        coup.IsUsed = cac.IsUsed;
                        
                        Coupon2NormalMember c2n = _context.Coupon2NormalMembers.FirstOrDefault(c => c.CouponId==cac.Fid);
                        if (c2n!=null)
                        {
                            c2n.MemberId = cac.NmemberID;
                        }
                        else
                        {
                            c2n=new Coupon2NormalMember();
                            c2n.MemberId = cac.NmemberID;
                            c2n.CouponId=cac.Fid;
                            _context.Coupon2NormalMembers.Add(c2n);
                                
                        }
                            
                        
                    }
                }else if (cac.Fid==0)
                {
                    Coupon newCoup = new Coupon();                    
                    newCoup.Price = cac.Price;
                    newCoup.Memo = cac.Memo;
                    newCoup.Title = cac.Title;
                    newCoup.IsUsed = cac.IsUsed;
                    newCoup.CouponCode=Guid.NewGuid().ToString().Substring(0,10);
                    _context.Coupons.Add(newCoup);
                    _context.SaveChanges();
                    if (cac.NmemberID!=null)
                    {
                        Coupon2NormalMember c2n = new Coupon2NormalMember();
                                                        
                        c2n.MemberId = cac.NmemberID;

                        int fid = 1;
                        Coupon lastCoup = _context.Coupons.OrderByDescending(c => c.Fid).FirstOrDefault();
                        if (lastCoup != null)
                            fid=lastCoup.Fid;

                        c2n.CouponId=fid;
                        _context.Coupon2NormalMembers.Add(c2n);                            

                    }
                }
                

                _context.SaveChanges();
                result="1";
            }
           
            return Content(result);
            //return NoContent();

        }
        
        public IActionResult ACouponDelete(int id)
        {
            //var coup = await _context.Coupons.FindAsync(id);
            Coupon coup = _context.Coupons.FirstOrDefault(c => c.Fid==id);
            if (coup == null)
            {
                return NotFound();
            }
            Coupon2NormalMember c2n = _context.Coupon2NormalMembers.FirstOrDefault(c => c.CouponId==id);
            if (c2n!=null)
                _context.Coupon2NormalMembers.Remove(c2n);
            _context.Coupons.Remove(coup);
            _context.SaveChanges();

            return RedirectToAction("ACouponList");
        }

        public IActionResult CCouponExchange()
        {
            int memberId = 0;
            List<CACouponViewModel> list=new List<CACouponViewModel>();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                NormalMember user = JsonConvert.DeserializeObject<NormalMember>(json);
                if (user != null)
                {
                    memberId = user.Fid;
                    var coupons = from c2n in _context.Coupon2NormalMembers
                                  join coup in  _context.Coupons on c2n.CouponId equals coup.Fid
                                  into cGroup
                                  from c in cGroup.DefaultIfEmpty()
                                  where c2n.MemberId==memberId && c.IsUsed==1
                                  select new
                                  {
                                      c2n.CouponId,
                                      c.Title,
                                      c.Price,
                                      c.Memo,
                                      c2n.Fid

                                  };
                    foreach(var c in coupons.OrderByDescending(c=>c.Fid))
                    {
                        CACouponViewModel cac = new CACouponViewModel();
                        cac.Fid = (int)c.CouponId;
                        cac.Title = c.Title;
                        cac.Price = c.Price;
                        cac.Memo = c.Memo;
                        cac.NmemberID = memberId;
                        list.Add(cac);
                    }

                }
            }
            return View(list.AsEnumerable());
        }
        
        public IActionResult CSubmitCouponExchange(string data)
        {            
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                return Json("尚未登入會員");

            string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            NormalMember user = JsonConvert.DeserializeObject<NormalMember>(json);
            Coupon coup = _context.Coupons.FirstOrDefault(c => c.CouponCode.Equals(data));
            var coupInfo = from c in _context.Coupons
                          join cn in _context.Coupon2NormalMembers on c.Fid equals cn.CouponId
                          into sGroup
                          from s in sGroup.DefaultIfEmpty()
                          join n in _context.NormalMembers on s.MemberId equals n.Fid
                          into sg2
                          from sg in sg2.DefaultIfEmpty()
                          where c.CouponCode==data
                          select new
                            {
                                c.Fid,
                                c.Price,
                                c.CouponCode,
                                c.Memo,
                                c.Title,
                                c.IsUsed,
                                NmemberID = s.MemberId,
                                NmemberName = sg.MemberName
                            };
            if (user!=null)
            {
                if (coup==null || coup.IsUsed!=1)                
                    return Json("優惠券代碼錯誤");                                
                                    
                if (!string.IsNullOrEmpty(coupInfo.ToList()[0].NmemberName))
                    return Json("優惠券已被兌換過");

                Coupon2NormalMember c2n = new Coupon2NormalMember();
                c2n.MemberId=user.Fid;
                c2n.CouponId=coup.Fid;
                _context.Coupon2NormalMembers.Add(c2n);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception err)
                {
                    return Json($"error:{err.Message}");
                }

                return Json(coup);
                                    
                    
            }

            return NoContent();
                
        }
        public IActionResult CSimpleLogin(CLoginViewModel login)
        {
            string result = "0";
            NormalMember x = _context.NormalMembers.FirstOrDefault(t => t.Phone.Equals(login.txtAccount) && t.Password.Equals(login.txtPassword));
            if (x != null)
            {
                if (x.IsSuspensed == 0)
                {
                    string json = JsonConvert.SerializeObject(x);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);
                    result = "1";
                }
                else
                    result = "-1";
            }
            return Content(result);
        }
        public IActionResult CCoupons2Member()
        {
            List<CACouponViewModel> list = new List<CACouponViewModel>();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                NormalMember user = JsonConvert.DeserializeObject<NormalMember>(json);
                
                if (user != null)
                {
                    var coupons = from c2n in _context.Coupon2NormalMembers
                                  join coup in _context.Coupons on c2n.CouponId equals coup.Fid                                  
                                  where c2n.MemberId == user.Fid 
                                  select new
                                  {
                                      c2n.CouponId,
                                      coup.Title,
                                      coup.Price,
                                      coup.Memo,
                                      coup.IsUsed,
                                      c2n.Fid

                                  };

                    if (coupons != null)
                    {
                        foreach (var c in coupons.OrderByDescending(c => c.Fid))
                        {
                            CACouponViewModel cac = new CACouponViewModel();
                            cac.Fid = (int)c.CouponId;
                            cac.Title = c.Title;
                            cac.Price = c.Price;
                            cac.Memo = c.Memo;
                            cac.IsUsed = c.IsUsed;
                            cac.NmemberID = user.Fid;
                            list.Add(cac);
                        }

                    }
                }

            }
            return View(list);            
        }

        public IActionResult CChangeCouponOwner(string data)
        {
            //todo 轉贈優惠券
            if (!string.IsNullOrEmpty(data))
            {
                CChangeCouponOwnerViewModel cco = JsonConvert.DeserializeObject<CChangeCouponOwnerViewModel>(data);
                string result = "";
                if (cco != null)
                {
                    string[] NMemberNum = cco.receiverNfid.Split('-');
                    int receiverId = Convert.ToInt32(NMemberNum[1]);
                    NormalMember member = _context.NormalMembers.FirstOrDefault(m => m.Fid == receiverId);
                    if (member == null)
                        return Json("error:Receiver Member ID not exist");
                    
                    string[] coups = cco.coupList.Split(',');
                    List<int> coupIds = new List<int>();
                    foreach (string cid in coups)
                    {
                        int num = Convert.ToInt32(cid);
                        coupIds.Add(num);
                    }

                    if (coupIds.Count == 0)
                        return Json("error:Coupon list is empty");

                    foreach (int id in coupIds)
                    {
                        Coupon2NormalMember c2n = _context.Coupon2NormalMembers.FirstOrDefault(c => c.CouponId == id && c.MemberId == cco.giverNfid);
                        if (c2n == null)
                            return Json($"error:CouponID:{id} not belong to NmemberID:{cco.giverNfid}");
                        
                        c2n.MemberId = receiverId;
                        try
                        {
                            _context.SaveChanges();

                        }
                        catch (Exception err)
                        {
                            return Json($"CouponID:{id} update error:{err.Message} ");
                        }

                    }


                    if (string.IsNullOrEmpty(member.Email))
                        return Json("error:Member Email address is empty");
                    
                    string mailBody = $"{member.MemberName} 先生/小姐 您好:<br>此信件為通知有人轉贈優惠券給您，您可在登入後至「會員中心」>「我的券夾」查看，謝謝。" +
                        $"<br><br>對方傳達給您的悄悄話:<br>" +
                        $"<label style='color:orange'>{cco.txtMessage}</label>"+
                        $"<br><br><hr><br>" +
                        $"此為系統通知信，請勿直接回信，謝謝。";
                    result = "success " + sendMail(member.Email, mailBody, "優惠券轉贈通知");


                    return Json(result);
                }

            }
            return NoContent();
        }

        private string sendMail(string email, string mailBody, string mailSubject)
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
