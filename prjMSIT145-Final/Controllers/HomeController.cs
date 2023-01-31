using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;

namespace prjMSIT145_Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ispanMsit145shibaContext _context;
        public HomeController(ILogger<HomeController> logger, ispanMsit145shibaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult CIndex()
        {
            List<VBusinessMemberContainImgViewModel> BusinessMemberList = new List<VBusinessMemberContainImgViewModel>();
            var BusinessMemberdatas = _context.BusinessMembers.Join(_context.BusinessImgs, BM => BM.Fid, BI => BI.BFid, (BM, BI) => new
            {
                FID = BM.Fid,
                MemberName = BM.MemberName,
                OpenTime = BM.OpenTime,
                CloseTime = BM.CloseTime,
                Address = BM.Address,
                MemberAccount=BM.MemberAccount,
                IsSuspensed=BM.IsSuspensed,
                Gps=BM.Gps,
                IsOpened=BM.IsOpened,
                LogoImgFileName = BI.LogoImgFileName,
                SighImgFileName = BI.SighImgFileName,
            }).OrderBy(BM => BM.FID);
            foreach (var item in BusinessMemberdatas)
            {
                BusinessMemberList.Add(new VBusinessMemberContainImgViewModel
                {
                    Fid = item.FID,
                    MemberName = item.MemberName,
                    OpenTime = Convert.ToString(item.OpenTime).Substring(0,5),
                    CloseTime = Convert.ToString(item.CloseTime).Substring(0, 5),
                    Address = item.Address,
                    MemberAccount = item.MemberAccount,
                    IsSuspensed = item.IsSuspensed,
                    Gps = item.Gps,
                    IsOpened = item.IsOpened,
                    LogoImgFileName = item.LogoImgFileName,
                    SighImgFileName = item.SighImgFileName,
                });
            }
            CUtility.BusinessMemberList = BusinessMemberList;
            return View(CUtility.BusinessMemberList);
        }

        public IActionResult CShowProduct(int? BFid,int? OrderFid)
        {
            if (BFid == null)
                return RedirectToAction("CIndex");
            #region 取得該店家資訊至集合
            List<VBusinessMemberDetailViewModel> BusinessMemberDetailList = new List<VBusinessMemberDetailViewModel>();
            var BusinessMemberdatas = _context.BusinessMembers.Join(_context.BusinessImgs, BM => BM.Fid, BI => BI.BFid, (BM, BI) => new
            {
                FID = BM.Fid,
                MemberName = BM.MemberName,
                Phone=BM.Phone,
                OpenTime = BM.OpenTime,
                CloseTime = BM.CloseTime,
                Address = BM.Address,
                MemberAccount = BM.MemberAccount,
                IsSuspensed = BM.IsSuspensed,
                Gps = BM.Gps,
                IsOpened = BM.IsOpened,
                LogoImgFileName = BI.LogoImgFileName,
                BannerImgFileName1 = BI.BannerImgFileName1,
            }).Where(BM=>BM.FID ==BFid).OrderBy(BM => BM.FID);
            foreach (var item in BusinessMemberdatas)
            {
                BusinessMemberDetailList.Add(new VBusinessMemberDetailViewModel
                {
                    Fid = item.FID,
                    MemberName = item.MemberName,
                    Phone = item.Phone,
                    OpenTime = Convert.ToString(item.OpenTime).Substring(0, 5),
                    CloseTime = Convert.ToString(item.CloseTime).Substring(0, 5),
                    Address = item.Address,
                    MemberAccount = item.MemberAccount,
                    IsSuspensed = item.IsSuspensed,
                    Gps = item.Gps,
                    IsOpened = item.IsOpened,
                    LogoImgFileName = item.LogoImgFileName,
                    BannerImgFileName1 = item.BannerImgFileName1,
                });
            }
            CUtility.BusinessMemberDetailList = BusinessMemberDetailList;
            #endregion
            #region 取得該店家付款方式資訊至集合
            List<VPaymenttermViewModel> PaymenttermList = new List<VPaymenttermViewModel>();
            var Paymenttermdatas = from P in _context.PaymentTerm2BusiMembers
                           join B in _context.BusinessMembers on P.BFid equals B.Fid
                           join A in _context.PaymentTermCategories on P.PayTermCatId equals A.Fid
                           where P.BFid == BFid
                           orderby P.Fid
                           select new
                           {
                               BFid=P.BFid,
                               PaymentType=A.PaymentType,
                           };
            int ListCount = Paymenttermdatas.ToList().Count;
            int IDcheck = 0;
            int IDconfirm = 0;
            int temp = 0;
            string PayWay = "";
            for (int i = 0; i < ListCount + 1; i++)
            {
                if (i == ListCount)
                {
                    if (i != IDcheck)
                    {
                        IDcheck = i;
                        if (IDconfirm != temp)
                        {
                            PaymenttermList.Add(new VPaymenttermViewModel
                            {
                                BFid = IDconfirm,
                                PaymentType = PayWay
                            });
                            temp = IDconfirm;
                            PayWay = "";
                        }
                        IDconfirm = IDcheck;
                        PayWay = Paymenttermdatas.ToList()[i - 1].PaymentType;
                    }
                    else
                    {
                        PayWay += "、" + Paymenttermdatas.ToList()[i - 1].PaymentType;
                    }
                }
                else
                {
                    if (Paymenttermdatas.ToList()[i].BFid != IDcheck)
                    {
                        IDcheck = (int)Paymenttermdatas.ToList()[i].BFid;
                        if (IDconfirm != temp)
                        {
                            PaymenttermList.Add(new VPaymenttermViewModel
                            {
                                BFid = IDconfirm,
                                PaymentType = PayWay,
                            });
                            temp = IDconfirm;
                            PayWay = "";
                        }
                        IDconfirm = IDcheck;
                        PayWay = Paymenttermdatas.ToList()[i].PaymentType;
                    }
                    else
                    {
                        PayWay += "、" + Paymenttermdatas.ToList()[i].PaymentType;
                    }
                }
            }
            CUtility.PaymenttermList = PaymenttermList;
            #endregion
            #region 取得店家商品類別資訊至集合
            List<VProductClassViewModel> ProductClassList = new List<VProductClassViewModel>();
            int BFID = (int)BFid;
            var ProductClassdatas = from P in _context.ProductCategories
                                where P.BFid == BFID
                                orderby P.Fid
                                select new
                                {
                                    FID = P.Fid,
                                    CategoryName = P.CategoryName
                                };
            List<TProductClassTemp> ProductClassTempList = new List<TProductClassTemp>();
            foreach (var item in ProductClassdatas)
            {
                ProductClassTempList.Add(new TProductClassTemp
                {
                    Fid=item.FID,
                    CategoryName=item.CategoryName,
                });
            }
            foreach (var item1 in ProductClassTempList)
            {
                #region 取得店家內，各商品類別內之產品資訊至集合
                List<VProducts> ProductsList = new List<VProducts>();
                for (int p = 0; p < ProductClassTempList.Count; p++)
                {
                    var Productdatas = from P in _context.Products
                                       join B in _context.BusinessMembers on P.BFid equals B.Fid
                                       join C in _context.ProductCategories on P.CategoryFid equals C.Fid
                                       where (B.Fid == BFID & C.Fid == item1.Fid)
                                       orderby B.Fid
                                       select new
                                       {
                                           Fid = P.Fid,
                                           ProductName = P.ProductName,
                                           UnitPrice = P.UnitPrice,
                                           IsForSale = P.IsForSale,
                                           Photo = P.Photo,
                                       };
                    ProductsList.Clear();
                    foreach (var item2 in Productdatas)
                    {
                        ProductsList.Add(new VProducts
                        {
                            Fid = item2.Fid,
                            ProductName = item2.ProductName,
                            UnitPrice = Convert.ToInt32(item2.UnitPrice),
                            IsForSale = item2.IsForSale,
                            Photo = item2.Photo,
                            OrdetAmount = 0,
                        });
                    }
                }
                #endregion
                ProductClassList.Add(new VProductClassViewModel
                {
                    Fid = item1.Fid,
                    CategoryName = item1.CategoryName,
                    ProductsList = ProductsList
                });
            }
            CUtility.ProductClassList = ProductClassList;
            #endregion
            #region 判斷是否為加點
            OrderFid = CUtility.OrdersID;
            if (OrderFid != 0)
            {
                var OrderItemTable = from D in _context.ViewOrderDetails
                                     where D.OrderFid == OrderFid
                                     select new
                                     {
                                         ItemFid = D.ItemFid,
                                         ProductFid = D.ProductFid,
                                         Qty = D.Qty
                                     };
                var ReCount = from O in OrderItemTable.Distinct()
                              group O by O.ProductFid into G
                              select new
                              {
                                  ProductID = G.Key,
                                  TotalAmount = G.Sum(O => O.Qty)
                              };
                for (int i = 0; i < CUtility.ProductClassList.Count; i++)
                {
                    int num1 = i;
                    for (int j = 0; j < CUtility.ProductClassList[num1].ProductsList.Count; j++)
                    {
                        int num2 = j;
                        foreach (var item in ReCount)
                        {
                            if (item.ProductID == CUtility.ProductClassList[num1].ProductsList[num2].Fid)
                            {
                                CUtility.ProductClassList[num1].ProductsList[num2].OrdetAmount = Convert.ToInt32(item.TotalAmount);
                            }
                        }
                    }
                }
            }
            #endregion
            List<VCUtilityViewModel> CUL= new List<VCUtilityViewModel>();
            CUL.Add(new VCUtilityViewModel
            {
                BusinessMemberDetailList = CUtility.BusinessMemberDetailList,
                PaymenttermList = CUtility.PaymenttermList,
                ProductClassList = CUtility.ProductClassList,
            });
            return View(CUL);
        }


        //-------------------------------------------------------------------------------------------------
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}