﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;


namespace prjMSIT145_Final.Controllers
{
    public class OrderController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public OrderController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult BList(int? orderid)
        {
           

            return View();
        }
        public IActionResult BList(string? state)
        {
          
            string json = "";
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
               json= HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
               BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
               List<Order> order =_context.Orders.Where(o => o.Fid == member.Fid&&o.OrderState== state).ToList(); 
                return View(order);
            }
            return RedirectToAction("Blogin", "BusinessMember");

        }
        public IActionResult BDelete()
        {
            return View();
        }
        public IActionResult BListDetailApi(int? orderid)
        {
            var order = _context.Orders.Where(o => o.Fid == orderid);
            //合併items
           
            //合併客戶名稱
            var orderdMbName = order.Join(_context.NormalMembers, o => o.NFid, N => N.Fid, (o, N) => new
            {
                Fid = o.Fid,
                NFid = o.NFid,
                BFid = o.BFid,
                MbName = N.MemberName,
                PickUpDate = o.PickUpDate,
                PickUpTime = o.PickUpTime,
                PickUpType = o.PickUpType,
                PickUpPerson = o.PickUpPerson,
                PickUpPersonPhone = o.PickUpPersonPhone,
                PayTermCatId = o.PayTermCatId,//還須合併付款方法?
                TaxIdnum = o.TaxIdnum,
                OrderState = o.OrderState,
                Memo = o.Memo,
                OrderTime = o.OrderTime,
                TotalAmount = o.TotalAmount,
                OrderISerialId = o.OrderISerialId,
            });
            //合併產品名
            var orderitemToProduct = _context.OrderItems.Join(_context.Products, o => o.ProductFid, p => p.Fid, (o, p) => new
            {
                //ProductFid = o.ProductFid,
                fid=o.Fid,
                ProductName = p.ProductName,
                ProductQty = o.Qty,
                Productprice = p.UnitPrice,
                OrderFid = o.OrderFid
            }); 
            var ItemToName = _context.OrderOptionsDetails.Join(_context.ProductOptions, i => i.OptionFid, p => p.Fid, (i, p) => new
            {
                Fid = i.Fid,
                ItemId=i.ItemFid,
                OptionName = p.OptionName,
                ItemPrice = p.UnitPrice,
            });


            List<COrderListViewModel> list = new List<COrderListViewModel>();
            foreach (var datas in orderdMbName)
            {
                COrderListViewModel vm = new COrderListViewModel();
                vm.Fid = datas.Fid;
                vm.BFid = datas.BFid;
                vm.NFid = datas.NFid;
                vm.MbName = datas.MbName;
                vm.PickUpDate = datas.PickUpDate;
                vm.PickUpTime = datas.PickUpTime;
                vm.PickUpType = datas.PickUpType;
                vm.PickUpPerson = datas.PickUpPerson;
                vm.PickUpPersonPhone = datas.PickUpPersonPhone;
                vm.PayTermCatId = datas.PayTermCatId;
                vm.TaxIdnum = datas.TaxIdnum;
                vm.OrderState = datas.OrderState;
                vm.Memo = datas.Memo;
                vm.OrderTime = datas.OrderTime;
                vm.TotalAmount = datas.TotalAmount;
                vm.OrderISerialId = datas.OrderISerialId;
                vm.totalQty = 0;
                vm.items = new List<COrderItemViewModel>();
                //合併配料
                
                var orderitem = orderitemToProduct.Where(i => i.OrderFid == datas.Fid);
                foreach (var items in orderitem)
                {
                    COrderItemViewModel itemVm= new COrderItemViewModel();
                    itemVm.ProductName= items.ProductName;
                    itemVm.Productprice = items.Productprice;
                    itemVm.Qty = items.ProductQty;
                    vm.totalQty += items.ProductQty;
                    itemVm.OptionName = new List<string>();
                    itemVm.OptionPrice = 0;
                var itemOption = ItemToName.Where(o => o.ItemId == items.fid);
                    foreach(var Option in itemOption)
                    {
                        itemVm.OptionName.Add(Option.OptionName);
                        itemVm.OptionPrice += Option.ItemPrice;
                    }
                    vm.items.Add(itemVm);


                }
                

                list.Add(vm);
            }

            return Json(list);
        }

    }
}
