using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Net;
using prjMSIT145_Final.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace prjMSIT145_Final.Controllers
{
    public class OrderDetialController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public OrderDetialController(ispanMsit145shibaContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            int NFid = 0;
            if (!(HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)))
            {
                return Redirect("/CustomerMember/Login");
            }
            NormalMember Memberdatas = JsonSerializer.Deserialize<NormalMember>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            NFid = Memberdatas.Fid;

            List<COrderDetialViewModel> OrderDetiallist = new List<COrderDetialViewModel>();
            var OrderDatas = from O in _context.Orders
                             join B in _context.BusinessMembers
                             on O.BFid equals B.Fid
                             join BI in _context.BusinessImgs
                             on O.BFid equals BI.BFid
                             where O.NFid == NFid
                             select new
                             {
                                 O.Fid,
                                 B.MemberName,
                                 O.OrderState,
                                 O.OrderTime,
                                 O.TotalAmount,
                                 BI.LogoImgFileName
                             };

            foreach (var c in OrderDatas)
            {
                COrderDetialViewModel vm = new COrderDetialViewModel();
                vm.Fid = c.Fid;
                vm.BMemberName = c.MemberName;
                switch (c.OrderState)
                {
                    case "1":
                        vm.OrderState = "未接單";
                        break;
                    case "2":
                        vm.OrderState = "商家準備中";
                        break;
                    case "3":
                        vm.OrderState = "商品準備完成";
                        break;
                    case "4":
                        vm.OrderState = "訂單已完成";
                        break;
                    case "5":
                        vm.OrderState = "商家退單";
                        break;
                    default:
                        vm.OrderState = "揪團失敗";
                        break;
                }
                vm.OrderTime = c.OrderTime;
                vm.TotalAmount = Convert.ToInt32(c.TotalAmount);
                vm.LogoImgFileName = c.LogoImgFileName;

                OrderDetiallist.Add(vm);
            }
            return View(OrderDetiallist);
        }

        public IActionResult ListInfo(int? Fid)
        {
            #region
            var q = from o in _context.Orders
                    join b in _context.BusinessMembers
                    on o.BFid equals b.Fid
                    join a in _context.PaymentTermCategories
                    on o.PayTermCatId equals a.Fid
                    join bi in _context.BusinessImgs
                    on o.BFid equals bi.BFid
                    where o.Fid == Fid
                    select new
                    {
                        Fid = o.Fid,
                        NFid = o.NFid,
                        BFid = o.BFid,
                        BMemberName = b.MemberName,
                        BMemberPhone = b.Phone,
                        BAddress = b.Address,
                        PickUpDate = o.PickUpDate,
                        PickUpTime = o.PickUpTime,
                        PickUpType = o.PickUpType,
                        PickUpPerson = o.PickUpPerson,
                        PickUpPersonPhone = o.PickUpPersonPhone,
                        PayTernCatId = a.PaymentType,
                        OrderState = o.OrderState,
                        Memo = o.Memo,
                        OrderTime = o.OrderTime,
                        TotalAmount = o.TotalAmount,
                        LogoImgFileName = bi.LogoImgFileName,
                        OrderISerialId = o.OrderISerialId

                    };
            var Pr = from o in _context.OrderItems
                     join p in _context.Products
                     on o.ProductFid equals p.Fid
                     where o.OrderFid == Fid
                     select new
                     {
                         Fid = o.Fid,
                         ProductName = p.ProductName,
                         ProductQty = o.Qty,
                         Productprice = p.UnitPrice,
                         OrderFid = o.OrderFid
                     };

            var ItemName = from i in _context.OrderOptionsDetails
                           join p in _context.ProductOptions
                           on i.OptionFid equals p.Fid
                           select new
                           {
                               Fid = i.Fid,
                               ItemFid = i.ItemFid,
                               OptionName = p.OptionName,
                               ItemPrice = p.UnitPrice
                           };
            #endregion

            COrderDetialViewModel vm = new COrderDetialViewModel();

            if (q != null)
            {

                foreach (var c in q.ToList())
                {
                    switch (c.OrderState)
                    {
                        case "1":
                            vm.OrderState = "未接單";
                            break;
                        case "2":
                            vm.OrderState = "商家準備中";
                            break;
                        case "3":
                            vm.OrderState = "商品準備完成";
                            break;

                        case "4":
                            vm.OrderState = "訂單已完成";
                            break;
                        case "5":
                            vm.OrderState = "商家退單";
                            break;

                        default:
                            vm.OrderState = "揪團失敗";
                            break;
                    }

                    vm.OrderTime = c.OrderTime;
                    vm.TotalAmount = Convert.ToInt32(c.TotalAmount);
                    vm.PickUpPerson = c.PickUpPerson;
                    vm.Address = c.BAddress;
                    vm.BMemberName = c.BMemberName;
                    vm.BMemberPhone = c.BMemberPhone;
                    vm.Fid = c.Fid;
                    vm.BFid = c.BFid;
                    vm.NFid = c.NFid;
                    vm.PickUpTime = c.PickUpTime;
                    vm.PickUpDate = c.PickUpDate;
                    vm.PickUpType = c.PickUpType;
                    vm.PickUpPersonPhone = c.PickUpPersonPhone;
                    vm.PayTermCatId = c.PayTernCatId;
                    vm.Memo = c.Memo;
                    vm.items = new List<COrderItemViewModel>();
                    vm.LogoImgFileName = c.LogoImgFileName;
                    vm.OrderISerialId = c.OrderISerialId;

                    var orderitem = from i in Pr
                                    where i.OrderFid == c.Fid
                                    select i;
                    foreach (var item in orderitem.ToList())
                    {
                        COrderItemViewModel item2 = new COrderItemViewModel();
                        item2.ProductName = item.ProductName;
                        item2.Productprice = item.Productprice;
                        item2.Qty = item.ProductQty;
                        item2.OptionName = new List<string>();
                        item2.OptionPrice = 0;
                        vm.TotalQty += item.ProductQty;

                        var itemOption = from o in ItemName
                                         where o.ItemFid == item.Fid
                                         select o;

                        foreach (var Option in itemOption)
                        {
                            item2.OptionName.Add(Option.OptionName);
                            item2.OptionPrice += Option.ItemPrice;
                        }
                        vm.items.Add(item2);
                    }


                }

            }


            return View(vm);
        }

        public IActionResult CartItemDelete(int? OrderFid, int? ItemFid)
        {

            #region 扣除總金額
            var OrderDeitalTotalAmount = from q in _context.ViewShowFullOrders
                                         where q.ItemFid == ItemFid
                                         select new
                                         {
                                             SubTotal = q.SubTotal,
                                             TotalAmount = q.TotalAmount,
                                         };
            COrderDetialViewModel vm = new COrderDetialViewModel();
            if (OrderDeitalTotalAmount != null)
            {
                foreach (var c in OrderDeitalTotalAmount.ToList())
                {
                    vm.TotalAmount = c.TotalAmount - c.SubTotal;
                    Order prod = _context.Orders.FirstOrDefault(t => t.Fid == OrderFid);
                    if (prod != null)
                    {
                        prod.TotalAmount = vm.TotalAmount;
                    }
                }
            }
            _context.SaveChanges();
            #endregion
            #region 依據商品ID找出所屬配料資料並刪除 (刪除OrderOptionDetial)
            var OrderOptionsDetaildatasDelete = from OOD in _context.OrderOptionsDetails
                                                where OOD.ItemFid == ItemFid
                                                select OOD;
            _context.OrderOptionsDetails.RemoveRange(OrderOptionsDetaildatasDelete);
            _context.SaveChanges();
            #endregion
            #region 依據商品ID找出對應資料並刪除 (刪除OrderItems)
            var OrderItemsdatasDelete = from OI in _context.OrderItems
                                        where OI.Fid == ItemFid
                                        select OI;
            _context.OrderItems.RemoveRange(OrderItemsdatasDelete);
            _context.SaveChanges();
            #endregion
            #region 再次取得資料
            var qa = from o in _context.Orders
                    join b in _context.BusinessMembers
                    on o.BFid equals b.Fid
                    join nm in _context.NormalMembers
                    on o.NFid equals nm.Fid
                    where o.Fid == OrderFid
                    select new
                    {
                        Fid = o.Fid,
                        NFid = o.NFid,
                        BFid = o.BFid,
                        BMemberName = b.MemberName,
                        BMemberPhone = b.Phone,
                        BAddress = b.Address,
                        PickUpPerson = o.PickUpPerson,
                        PickUpPersonPhone = o.PickUpPersonPhone,
                        OrderState = o.OrderState,
                        Memo = o.Memo,
                        OrderTime = o.OrderTime,
                        TotalAmount = o.TotalAmount,
                        MemberPhotoFile = nm.MemberPhotoFile,
                        OrderISerialId = o.OrderISerialId,
                    };
            var Pr = from o in _context.OrderItems
                     join p in _context.Products
                     on o.ProductFid equals p.Fid
                     where o.OrderFid == OrderFid
                     select new
                     {
                         Fid = o.Fid,
                         ProductName = p.ProductName,
                         ProductQty = o.Qty,
                         Productprice = p.UnitPrice,
                         OrderFid = o.OrderFid
                     };

            var ItemName = from i in _context.OrderOptionsDetails
                           join p in _context.ProductOptions
                           on i.OptionFid equals p.Fid
                           select new
                           {
                               Fid = i.Fid,
                               ItemFid = i.ItemFid,
                               OptionName = p.OptionName,
                               ItemPrice = p.UnitPrice
                           };
            List<COrderDetialViewModel> list = new List<COrderDetialViewModel>();
            if (qa != null)
            {
             COrderDetialViewModel vm2 = new COrderDetialViewModel();
                vm2.TotalQty = 0;
                foreach (var c in qa.ToList())
                {
                    vm2.OrderTime = c.OrderTime;
                    vm2.TotalAmount = Convert.ToInt32(c.TotalAmount);
                    vm2.PickUpPerson = c.PickUpPerson;
                    vm2.Address = c.BAddress;
                    vm2.BMemberName = c.BMemberName;
                    vm2.BMemberPhone = c.BMemberPhone;
                    vm2.Fid = c.Fid;
                    vm2.BFid = c.BFid;
                    vm2.NFid = c.NFid;
                    vm2.PickUpPersonPhone = c.PickUpPersonPhone;
                    vm2.Memo = c.Memo;
                    vm2.MemberPhotoFile = c.MemberPhotoFile;
                    vm2.OrderISerialId = c.OrderISerialId;

                    vm2.items = new List<COrderItemViewModel>();
                    var orderitem = from i in Pr
                                    where i.OrderFid == c.Fid
                                    select i;
                    foreach (var item in orderitem.ToList())
                    {
                        COrderItemViewModel item2 = new COrderItemViewModel();
                        item2.ProductName = item.ProductName;
                        item2.Productprice = item.Productprice;
                        item2.Qty = item.ProductQty;
                        item2.OptionName = new List<string>();
                        item2.OptionPrice = 0;
                        vm2.TotalQty += item.ProductQty;
                        item2.Fid = item.Fid;

                        var itemOption = from o in ItemName
                                         where o.ItemFid == item.Fid
                                         select o;

                        foreach (var Option in itemOption)
                        {
                            item2.OptionName.Add(Option.OptionName);
                            item2.OptionPrice += Option.ItemPrice;
                        }
                        vm2.items.Add(item2);
                    }
                }
                list.Add(vm2);
            }
            #endregion
            return Json(list);
        }

        [HttpPost]
        public IActionResult CartList(COrderDetialViewModel vm)
        {

            Order prod = _context.Orders.FirstOrDefault(t => t.Fid == vm.Fid);

            if (prod != null)
            {

                prod.PickUpType = vm.PickUpType;
                prod.PickUpDate = vm.PickUpDate;
                prod.PickUpTime = vm.PickUpDate - DateTime.Now;
                prod.OrderState = vm.OrderState;
                prod.PayTermCatId = Convert.ToInt32(vm.PayTermCatId);
                prod.Memo = vm.Memo;
                prod.TotalAmount = Convert.ToInt32(vm.TotalAmount);
                prod.PickUpPerson = vm.PickUpPerson;
                prod.PickUpPersonPhone = vm.PickUpPersonPhone;

                Coupon coup = _context.Coupons.FirstOrDefault(t => t.Fid == 7);
                if (coup != null)
                {

                  coup.IsUsed = vm.IsUsed;
                }
            }
            //_context.SaveChanges();
            return RedirectToAction("List");

        }

        public IActionResult CartList(int Fid)
        {
            #region   
            var q = from o in _context.Orders
                    join b in _context.BusinessMembers
                    on o.BFid equals b.Fid
                    join nm in _context.NormalMembers
                    on o.NFid equals nm.Fid
                    where o.Fid == Fid
                    select new
                    {
                        Fid = o.Fid,
                        NFid = o.NFid,
                        BFid = o.BFid,
                        BMemberName = b.MemberName,
                        BMemberPhone = b.Phone,
                        BAddress = b.Address,
                        PickUpPerson = o.PickUpPerson,
                        PickUpPersonPhone = o.PickUpPersonPhone,
                        OrderState = o.OrderState,
                        Memo = o.Memo,
                        OrderTime = o.OrderTime,
                        TotalAmount = o.TotalAmount,
                        MemberPhotoFile = nm.MemberPhotoFile,
                        OrderISerialId = o.OrderISerialId,
                    };
            var Pr = from o in _context.OrderItems
                     join p in _context.Products
                     on o.ProductFid equals p.Fid
                     where o.OrderFid == Fid
                     select new
                     {
                         Fid = o.Fid,
                         ProductName = p.ProductName,
                         ProductQty = o.Qty,
                         Productprice = p.UnitPrice,
                         OrderFid = o.OrderFid
                     };

            var ItemName = from i in _context.OrderOptionsDetails
                           join p in _context.ProductOptions
                           on i.OptionFid equals p.Fid
                           select new
                           {
                               Fid = i.Fid,
                               ItemFid = i.ItemFid,
                               OptionName = p.OptionName,
                               ItemPrice = p.UnitPrice
                           };
            var coupons = from c in _context.Coupons
                          join cn in _context.Coupon2NormalMembers 
                          on c.Fid equals cn.CouponId
                          select new
                          {
                              c.Price,
                              c.CouponCode,
                              c.Title,
                              c.Memo,
                              c.IsUsed,
                              c.Fid,
                          };
            #endregion
            COrderDetialViewModel vm = new COrderDetialViewModel();
            vm.TotalQty = 0;
            if (q != null)
            {
                foreach (var c in q.ToList())
                {
                    vm.OrderTime = c.OrderTime;
                    vm.TotalAmount = Convert.ToInt32(c.TotalAmount);
                    vm.PickUpPerson = c.PickUpPerson;
                    vm.Address = c.BAddress;
                    vm.BMemberName = c.BMemberName;
                    vm.BMemberPhone = c.BMemberPhone;
                    vm.Fid = c.Fid;
                    vm.BFid = c.BFid;
                    vm.NFid = c.NFid;
                    vm.PickUpPersonPhone = c.PickUpPersonPhone;
                    vm.Memo = c.Memo;
                    vm.MemberPhotoFile = c.MemberPhotoFile;
                    vm.OrderISerialId = c.OrderISerialId;

                    foreach (var covm in coupons.ToList())
                    {

                        vm.Price = Convert.ToInt32(covm.Price);
                        vm.CouponCode = covm.CouponCode;
                        vm.Memo2 = covm.Memo;
                        vm.Title = covm.Title;
                        vm.IsUsed = covm.IsUsed;
                        vm.CouponFid = covm.Fid;
                    }

                    vm.items = new List<COrderItemViewModel>();
                    var orderitem = from i in Pr
                                    where i.OrderFid == c.Fid
                                    select i;
                    foreach (var item in orderitem.ToList())
                    {
                        COrderItemViewModel item2 = new COrderItemViewModel();
                        item2.ProductName = item.ProductName;
                        item2.Productprice = item.Productprice;
                        item2.Qty = item.ProductQty;
                        item2.OptionName = new List<string>();
                        item2.OptionPrice = 0;
                        vm.TotalQty += item.ProductQty;
                        item2.Fid = item.Fid;

                        var itemOption = from o in ItemName
                                         where o.ItemFid == item.Fid
                                         select o;

                        foreach (var Option in itemOption)
                        {
                            item2.OptionName.Add(Option.OptionName);
                            item2.OptionPrice += Option.ItemPrice;
                        }
                        vm.items.Add(item2);
                    } 
                }
            }

			ViewData["MerchantOrderNo"] = DateTime.Now.ToString("yyyyMMddHHmmss");  //訂單編號
			ViewData["ExpireDate"] = DateTime.Now.AddDays(3).ToString("yyyyMMdd"); //繳費有效期限       
            return View(vm);
        }
    }
}
