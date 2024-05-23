using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Web.ViewModels;
using prjMSIT145Final.Web.Parameters;
using prjMSIT145Final.Service.Interfaces;
using MapsterMapper;
using prjMSIT145Final.Service.ParameterDtos;
using prjMSIT145Final.ViewModels.Admin;
using prjMSIT145Final.ViewModels.NormalMember;
using prjMSIT145Final.ViewModels.Order;
using prjMSIT145Final.ViewModels.BusinessMember;
using iProcurementWebApi.Infrastructure.Extensions;
using prjMSIT145Final.Parameters;
using prjMSIT145Final.Helpers;

namespace prjMSIT145Final.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        private readonly INormalMemberService _normalMemberService;
        private readonly IOrderService _orderService;
        private readonly IBusinessMemberService _businessMemberService;
        private readonly IUploadImgHelper _uploadImgHelper;
        private readonly ISendMailHelper _sendMailHelper;
        private readonly IAdImgService _adImgService;
        private readonly IServiceMailService _serviceMailService;

        //private readonly IConfiguration _config;

        public AdminController(ispanMsit145shibaContext context
            ,IAdminService adminService
            ,IMapper mapper
            ,INormalMemberService normalMemberService
            ,IOrderService orderService
            ,IBusinessMemberService businessMemberService
            ,IUploadImgHelper uploadImgHelper
            ,ISendMailHelper sendMailHelper
            ,IAdImgService adImgService
            ,IServiceMailService serviceMailService
            //,IConfiguration config
            )
        {
            _adminService = adminService;
            _mapper = mapper;
            _normalMemberService = normalMemberService;
            _orderService = orderService;
            _businessMemberService = businessMemberService;
            _uploadImgHelper = uploadImgHelper;
            _sendMailHelper = sendMailHelper;
            _adImgService = adImgService;
            _serviceMailService = serviceMailService;
            _context = context;
            //_config = config;
        }

        /// <summary>
        /// 寄送帳號停權/復權通知
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendAccountLockedNotice([FromBody] SendEmailParameter parameter)
        {
            if (parameter != null)
            {                
                await _adminService.SendAccountLockedNotice(_mapper.Map<SendEmailParameterDto>(parameter));
                var mail = await _sendMailHelper.SetAccountLockedNoticeContent(parameter);
                await _sendMailHelper.SendMail(mail);
            }

            return NoContent();
        }

        /// <summary>
        /// 登入畫面
        /// </summary>
        /// <returns></returns>
        public IActionResult ALogin()
        {
            return View();
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ALogin(CheckPwdParameter parameter)
        {
            var member = await _adminService.Get(_mapper.Map<CheckPwdParameterDto>(parameter));
            if (member.Fid == 0)
            {
                ViewData["checkAccountResult"] = "帳號或密碼有誤";
                return View();
            }

            var json = System.Text.Json.JsonSerializer.Serialize(_mapper.Map<AdminMemberViewModel>(member));
            HttpContext.Session.SetString(CDictionary.SK_LOGINED_ADMIN, json);

            return RedirectToAction("ANormalMemberList");
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ALogout()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_ADMIN))
            {
                HttpContext.Session.Remove(CDictionary.SK_LOGINED_ADMIN);
            }

            return RedirectToAction("ALogin");
        }

        /// <summary>
        /// 一般會員列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ANormalMemberList()
        {
            List<CANormalMemberViewModel> list = new List<CANormalMemberViewModel>();

            var members = await _normalMemberService.GetAll();
            var normalMembers = _mapper.Map<IEnumerable<NormalMemberViewModel>>(members);            

            if (normalMembers != null)
            {
                foreach (var n in normalMembers)
                {
                    var cvm = new CANormalMemberViewModel();
                    cvm.normalMember = n;
                    list.Add(cvm);
                }
            }

            return View(list);

        }

        /// <summary>
        /// 一般會員內容(進入畫面)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ANormalMemberDetails(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("ANormalMemberList");
            }

            var memberData = await _normalMemberService.GetById(id.GetValueOrDefault());

            if (memberData.Fid != 0)
            {
                var member = _mapper.Map<NormalMemberViewModel>(memberData);
                var orderDatas = await _orderService.GetByNormalMemberId(id.GetValueOrDefault());
                var orders = _mapper.Map<IEnumerable<OrderViewModel>>(orderDatas);

                CANormalMemberViewModel viewModel = new CANormalMemberViewModel
                {
                    normalMember = member,
                    orders = orders
                };

                return View(viewModel);
            }

            return RedirectToAction("ANormalMemberList");
        }

        /// <summary>
        /// 修改一般會員密碼
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ANormalMemberDetails(CANormalMemberViewModel member)
        {
            if (member != null)
            {
                await _normalMemberService.Modify(
                    new ModifyPwdParameterDto 
                    { 
                        Password = member.txtPassword,
                        ConfirmPwd = member.txtConfirmPwd,
                        Fid = member.Fid
                    });
            }

            return RedirectToAction("ANormalMemberList");
        }

        /// <summary>
        /// 一般會員訂單瀏覽
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ANormalMemberOrder(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("ANormalMemberDetails");
            }

            //var orderDatas = await _orderService.Get(id.GetValueOrDefault());

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
                CANormalMemberOrderViewModel member = new CANormalMemberOrderViewModel();
                List<CANormalMemberOrderDetailViewModel> items = new List<CANormalMemberOrderDetailViewModel>();
                foreach (var vsf in orderDatas.Distinct())
                {                    
                    items.Add(new CANormalMemberOrderDetailViewModel
                    {
                        productName = vsf.ProductName,
                        Options = vsf.Options + "/" + "$"
                                    + ((decimal)vsf.SubTotal).ToString("###,###")
                                    + "/" + vsf.Qty + "份"
                    });
                }
                member.details = items;

                member.BMemberName = orderDatas.Distinct().ToList()[0].BMemberName;
                member.BMemberPhone = orderDatas.Distinct().ToList()[0].BMemberPhone;
                member.OrderISerialId = orderDatas.Distinct().ToList()[0].OrderISerialId;
                member.PickUpDate = orderDatas.Distinct().ToList()[0].PickUpDate;
                member.PickUpTime = orderDatas.Distinct().ToList()[0].PickUpTime;
                member.TotalAmount = orderDatas.Distinct().ToList()[0].TotalAmount;
                member.PickUpType = orderDatas.Distinct().ToList()[0].PickUpType;
                member.PickUpPerson = orderDatas.Distinct().ToList()[0].PickUpPerson;
                member.PickUpPersonPhone = orderDatas.Distinct().ToList()[0].PickUpPersonPhone;
                member.Memo = orderDatas.Distinct().ToList()[0].Memo;
                member.businessImgFile = orderDatas.Distinct().ToList()[0].LogoImgFileName;
                member.businessAddress = orderDatas.Distinct().ToList()[0].Address;
                member.PayTermCatName = orderDatas.Distinct().ToList()[0].PaymentType;
                member.OrderState = orderDatas.Distinct().ToList()[0].OrderState;

                return View(member);
            }

            return RedirectToAction("ANormalMemberDetails");
        }

        /// <summary>
        /// 店家會員列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ABusinessMemberList()
        {
            var list = new List<CABusinessMemberViewModel>();
            var members = await _businessMemberService.GetAll();
            var businessMembers = _mapper.Map<IEnumerable<BusinessMemberViewModel>>(members);

            if (businessMembers != null)
            {
                foreach (BusinessMemberViewModel b in businessMembers)
                {
                    CABusinessMemberViewModel cvm = new CABusinessMemberViewModel();
                    cvm.businessMember = b;
                    list.Add(cvm);
                }
            }

            return View(list);
        }

        /// <summary>
        /// 店家會員內容(畫面)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ABusinessMemberDetails(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("ABusinessMemberList");
            }

            //var businessDatas = _context.BusinessMembers.FirstOrDefault(c => c.Fid == (int)id);
            //IEnumerable<Order> orderDatas = from order in _context.Orders
            //                                where order.BFid == (int)id
            //                                select order;
            //BusinessImg businessLogo = _context.BusinessImgs.FirstOrDefault(bl => bl.BFid == (int)id);

            var businessDatas = await _businessMemberService.GetById((int)id);
            var orderDatas = await _orderService.GetByBusinessMemberId((int)id);
            var businessLogo = await _businessMemberService.GetImgById((int)id);


            if (businessDatas != null)
            {
                CABusinessMemberViewModel b = new CABusinessMemberViewModel();
                b.businessMember = _mapper.Map<BusinessMemberViewModel>(businessDatas);
                if (orderDatas != null)
                {
                    b.orders = _mapper.Map<IEnumerable<OrderViewModel>>(orderDatas);
                }

                if (businessLogo != null)
                {
                    b.LOGO_ImgFileName = businessLogo.LogoImgFileName;
                }
                return View(b);
            }

            return RedirectToAction("ABusinessMemberList");

        }

        /// <summary>
        /// 修改店家會員密碼
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ABusinessMemberDetails(CABusinessMemberViewModel member)
        {
            //if (b != null)
            //{
            //    var user = _context.BusinessMembers.FirstOrDefault(t => t.Fid == b.Fid);
            //    if (!string.IsNullOrEmpty(b.txtPassword) && (b.txtPassword.Trim() == b.txtConfirmPwd))
            //    {
            //        if (user != null)
            //        {
            //            user.Password = b.txtPassword;
            //        }
            //    }
            //    user.IsSuspensed = (int)b.IsSuspensed;
            //    _context.SaveChanges();

            //}

            if (member != null)
            {
                await _businessMemberService.Modify(
                    new ModifyPwdParameterDto
                    {
                        Password = member.txtPassword,
                        ConfirmPwd = member.txtConfirmPwd,
                        Fid = member.Fid
                    });
            }


            return RedirectToAction("ABusinessMemberList");

        }

        /// <summary>
        /// 店家會員訂單瀏覽
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ABusinessMemberOrder(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("ABusinessMemberDetails");
            }


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
                    detail.productName = vsf.ProductName;
                    detail.Options = vsf.Options + "/" + "$" + ((decimal)vsf.SubTotal).ToString("###,###") + "/" + vsf.Qty + "份";
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
                n.normalImgFile = orderDatas.Distinct().ToList()[0].MemberPhotoFile;

                return View(n);
            }

            return RedirectToAction("ABusinessMemberDetails");
        }

        /// <summary>
        /// 廣告圖管理
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ADisplayImgManage()
        {
            var imgs = await _adImgService.GetAllAd();

            List<CAAdImg> list = new List<CAAdImg>();
            foreach (var img in imgs)
            {
                list.Add(new CAAdImg
                {
                    adImg = img
                });
            }
            if (list.Count > 0)
            {
                var ader = await _businessMemberService.GetById(list[0].BFid.GetValueOrDefault());
                if (ader != null)
                {
                    list[0].ImgBelongTo = ader.MemberName;
                }
            }

            return View(list.OrderBy(img=>img.OrderBy));
        }

        /// <summary>
        /// 載入廣告圖片資訊
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public async Task<IActionResult> LoadImgInfo(string fid)
        {
            var img = await _adImgService.GetAdById(Convert.ToInt32(fid));
            var ader = await _businessMemberService.GetById(img.BFid.GetValueOrDefault());

            var cAAdImg = new CAAdImg
            {
                ImgBelongTo = ader.MemberName,
                adImg = img
            };            

            return Json(cAAdImg);
        }

        /// <summary>
        /// 移動廣告圖片時儲存圖片排序
        /// </summary>
        /// <param name="ads"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangeAdOrderBy(IEnumerable<CAAdImg> ads)
        {
            string result = "0";

            if (ads.IsAny())
            {
                var parameters = ads.Select(ad => new AdImg
                {
                    Fid = Convert.ToInt32(ad.sFid),
                    OrderBy = ad.OrderBy
                });

                await _adImgService.ModifyAdsOrderBy(parameters);

                result = "1";
            }

            return Content(result);
        }

        /// <summary>
        /// 儲存小圖片的資訊
        /// </summary>
        /// <param name="adImg"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveAdInfo([FromBody] CAAdImg adImg)
        {
            string result = "0";

            if (adImg != null)
            {
                var ad = new AdImg
                {
                    Fid = Convert.ToInt32(adImg.sFid),
                    ImgName = adImg.ImgName,
                    StartTime = adImg.StartTime,
                    EndTime = adImg.EndTime,
                    Hyperlink = adImg.Hyperlink,
                    OrderBy = adImg.OrderBy
                };

                await _adImgService.ModifyAdInfo(ad);
                result = "1";
            }

            return Content(result);
        }

        /// <summary>
        /// 刪除圖
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteAd([FromForm]string data)
        {
            string result = "0";

            if (!string.IsNullOrEmpty(data))
            {
                await _adImgService.DeleteAd(Convert.ToInt32(data));
                result = "1";
            }

            return Content(result);
        }

        /// <summary>
        /// 上傳並儲存新輪播圖片
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AaddAdImg(string data)
        {
            AdImg returnAd = null;

            if (!string.IsNullOrEmpty(data))
            {
                var parameter = JsonConvert.DeserializeObject<UploadImgParameter>(data);

                var fName = await _uploadImgHelper.UploadAdImg(parameter);
                returnAd = await _adImgService.AddUploadAdInfo(new AdImg
                {
                    ImgName = fName,
                    EndTime = DateTime.Now.AddYears(3)
                });

            }

            return Json(returnAd);
        }

        /// <summary>
        /// 上傳並儲存新小圖片
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveSmallImgData(string data)
        {
            AdImg returnAd = null;

            if (!string.IsNullOrEmpty(data))
            {
                var parameter = JsonConvert.DeserializeObject<UploadImgParameter>(data);

                var fName = await _uploadImgHelper.UploadAdImg(parameter);
                returnAd = await _adImgService.UpdateUploadAdInfo(new AdImg
                {
                    Fid = parameter.Fid.GetValueOrDefault(),
                    Hyperlink = parameter.Hyperlink,
                    ImgName = fName
                });                
            }

            return Json(returnAd);

        }

        /// <summary>
        /// 管理者帳密頁面
        /// </summary>
        /// <returns></returns>
        public IActionResult ASetting()
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

        /// <summary>
        /// 修改管理者帳密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveAdminPwd([FromBody] CASettingViewModel cas)
        {
            string result = "0";

            if (cas != null)
            {
                //CASettingViewModel cas = JsonConvert.DeserializeObject<CASettingViewModel>(data);

                await _adminService.UpdatePwd(new ModifyPwdParameterDto
                {
                    Fid = cas.Fid,
                    Password = cas.txtPassword,
                    Email = cas.Email
                });

                result = "1";
            }

            return Content(result);
        }

        /// <summary>
        /// DB建立忘記密碼請求並發信
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AForgotAdminPwd([FromBody]ForgetPwdParameter parameter)
        {
            var checkResult = false;

            //一般會員
            if (parameter?.memberType == "N")
            {
                checkResult = await _normalMemberService.CheckAccountInfo
                                    (_mapper.Map<ForgetPwdParameterDto>(parameter));
            }
            //網站管理者
            else if (parameter?.memberType == "A")
            {
                checkResult = await _adminService.CheckAccountInfo
                                    (_mapper.Map<ForgetPwdParameterDto>(parameter));
            }

            if (!checkResult)
            {
                return Json("登入帳號或信箱不符");
            }

            var token = Guid.NewGuid().ToString();

            var result = await _adminService.AddChangePasswordRequest(new ChangeRequestPassword
            {
                Token = token,
                Account = parameter?.txtAccount,
                Email = parameter?.txtEmail,
                Expire = DateTime.Now.AddMinutes(10)
            });


            var emailUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
            var url = $"{emailUrl}/Admin/ResetPwd?token={token}&acc={parameter?.txtAccount}&tp={parameter?.memberType}";

            //result = await SetForgetPwdMail(parameter);
            var mail = await _sendMailHelper.SetForgetPwdMailContent(parameter,url, token);
            result += await _sendMailHelper.SendMail(mail);


            return Json(result);
        }

        
        /// <summary>
        /// 忘記密碼的重設密碼頁
        /// </summary>
        /// <param name="token"></param>
        /// <param name="acc"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public async Task<IActionResult> ResetPwd(string token, string tp)
        {
            var result = await _adminService.CheckTokenIsExpired(token);

            if (DateTime.Now > result.Expire.GetValueOrDefault())
            {
                if (tp.ToUpper() == "A")
                {
                    return RedirectToAction("ALogin");
                }
                else if (tp.ToUpper() == "N")
                {
                    return RedirectToAction("Login", "CustomerMember");
                }
            }

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_RESETPWD_EXPIRE))
            {
                HttpContext.Session.Remove(CDictionary.SK_RESETPWD_EXPIRE);
            }

            HttpContext.Session.SetString(CDictionary.SK_RESETPWD_EXPIRE, result.Expire.GetValueOrDefault().ToString());

            return View();

        }

        /// <summary>
        /// 忘記密碼的送出重設密碼
        /// </summary>
        /// <param name="resetModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> SubmitResetPwd(CResetPwdViewModel resetModel)
        {
            string result = "";

            if(resetModel == null)
            {
                return Json(result);
            }

            if (resetModel?.txtPassword != resetModel?.txtConfirmPwd)
            {
                return Json("2次輸入密碼不符");
            }
            
            var expire = HttpContext.Session.Keys.Contains(CDictionary.SK_RESETPWD_EXPIRE)
                ? HttpContext.Session.GetString(CDictionary.SK_RESETPWD_EXPIRE) : string.Empty;            

            var expiredTime = string.IsNullOrEmpty(expire) ? DateTime.MinValue: Convert.ToDateTime(expire);

            if (DateTime.Now > expiredTime)
            {
                return Json("error:Link Expired");
            }

            
            try
            {
                if (resetModel?.tp.ToUpper() == "N")
                {
                    await _normalMemberService.Modify(
                        new ModifyPwdParameterDto
                        {
                            Password = resetModel?.txtPassword,
                            ConfirmPwd = resetModel?.txtConfirmPwd,
                            Account = resetModel?.txtAccount
                        });

                    result = "success";
                }
                else if (resetModel?.tp.ToUpper() == "A")
                {
                    await _adminService.UpdatePwd(
                        new ModifyPwdParameterDto
                        {
                            Password = resetModel?.txtPassword,
                            ConfirmPwd = resetModel?.txtConfirmPwd,
                            Account = resetModel?.txtAccount
                        });

                    result = "success";
                }

            }
            catch (Exception err)
            {
                result = $"error:{err.Message}";
            }

            result += " " + await _adminService.DeleteChangePwdRequest(new ChangeRequestPassword
            {
                Token = resetModel?.token,
                Account = resetModel?.txtAccount
            });

            HttpContext.Session.Remove(CDictionary.SK_RESETPWD_EXPIRE);

            return Json(result);
        }

        /// <summary>
        /// 重設密碼後刪除DB的忘記密碼請求
        /// </summary>
        /// <param name="expireTime"></param>
        /// <param name="token"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        //private string deleteChangePwdRequest(DateTime expireTime, string token, string acc)
        //{
        //    string result = "";
        //    ChangeRequestPassword request = _context.ChangeRequestPasswords.FirstOrDefault(r => r.Token == token);
        //    if (request != null)
        //    {
        //        var deleteItem = _context.ChangeRequestPasswords.Where(r => r.Account == acc);
        //        _context.ChangeRequestPasswords.RemoveRange(deleteItem.ToList());
        //        try
        //        {
        //            _context.SaveChanges();
        //            result += "success";
        //        }
        //        catch (Exception err)
        //        {
        //            result += $"error:{err.Message}";
        //        }

        //    }

        //    return result;
        //}

        /// <summary>
        /// 客服信箱列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AServiceMailList()
        {
            var mailList = new List<CgetServiceMailViewModel>();

            var list = await _serviceMailService.GetAll();

            if(list.IsAny())
            {
                foreach(var serviceMail in list)
                {
                    mailList.Add(new CgetServiceMailViewModel
                    {
                        serviceMail = serviceMail,
                    });
                }       
            }
           
            return View(mailList.AsEnumerable());
        }

        /// <summary>
        /// 取得尚未回覆的客服信件
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetUnrepliedServiceMail()
        {

            var mailList = new List<CgetServiceMailViewModel>();
            
            try
            {
                var list = await _serviceMailService.GetUnreplied();

                if (list.IsAny())
                {
                    foreach (var serviceMail in list)
                    {
                        mailList.Add(new CgetServiceMailViewModel
                        {
                            serviceMail = serviceMail,
                        });
                    }
                }
            }
            catch (Exception err)
            {
                return Json($"error:{err.Message}");
            }            
            
            return Json(mailList);
        }

        public async Task<IActionResult> GetServiceMailContent(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return NoContent();
            }

            int id = Convert.ToInt32(data);

            try
            {
                var model = new CgetServiceMailViewModel();
                var sm = await _serviceMailService.GetContentById(id);
                                                   
                if (sm.Fid > 0)
                {
                    model.serviceMail = sm;
                }

                return Json(model);
            }
            catch (Exception err)
            {
                return Json($"error:{err.Message}");
            }
            
        }

        /// <summary>
        /// 送出客服信回覆
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SubmitServiceMailReply(CgetServiceMailViewModel mail)
        {
            if (mail == null)
            {
                return NoContent();
            }
            
            try
            {
                await _serviceMailService.SubmitReply(new ServiceMailBox
                {
                    Fid = mail.Fid,
                    Reply = mail.Reply,
                    ReadTime = mail.ReadTime
                });
                        
                return Json("success");
            }
            catch (Exception err)
            {
                return Json($"error:{err.Message}");
            }                            
        }
    }
}
