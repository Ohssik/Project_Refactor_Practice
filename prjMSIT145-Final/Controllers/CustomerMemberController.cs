using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Protocol;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Composition;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        public ActionResult Login(CLoginViewModel vm)                                                                             //登入動作
        {


            NormalMember x = _context.NormalMembers.FirstOrDefault(t => t.Phone.Equals(vm.txtAccount) && t.Password.Equals(vm.txtPassword));
            if (x != null)
            {
                if (x.Password.Equals(vm.txtPassword) && x.Phone.Equals(vm.txtAccount))
                {
                    if (x.IsSuspensed == 0) {
                        string json = System.Text.Json.JsonSerializer.Serialize(x);
                        HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json);

                        return Redirect("~/Home/CIndex");
                    }

                    return View();
                    
                }
            }
            return View();
        }

        public ActionResult loginmailverify(CLoginViewModel vm)                                                              //登入顯示會員狀態
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
        public IActionResult ValidGoogleLogin()                                                                                                  //google
        {
            string? formCredential = Request.Form["credential"]; //回傳憑證
            string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload? payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
            if (payload == null)
            {
                // 驗證失敗
               
                return Redirect("~/Home/CIndex");
            }
            else
            {
                NormalMember member = _context.NormalMembers.FirstOrDefault(c => c.GoogleEmail == payload.Email);
                if (member != null)
                {
                    CLoginViewModel vm = new CLoginViewModel();
                    vm.txtAccount = member.Phone;
                    vm.txtPassword = member.Password;
                  
                    Login(vm);
                }
                else
                {
                    NormalMember x = new NormalMember();

                    x.GoogleEmail = payload.Email;
                    x.MemberName = payload.Name;

                    return RedirectToAction("Register", x);

                }

            };
            return Redirect("~/Home/CIndex");
        }

        /// <summary>
        /// 驗證 Google Token
        /// </summary>
        /// <param name="formCredential"></param>
        /// <param name="formToken"></param>
        /// <param name="cookiesToken"></param>
        /// <returns></returns>
        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)   //google
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload? payload;
            try
            {
                // 驗證 token
                if (formToken != cookiesToken)
                {
                    return null;
                }

                // 驗證憑證
                IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleApiClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return payload;
        }

        public ActionResult LineLoginDirect()                                                                                    //line
        {
            string response_type = "code";
            string client_id = "1657900734";
            string redirect_uri = HttpUtility.UrlEncode("https://localhost:7266/CustomerMember/CALLBACKLOGIN");
            string state = "aaa";
            string LineLoginUrl = string.Format("https://access.line.me/oauth2/v2.1/authorize?response_type={0}&client_id={1}&redirect_uri={2}&state={3}&scope=openid%20profile&nonce=09876xyz",
                response_type,
                client_id,
                redirect_uri,
                state
                );
            return Redirect(LineLoginUrl);
        }

        public ActionResult CALLBACKLOGIN(string code, string state)                                                               //line callback
        {
            if (state == "aaa")
            {
                #region Api變數宣告
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string result = string.Empty;
                NameValueCollection nvc = new NameValueCollection();
                #endregion
                try
                {
                    //取回Token
                    string ApiUrl_Token = "https://api.line.me/oauth2/v2.1/token";
                    nvc.Add("grant_type", "authorization_code");
                    nvc.Add("code", code);
                    nvc.Add("redirect_uri", "https://localhost:7266/CustomerMember/CALLBACKLOGIN");
                    nvc.Add("client_id", "1657900734");
                    nvc.Add("client_secret", "8a686ccc1f01658c94ff67511e5d46b3");
                    string JsonStr = Encoding.UTF8.GetString(wc.UploadValues(ApiUrl_Token, "POST", nvc));
                    LineLoginToken ToKenObj = JsonConvert.DeserializeObject<LineLoginToken>(JsonStr);
                    wc.Headers.Clear();

                    //取回User Profile
                    string ApiUrl_Profile = "https://api.line.me/v2/profile";
                    wc.Headers.Add("Authorization", "Bearer " + ToKenObj.access_token);
                    string UserProfile = wc.DownloadString(ApiUrl_Profile);
                    LineProfile ProfileObj = JsonConvert.DeserializeObject<LineProfile>(UserProfile);
                    NormalMember member = _context.NormalMembers.FirstOrDefault(c => c.LineUserid == ProfileObj.userId);
                    if (member != null)
                    {
                        

                        CLoginViewModel loginmember =new CLoginViewModel();
                        loginmember.txtAccount = member.Phone;
                        loginmember.txtPassword = member.Password;
                        if (member.IsSuspensed != 0)
                        {
                            return RedirectToAction("Login", new { isSus = "f" });
                        }
                        else {

                            Login(loginmember);
                        }
                           

                    }
                    else
                    {      
                        NormalMember newmember = new NormalMember();
                        newmember.LineUserid=ProfileObj.userId;
                        newmember.MemberName = ProfileObj.displayName;
                        newmember.MemberPhotoFile = ProfileObj.pictureUrl;
                        return View("Register", newmember);                     
                    }
                   
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    throw;
                }
            }
            return Redirect("~/Home/CIndex");
        }

             public class LineLoginToken                                                                              //line
             {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string id_token { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
            }

            public class LineProfile
            {
            public string userId { get; set; }
            public string displayName { get; set; }
            public string pictureUrl { get; set; }
            public string statusMessage { get; set; }
         }





        public ActionResult Loginout()                                                                                          //登出
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGINED_USER);

            return Redirect("~/Home/CIndex");
            //return RedirectToAction("Index", "CustomerMember");
        }
        public IActionResult Register(NormalMember member)
        {

            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> Register(CNormalMemberViewModel vm, IFormFile photo)                                               //註冊動作
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
                    //return View();
                    return await Task.Run(() => View());

                }
            }
            if (vm.MemberName == null && vm.Password == null)
            {
                //return View();
                return await Task.Run(() => View());
            }
            if (vm.Phone == null)
            {
                //return View();
                return await Task.Run(() => View());
            }
            else
            {
                bool correct = Regex.IsMatch(vm.Phone, @"^09[0-9]{8}$");
                if (!(correct))
                {
                    //return View();
                    return await Task.Run(() => View());
                }
                
            }
            if (vm.Email == null)
            {
                //return View();
                return await Task.Run(() => View());
            }
            else
            {
                bool correct = Regex.IsMatch(vm.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!(correct))
                {
                    //return View();
                    return await Task.Run(() => View());
                }
            }
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            if (vm.MemberPhotoFile != null)
            {
                string imgaeURL = vm.MemberPhotoFile;

                string filePath2 = Path.Combine(_eviroment.WebRootPath, "images/Customer/Member", fileName);
                using (HttpClient client = new HttpClient())
                {
                    using (var response = await client.GetAsync(imgaeURL))
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            using (var fileStream = System.IO.File.Create(filePath2))
                            {
                                await stream.CopyToAsync(fileStream);
                            }
                        }
                    }
                }
                vm.MemberPhotoFile = fileName;
            }
            if (photo != null)
            {
                //string fileName = Guid.NewGuid().ToString() + ".jpg";
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

            //聊天室相關
            ChatroomUser chatroomUser = new ChatroomUser();
            chatroomUser.UserType = 0;//0是客戶 1是商家 2 是平台 欄位改成INT
            chatroomUser.Memberfid = vm.member.Fid;
            _context.ChatroomUsers.Add(chatroomUser);
            _context.SaveChanges();
            vm.member.ChatroomUserid = chatroomUser.ChatroomUserid;
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
            return await Task.Run(() => Redirect("~/Home/CIndex"));
            //return Redirect("~/Home/CIndex");

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
                    string token = Guid.NewGuid().ToString();
                    ChangeRequestPassword request = new ChangeRequestPassword();
                    request.Token = token;
                    request.Account = member.Phone;
                    request.Email = member.Email;
                    request.Expire = DateTime.Now.AddMinutes(10);
                    _context.ChangeRequestPasswords.Add(request);
                    _context.SaveChanges();



                    string smtpAddress = "smtp.gmail.com";
                    //設定Port
                    int portNumber = 587;
                    bool enableSSL = true;
                    //填入寄送方email和密碼
                    //string url = $"https://localhost:7266/CustomerMember/forgetalterpassword/?token={token}&Fid={member.Fid}";
                    string url = $"https://localhost:7266/CustomerMember/forgetalterpassword/?token={request.Token}&Account={request.Account}&Email={request.Email}";
                    string emailFrom = "a29816668@gmail.com";
                    string emailpassword = "joksdquaswjdyzpu";
                    //收信方email 可以用逗號區分多個收件人
                    string emailTo = vm.Email;
                    //主旨
                    string subject = "重製密碼";
                    //內容
                    string body = $"<h2>重製密碼</h2><h3><br><a href={url}>請在<span style='color:red;'>10分鐘內</span>點此連結重設密碼</a></h3>";
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

                    //return Redirect("~/Home/CIndex");
                    return Redirect("~/Home/CIndex");

                }

            }

                
        }
        public IActionResult Forgetpasswordapi(CNormalMemberViewModel vm)
        {
            if (vm.Email!= null && vm.Phone!=null)
            {
                NormalMember member =_context.NormalMembers.FirstOrDefault(c=>c.Email== vm.Email && c.Phone==vm.Phone);
                if (member != null) {
                    return Json("已送出重置密碼信件,請於10鐘內重製密碼");
                    }
                else
                {
                    return Json("帳號或Email錯誤");
                }
                
            }

                 return Json("請兩格都不要空白");
        }


        public IActionResult forgetalterpassword(string Account,string token ,string Email)
        {

            ChangeRequestPassword request = _context.ChangeRequestPasswords.FirstOrDefault(c => c.Token == token);
            if (request != null)
            {
                string expire = request.Expire.ToString();
                if (DateTime.Now > Convert.ToDateTime(expire))
                {
                    
                    return Redirect("~/Home/CIndex");

                }
                else
                {
                    if ( Account!= null && Email!=null)
                    {
                        //NormalMember member = _context.NormalMembers.FirstOrDefault(c => c.Fid == Fid);
                        if (request != null)
                        {
                            CResetPwdViewModel creset=new CResetPwdViewModel();
                            creset.txtAccount = request.Account;
                            creset.token = request.Token;
                            return View(creset);
                            //return View(member);
                        }
                        return Redirect("~/Home/CIndex");

                    }

                }

            }
            return Redirect("~/Home/CIndex");
            
           
        }
        [HttpPost]
        public IActionResult forgetalterpassword(CResetPwdViewModel vm)
        {
            if( vm.txtPassword !=vm.txtConfirmPwd)
            {
                return View(vm);
            }
            else
            {
                NormalMember x=_context.NormalMembers.FirstOrDefault(c=>c.Phone==vm.txtAccount);
                
                if (x == null)
                {
                    return View();
                }
                else
                {
                    ChangeRequestPassword request = _context.ChangeRequestPasswords.First(c => c.Token == vm.token && c.Account == vm.txtAccount);
                    x.Password = vm.txtPassword;
                    _context.ChangeRequestPasswords.Remove(request);
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

            result += " ";
            #region ADO.NET測試
            //var connStr = _config["ConnectionStrings:ispanMsit145shibaconnection"];
            //using (SqlConnection conn = new SqlConnection(connStr))
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.Connection = conn;
            //        cmd.CommandText = "insert into ServiceMailBox(SenderName,Email,Phone,Subject,Context,ReceivedTime)" +
            //            "values(@SenderName,@Email,@Phone,@Subject,@Context,@ReceivedTime)";
            //        cmd.Parameters.AddWithValue("SenderName", mail.txtSenderName);
            //        cmd.Parameters.AddWithValue("Email", mail.txtEmailAddress);
            //        cmd.Parameters.AddWithValue("Phone", mail.txtPhone);
            //        cmd.Parameters.AddWithValue("Subject", mail.txtMailSubject);
            //        cmd.Parameters.AddWithValue("Context", mail.txtMailContent);
            //        cmd.Parameters.AddWithValue("ReceivedTime", DateTime.Now);
            //        try
            //        {
            //            cmd.ExecuteNonQuery();
            //            result += "success";
            //        }
            //        catch (Exception err)
            //        {
            //            result += $"error:{err.Message}";
            //        }

            //    }
            //}
            #endregion

            try
            {
                ServiceMailBox sm = new ServiceMailBox();
                               
                sm.SenderName = mail.txtSenderName;
                sm.Email = mail.txtEmailAddress;
                sm.Phone = mail.txtPhone;
                sm.Subject = mail.txtMailSubject;
                sm.Context = mail.txtMailContent;
                sm.ReceivedTime = DateTime.Now;
                _context.ServiceMailBoxes.Add(sm);
                _context.SaveChanges();
                
                result += "success";
            }
            catch (Exception err)
            {
                result += $"error:{err.Message}";
            }


            return Json(result);
        }
    }
}
