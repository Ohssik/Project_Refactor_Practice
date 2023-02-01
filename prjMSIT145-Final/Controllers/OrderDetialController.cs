using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Net;
using prjMSIT145_Final.ViewModel;
using Microsoft.EntityFrameworkCore;

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

            List<COrderDetialViewModel> list = new List<COrderDetialViewModel>();

            var q = from emp in _context.Orders
                    join g in _context.BusinessMembers
                    on emp.BFid equals g.Fid
                    select new
                    {
                        emp.OrderState,
                        emp.OrderTime,
                        emp.TotalAmount,
                        g.MemberName,
                        emp.Fid
                    };

            if (q != null)
            {
                
                foreach (var c in q.ToList())
                {
                    COrderDetialViewModel vm = new COrderDetialViewModel();
                    vm.OrderState = c.OrderState;
                    vm.OrderTime = c.OrderTime;
                    vm.TotalAmount = c.TotalAmount;
                    vm.BMemberName = c.MemberName;
                    vm.Fid = c.Fid;
                   

                    list.Add(vm);
                }

            }
            
            

            return View(list);
        }
        [HttpPost]
        public IActionResult ListInfo()
        {

            return View();
        }
    

    public IActionResult ListInfo(int? Fid)
    {


        
            

            var q = from o in _context.Orders
                               join b in _context.BusinessMembers
                               on o.BFid equals b.Fid
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
                                   PayTernCatId = o.PayTermCatId,
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


            COrderDetialViewModel vm = new COrderDetialViewModel();

            if (q != null)
            {

                foreach (var c in q.ToList())
                {
                    vm.OrderState = c.OrderState;
                    vm.OrderTime = c.OrderTime;
                    vm.TotalAmount = c.TotalAmount;
                    vm.PickUpPerson= c.PickUpPerson;
                    vm.Address = c.BAddress;
                    vm.BMemberName= c.BMemberName;
                    vm.BMemberPhone= c.BMemberPhone;
                    vm.Fid = c.Fid;
                    vm.BFid= c.BFid;
                    vm.NFid= c.NFid;
                    vm.PickUpTime= c.PickUpTime;
                    vm.PickUpDate= c.PickUpDate;
                    vm.PickUpType= c.PickUpType;
                    vm.PickUpPersonPhone= c.PickUpPersonPhone;
                    vm.PayTermCatId = c.PayTernCatId;
                    vm.Memo = c.Memo;
                    vm.items = new List<COrderItemViewModel>();

                    var orderitem = from i in Pr
                                    where i.OrderFid == c.Fid
                                    select i;

                    foreach (var item in orderitem)
                    {
                        COrderItemViewModel item2 = new COrderItemViewModel();
                        item2.ProductName = item.ProductName;
                        item2.Productprice = item.Productprice;
                        item2.Qty = item.ProductQty;
                        item2.OptionName = new List<string>();
                        item2.OptionPrice = 0;
                        vm.totalQty += item.ProductQty;

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
