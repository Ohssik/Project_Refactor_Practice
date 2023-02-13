using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Net;
using prjMSIT145_Final.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace prjMSIT145_Final.Controllers
{
    public class OrderDetialController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public OrderDetialController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult List(int? id)
        {

            if (id == null)
            {
                return View();
            }
            return RedirectToAction("ListInfo");



        }
        public IActionResult List()
        {
            int NFid = 0;
            if (HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER) == null)
            {
                return Redirect("/CustomerMember/Login");
            }
            NormalMember Memberdatas = JsonSerializer.Deserialize<NormalMember>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            NFid = Memberdatas.Fid;

            List<COrderDetialViewModel> OrderDetiallist = new List<COrderDetialViewModel>();
            var OrderDatas = from O in _context.Orders
                             join B in _context.BusinessMembers
                             on O.BFid equals B.Fid
                             where O.NFid == NFid
                             select new
                             {
                                 O.Fid,
                                 B.MemberName,
                                 O.OrderState,
                                 O.OrderTime,
                                 O.TotalAmount,
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
                        vm.OrderState = "已接單";
                        break;
                    case "3":
                        vm.OrderState = "商家準備中";
                        break;
                    case "4":
                        vm.OrderState = "已完成";
                        break;
                    case "5":
                        vm.OrderState = "商家退單";
                        break;
                    default:
                        vm.OrderState = "揪團失敗";
                        break;
                }
                vm.OrderTime = c.OrderTime;
                vm.TotalAmount = c.TotalAmount;

                OrderDetiallist.Add(vm);
            }
            return View(OrderDetiallist);
        }
        [HttpPost]
        public IActionResult ListInfo()
        {

            return View();
        }
        public IActionResult ListInfo(int? Fid)
        {
            #region
            var q = from o in _context.Orders
                    join b in _context.BusinessMembers
                    on o.BFid equals b.Fid
                    join a in _context.PaymentTermCategories
                    on o.PayTermCatId equals a.Fid
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
                        TotalAmount = o.TotalAmount

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
                            vm.OrderState = "已接單";
                            break;
                        case "3":
                            vm.OrderState = "商家準備中";
                            break;

                        case "4":
                            vm.OrderState = "已完成";
                            break;
                        case "5":
                            vm.OrderState = "商家退單";
                            break;

                        default:
                            vm.OrderState = "揪團失敗";
                            break;
                    }

                    vm.OrderTime = c.OrderTime;
                    vm.TotalAmount = c.TotalAmount;
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

        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                OrderItem prod = _context.OrderItems.FirstOrDefault(p => p.Fid == id);
                

                if (prod != null)
                {
                    _context.OrderItems.Remove(prod);
                    _context.SaveChanges();
                }

                
            }
            return RedirectToAction("CartList");
        }

        [HttpPost]
        public IActionResult CartList(COrderDetialViewModel vm)
        {
            
            Order prod = _context.Orders.FirstOrDefault(t => t.Fid == vm.Fid);

            if(prod != null)
            {
                
                prod.PickUpType= vm.PickUpType;
                prod.PickUpDate = vm.PickUpDate;
                prod.PickUpTime = vm.PickUpDate - DateTime.Now;
                prod.OrderState = vm.OrderState;
                prod.PayTermCatId = Int32.Parse(vm.PayTermCatId);
                prod.Memo = vm.Memo;
                prod.TotalAmount= vm.TotalAmount;
                prod.PickUpPerson = vm.PickUpPerson;
                prod.PickUpPersonPhone = vm.PickUpPersonPhone;
            }


           
            _context.SaveChanges();

            return RedirectToAction("List");
        }


        public IActionResult CartList(int Fid)
        {
            #region   
            var q = from o in _context.Orders
                    join b in _context.BusinessMembers
                    on o.BFid equals b.Fid
                    //join a in _context.PaymentTermCategories
                    //on o.PayTermCatId equals a.Fid
                    where o.Fid == Fid
                    select new
                    {
                        Fid = o.Fid,
                        NFid = o.NFid,
                        BFid = o.BFid,
                        BMemberName = b.MemberName,
                        BMemberPhone = b.Phone,
                        BAddress = b.Address,
                        //PickUpDate = o.PickUpDate,
                        //PickUpTime = o.PickUpTime,
                        //PickUpType = o.PickUpType,
                        PickUpPerson = o.PickUpPerson,
                        PickUpPersonPhone = o.PickUpPersonPhone,
                        //PayTernCatId = a.PaymentType,
                        OrderState = o.OrderState,
                        Memo = o.Memo,
                        OrderTime = o.OrderTime,
                        TotalAmount = o.TotalAmount

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


            vm.TotalQty = 0;

            if (q != null)
            {

                foreach (var c in q.ToList())
                {

                    vm.OrderTime = c.OrderTime;
                    vm.TotalAmount = c.TotalAmount;
                    vm.PickUpPerson = c.PickUpPerson;
                    vm.Address = c.BAddress;
                    vm.BMemberName = c.BMemberName;
                    vm.BMemberPhone = c.BMemberPhone;
                    vm.Fid = c.Fid;
                    vm.BFid = c.BFid;
                    vm.NFid = c.NFid;
                    //vm.PickUpTime = c.PickUpTime;
                    //vm.PickUpDate = c.PickUpDate;
                    //vm.PickUpType = c.PickUpType;
                    vm.PickUpPersonPhone = c.PickUpPersonPhone;
                    //vm.PayTermCatId = c.PayTernCatId;
                    vm.Memo = c.Memo;

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

    }
}
