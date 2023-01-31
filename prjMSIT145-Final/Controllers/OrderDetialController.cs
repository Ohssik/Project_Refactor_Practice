using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;
using System.Text.Json;
using System.Collections.Concurrent;
using System.Net;
using prjMSIT145_Final.ViewModel;

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


        public IActionResult List(CKeywordViewModel vm)
        {

            string keyword = vm.txtKeyword;
            IEnumerable<ViewShowFullOrder> datas = null;


            if (keyword == null)
                datas = from c in _context.ViewShowFullOrders
                        select c;
            else
                datas = _context.ViewShowFullOrders.Where(c => c.BMemberName.Contains(keyword)).ToList();

            List<COrderDetialViewModel> list = new List<COrderDetialViewModel>();
            foreach (var d in datas)
            {
                COrderDetialViewModel v = new COrderDetialViewModel();
                v.Order = d;
                list.Add(v);
            }
            return View(list);

        }

        public IActionResult ListInfo(int? orderid)
        {
            orderid = 1;

            var order = from o in _context.Orders
                        where o.Fid == orderid
                        select o;

            var orderBMemberName = from o in _context.Orders
                                   join b in _context.BusinessMembers
                                   on o.NFid equals b.Fid
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

            var orderitemProduct = from o in _context.OrderItems
                                   join p in _context.Products
                                   on o.ProductFid equals p.Fid
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

            List<COrderListViewModel> list = new List<COrderListViewModel>();
            foreach (var data in orderBMemberName)
            {
                COrderListViewModel vm = new COrderListViewModel();

                vm.Fid = data.Fid;
                vm.BFid = data.BFid;
                vm.NFid = data.NFid;
                vm.BMemberName = data.BMemberName;
                vm.BMemberPhone = data.BMemberPhone;
                vm.BAddress = data.BAddress;
                vm.PickUpDate = data.PickUpDate;
                vm.PickUpTime = data.PickUpTime;
                vm.PickUpType = data.PickUpType;
                vm.PickUpPerson = data.PickUpPerson;
                vm.PickUpPersonPhone = data.PickUpPersonPhone;
                vm.PayTermCatId = data.PayTernCatId;
                vm.OrderState = data.OrderState;
                vm.Memo = data.Memo;
                vm.OrderTime = data.OrderTime;
                vm.TotalAmount = data.TotalAmount;
                vm.totalQty = 0;
                vm.items = new List<COrderItemViewModel>();

                var orderitem = from i in orderitemProduct
                                where i.OrderFid == data.Fid
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

            return View();
        }
    }
}
