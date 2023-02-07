using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Signing;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using static NuGet.Packaging.PackagingConstants;

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
                IsSuspensed = BM.IsSuspensed,
                Gps = BM.Gps,
                IsOpened = BM.IsOpened,
                LogoImgFileName = BI.LogoImgFileName,
                SighImgFileName = BI.SighImgFileName,
            }).OrderBy(BM => BM.FID);
            foreach (var item in BusinessMemberdatas)
            {
                BusinessMemberList.Add(new VBusinessMemberContainImgViewModel
                {
                    Fid = item.FID,
                    MemberName = item.MemberName,
                    OpenTime = Convert.ToString(item.OpenTime).Substring(0, 5),
                    CloseTime = Convert.ToString(item.CloseTime).Substring(0, 5),
                    Address = item.Address,
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

        public IActionResult CShowProduct(int? BFid, int? OrderFid)
        {
            if (BFid == null)
                return RedirectToAction("CIndex");
            #region 取得該店家資訊至集合
            List<VBusinessMemberDetailViewModel> BusinessMemberDetailList = new List<VBusinessMemberDetailViewModel>();
            var BusinessMemberdatas = _context.BusinessMembers.Join(_context.BusinessImgs, BM => BM.Fid, BI => BI.BFid, (BM, BI) => new
            {
                FID = BM.Fid,
                MemberName = BM.MemberName,
                Phone = BM.Phone,
                OpenTime = BM.OpenTime,
                CloseTime = BM.CloseTime,
                Address = BM.Address,
                MemberAccount = BM.MemberAccount,
                IsSuspensed = BM.IsSuspensed,
                Gps = BM.Gps,
                IsOpened = BM.IsOpened,
                LogoImgFileName = BI.LogoImgFileName,
                BannerImgFileName1 = BI.BannerImgFileName1,
            }).Where(BM => BM.FID == BFid).OrderBy(BM => BM.FID);
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
            if (BusinessMemberDetailList.Count == 0)
                return RedirectToAction("CIndex");
            if (BusinessMemberDetailList[0].IsSuspensed != 0 || BusinessMemberDetailList[0].IsOpened != 1)
                return RedirectToAction("CIndex");
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
                                       BFid = P.BFid,
                                       PaymentType = A.PaymentType,
                                   };
            string PayWay = "";
            int ListCount = Paymenttermdatas.ToList().Count;
            if (ListCount != 0)
            {
                int IDcheck = 0;
                int IDconfirm = 0;
                int temp = 0;
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
            }
            else
            {
                PaymenttermList.Add(new VPaymenttermViewModel
                {
                    BFid = BFid,
                    PaymentType = "店家未提供付款方式",
                });
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
                    Fid = item.FID,
                    CategoryName = item.CategoryName,
                });
            }
            foreach (var item1 in ProductClassTempList)
            {
                #region 取得店家內，各商品類別內之產品資訊至集合
                List<VProductsViewModel> ProductsList = new List<VProductsViewModel>();
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
                        if (item2.IsForSale != 0)
                        {
                            ProductsList.Add(new VProductsViewModel
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
            #region 取得訂單編號
            if (OrderFid == null)
                CUtility.OrderID = 0;
            else
                CUtility.OrderID = (int)OrderFid;
            #endregion
            #region 判斷是否為加點，並計算各產品已點數量
            if (OrderFid > 0)
            {
                #region 判斷訂單編號是否有效，及是否已完成
                var Orderdatas = from O in _context.Orders
                                 where O.Fid == OrderFid
                                 select new
                                 {
                                     OrderState = O.OrderState,
                                 };
                int OrderListCount = Orderdatas.ToList().Count;
                if (OrderListCount == 0)
                    return RedirectToAction("CIndex");
                string isFinished = Orderdatas.ToList()[0].OrderState;
                if (isFinished != "0")
                    return RedirectToAction("CIndex");
                #endregion
                var OrderItemdatas = from OI in _context.OrderItems
                                     where OI.OrderFid == OrderFid
                                     orderby OI.Fid
                                     select new
                                     {
                                         Fid = OI.Fid,
                                         ProductFid = OI.ProductFid,
                                         Qty = OI.Qty
                                     };
                var ReCount = from O in OrderItemdatas.Distinct()
                              group O by O.ProductFid into G
                              select new
                              {
                                  ProductID = G.Key,
                                  TotalAmount = G.Sum(O => O.Qty)
                              };
                for (int i = 0; i < CUtility.ProductClassList.Count; i++)
                {
                    for (int j = 0; j < CUtility.ProductClassList[i].ProductsList.Count; j++)
                    {
                        foreach (var item in ReCount)
                        {
                            if (item.ProductID == CUtility.ProductClassList[i].ProductsList[j].Fid)
                            {
                                CUtility.ProductClassList[i].ProductsList[j].OrdetAmount = Convert.ToInt32(item.TotalAmount);
                            }
                        }
                    }
                }
            }
            #endregion
            #region 判斷商品類別生成區域辨別碼
            int regionlength_1 = 0;
            int regionlength_2 = 0;
            int regionlength_3 = 0;
            foreach (var item in CUtility.ProductClassList)
            {
                if (regionlength_1 <= regionlength_2 && regionlength_1 <= regionlength_3)
                {
                    int tempLength = 0;
                    if (item.ProductsList.Count != 0)
                        tempLength += 52;
                    tempLength += (item.ProductsList.Count * 60);
                    regionlength_1 += tempLength;
                    item.PlayregionID = 1;
                }
                else if (regionlength_2 < regionlength_1 && regionlength_2 <= regionlength_3)
                {
                    int tempLength = 0;
                    if (item.ProductsList.Count != 0)
                        tempLength += 52;
                    tempLength += (item.ProductsList.Count * 60);
                    regionlength_2 += tempLength;
                    item.PlayregionID = 2;
                }
                else
                {
                    int tempLength = 0;
                    if (item.ProductsList.Count != 0)
                        tempLength += 52;
                    tempLength += (item.ProductsList.Count * 60);
                    regionlength_3 += tempLength;
                    item.PlayregionID = 3;
                }
            }
            #endregion
            List<VCUtilityViewModel> CUL = new List<VCUtilityViewModel>();
            CUL.Add(new VCUtilityViewModel
            {
                BusinessMemberDetailList = CUtility.BusinessMemberDetailList,
                PaymenttermList = CUtility.PaymenttermList,
                ProductClassList = CUtility.ProductClassList,
                OrderID = CUtility.OrderID,
            });
            return View(CUL);
        }

        public IActionResult CShowProductOption(int? PFid)
        {
            #region 取得點選產品的基礎資訊
            List<VProductBasicInfoViewModel> ProductBasicInfoList = new List<VProductBasicInfoViewModel>();
            var ProductInfodatas = from P in _context.Products
                                   where P.Fid == PFid
                                   select new
                                   {
                                       Fid = P.Fid,
                                       ProductName = P.ProductName,
                                       UnitPrice = P.UnitPrice,
                                       Memo = P.Memo,
                                       Photo = P.Photo,
                                   };
            foreach (var item in ProductInfodatas)
            {
                ProductBasicInfoList.Add(new VProductBasicInfoViewModel
                {
                    Fid = item.Fid,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Memo = item.Memo,
                    Photo = item.Photo,
                });
            }
            CUtility.ProductBasicInfoList = ProductBasicInfoList;
            #endregion
            #region 取得商品配料類別資訊至集合
            List<VOptionGroupViewModel> OptionGroupList = new List<VOptionGroupViewModel>();
            var OptionGroupDatas = from P in _context.Products
                                   join OTP in _context.OptionsToProducts on P.Fid equals OTP.ProductFid
                                   join OG in _context.ProductOptionGroups on OTP.OptionGroupFid equals OG.Fid
                                   where P.Fid == PFid
                                   orderby P.Fid,OG.IsMultiple
                                   select new
                                   {
                                       Fid = OG.Fid,
                                       OptionGroupName = OG.OptionGroupName,
                                       IsMultiple = OG.IsMultiple,
                                   };
            List<TOptionGroupTemp> OptionGroupTempList = new List<TOptionGroupTemp>();
            foreach (var item in OptionGroupDatas)
            {
                OptionGroupTempList.Add(new TOptionGroupTemp
                {
                    Fid = item.Fid,
                    OptionGroupName = item.OptionGroupName,
                    IsMultiple = item.IsMultiple,
                });
            }
            foreach (var item1 in OptionGroupTempList)
            {
                #region 取得商品內，各配料類別內之配料資訊至集合
                List<VOptionViewModel> OptionList = new List<VOptionViewModel>();
                for (int p = 0; p < OptionGroupTempList.Count; p++)
                {
                    var OptionTable = from O in _context.ProductOptions
                                      where O.OptionGroupFid == item1.Fid
                                      orderby O.Fid
                                      select new
                                      {
                                          Fid = O.Fid,
                                          OptionName = O.OptionName,
                                          UnitPrice = O.UnitPrice,
                                      };
                    OptionList.Clear();
                    foreach (var item2 in OptionTable)
                    {
                        OptionList.Add(new VOptionViewModel
                        {
                            Fid = item2.Fid,
                            OptionName = item2.OptionName,
                            UnitPrice = Convert.ToInt32(item2.UnitPrice),
                        });
                    }
                }
                #endregion
                OptionGroupList.Add(new VOptionGroupViewModel
                {
                    Fid = item1.Fid,
                    OptionGroupName = item1.OptionGroupName,
                    IsMultiple = item1.IsMultiple,
                    OptionList = OptionList
                });
            }
            CUtility.OptionGroupList = OptionGroupList;
            #endregion
            List<VCUtilityViewModel> CUL = new List<VCUtilityViewModel>();
            CUL.Add(new VCUtilityViewModel
            {
                ProductBasicInfoList = CUtility.ProductBasicInfoList,
                OptionGroupList = CUtility.OptionGroupList,
            });
            return Json(CUL);
        }

        [HttpPost]
        public IActionResult CAddtoCart(IFormCollection NewOrder, int? NFid)
        {
            NFid = 1;
            int SNIDCount = 0;
            if (NFid != 0)  //是否有登入會員
            {
                if (NewOrder[NewOrder.Keys.ToList()[0]] == "0") //是否有訂單編號 (即是否為新訂單)
                {
                    #region 將新訂單寫入Orders
                    var OrderSNID = from O in _context.Orders
                                    select O.OrderISerialId;
                    foreach (var SNID in OrderSNID)
                    {
                        if (SNIDCount < Convert.ToInt32(SNID))
                            SNIDCount = Convert.ToInt32(SNID);
                    }
                    if (SNIDCount.ToString().Substring(4, 2) != DateTime.Now.ToString("MM"))
                        SNIDCount = Convert.ToInt32($"{Convert.ToString(DateTime.Now).Substring(0, 4)}{DateTime.Now.ToString("MM")}0000");
                    var NMInfo = from N in _context.NormalMembers
                                 where N.Fid == NFid
                                 select N;
                    string PickUpPerson = NMInfo.ToList()[0].MemberName;
                    string PickUpPersonPhone = NMInfo.ToList()[0].Phone;
                    _context.Orders.Add(new Order
                    {
                        NFid = NFid,
                        BFid = Convert.ToInt32(NewOrder[NewOrder.Keys.ToList()[1]]),
                        PickUpDate = null,
                        PickUpTime = null,
                        PickUpType = null,
                        PickUpPerson = PickUpPerson,
                        PickUpPersonPhone = PickUpPersonPhone,
                        PayTermCatId = null,
                        TaxIdnum = null,
                        OrderState = "0",
                        Memo = "",
                        OrderTime = DateTime.Now,
                        TotalAmount = Convert.ToDecimal(NewOrder[NewOrder.Keys.ToList()[3]]),
                        OrderISerialId = $"{Convert.ToString(DateTime.Now).Substring(0, 4)}{DateTime.Now.ToString("MM")}{Convert.ToString(SNIDCount+1).Substring(6, 4)}",
                    });
                    _context.SaveChanges();

                    var OrderID = from O in _context.Orders
                                  select O.Fid;
                    CUtility.OrderID = OrderID.Max();
                    #endregion
                    #region 將新訂單產品內容寫入Orderitems
                    _context.OrderItems.Add(new OrderItem
                    {
                        OrderFid = CUtility.OrderID,
                        ProductFid = Convert.ToInt32(NewOrder[NewOrder.Keys.ToList()[2]]),
                        Qty = Convert.ToInt32(NewOrder[NewOrder.Keys.ToList()[4]]),
                    });
                    _context.SaveChanges();

                    var OrderitemsID = from O in _context.OrderItems
                                       select O.Fid;
                    CUtility.OrderitemID = OrderitemsID.Max();
                    #endregion
                    #region 將新訂單產品配料內容寫入OrderOptionsDetail
                    for (int i = 5; i < NewOrder.Keys.ToList().Count-1; i++)
                    {
                        if (NewOrder[NewOrder.Keys.ToList()[i]] != "0")
                        {
                            _context.OrderOptionsDetails.Add(new OrderOptionsDetail
                            {
                                ItemFid = CUtility.OrderitemID,
                                OptionFid = Convert.ToInt32(NewOrder[NewOrder.Keys.ToList()[i]]),
                            });
                            _context.SaveChanges();
                        }
                    }
                    #endregion
                }
                else //既有訂單再新增
                {
                    #region 將訂單更新資訊寫入Orders
                    var UpdateOrder = from O in _context.Orders
                                      where O.Fid == CUtility.OrderID
                                      select O;
                    foreach (var item in UpdateOrder)
                    {
                        item.OrderTime = DateTime.Now;
                        item.TotalAmount = item.TotalAmount + Convert.ToDecimal(NewOrder[NewOrder.Keys.ToList()[3]]);
                    }
                    _context.SaveChanges();
                    #endregion
                    #region 將既有訂單新增產品內容寫入Orderitems
                    _context.OrderItems.Add(new OrderItem
                    {
                        OrderFid = CUtility.OrderID,
                        ProductFid = Convert.ToInt32(NewOrder[NewOrder.Keys.ToList()[2]]),
                        Qty = Convert.ToInt32(NewOrder[NewOrder.Keys.ToList()[4]]),
                    });
                    _context.SaveChanges();

                    var OrderitemsID = from O in _context.OrderItems
                                       select O.Fid;
                    CUtility.OrderitemID = OrderitemsID.Max();
                    #endregion
                    #region 將既有訂單新增產品配料內容寫入OrderOptionsDetail
                    for (int i = 5; i < NewOrder.Keys.ToList().Count-1; i++)
                    {
                        if (NewOrder[NewOrder.Keys.ToList()[i]] != "0")
                        {
                            _context.OrderOptionsDetails.Add(new OrderOptionsDetail
                            {
                                ItemFid = CUtility.OrderitemID,
                                OptionFid = Convert.ToInt32(NewOrder[NewOrder.Keys.ToList()[i]]),
                            });
                            _context.SaveChanges();
                        }
                    }
                    #endregion
                }
            }
            else //未登入則轉向登入頁面
                return Redirect("CustomerMember/Login");
            return RedirectToAction("CShowProduct", new { BFid = Convert.ToInt32(NewOrder[NewOrder.Keys.ToList()[1]]), OrderFid =CUtility.OrderID});
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