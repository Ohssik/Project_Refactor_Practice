using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using System.Collections.Generic;
using prjMSIT145_Final.ViewModels;

namespace prjMSIT145_Final.Controllers
{
    public class OrderController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public OrderController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
        public IActionResult CCartList(int? NFid)
        {
            NFid = 1;
            if(NFid==null)
                return Redirect("/CustomerMember/Login");
            List<VOrdersViewModel> OrdersList = new List<VOrdersViewModel>();
            var Orderdatas = from O in _context.Orders
                             where O.NFid == NFid
                             select new
                                 {
                                 Fid=O.Fid,
                                 NFid=O.NFid,
                                 BFid=O.BFid,
                                 PickUpDate=O.PickUpDate,
                                 PickUpTime=O.PickUpTime,
                                 PickUpType=O.PickUpType,
                                 PickUpPerson=O.PickUpPerson,
                                 PickUpPersonPhone=O.PickUpPersonPhone,
                                 PayTermCatId=O.PayTermCatId,
                                 TaxIdnum=O.TaxIdnum,
                                 OrderState=O.OrderState,
                                 Memo  = O.Memo,
                                 OrderTime=O.OrderTime,
                                 TotalAmount=O.TotalAmount,
                                 OrderISerialId=O.OrderISerialId,  
                             };
            foreach (var item in Orderdatas)
            {
                OrdersList.Add(new VOrdersViewModel
                {
                    Fid = item.Fid,
                    NFid = item.NFid,
                    BFid = item.BFid,
                    PickUpDate = item.PickUpDate,
                    PickUpTime = item.PickUpTime,
                    PickUpType = item.PickUpType,
                    PickUpPerson = item.PickUpPerson,
                    PickUpPersonPhone = item.PickUpPersonPhone,
                    PayTermCatId = item.PayTermCatId,
                    TaxIdnum = item.TaxIdnum,
                    OrderState = item.OrderState,
                    Memo = item.Memo,
                    OrderTime = item.OrderTime,
                    TotalAmount = item.TotalAmount,
                    OrderISerialId = item.OrderISerialId,
                });
            }
            CUtility.OrdersList = OrdersList;
            return View(CUtility.OrdersList);
        }




        //-------------------------------------------------------B、C分界線-------------------------------------------------------//
        [HttpPost]
        public IActionResult BList(int? orderid)
        {
           

        //    return View();
        //}
        //新訂單
        public IActionResult BNewList()
        {
            string state = "1";
            string json = "";
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
            {
               json= HttpContext.Session.GetString(CDictionary.SK_LOGINED_Business);
               BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
              var order = _context.Orders.Where(o => o.BFid == member.Fid && o.OrderState == state);
                var Datas = order.Join(_context.NormalMembers, o => o.NFid, n => n.Fid, (o, n) => new
                {
                    Fid = o.Fid,
                    NFid = o.NFid,
                    BFid = o.BFid,
                    PickUpDate = o.PickUpDate,
                    PickUpTime = o.PickUpTime,
                    PickUpType = o.PickUpType,
                    PickUpPerson = o.PickUpPerson,
                    PickUpPersonPhone = o.PickUpPersonPhone,
                    PayTermCatId = o.PayTermCatId,
                    TaxIdnum = o.TaxIdnum,
                    OrderState = o.OrderState,
                    Memo = o.Memo,
                    OrderTime = o.OrderTime,
                    TotalAmount = o.TotalAmount,
                    OrderISerialId = o.OrderISerialId,
                    NmbName = n.MemberName,

                });
                List <COrderViewModel> vm = new List <COrderViewModel>();
                foreach(var o in Datas)
                {
                    COrderViewModel model = new COrderViewModel();
                    model.Fid = o.Fid;
                    model.NFid = o.NFid;
                    model.BFid = o.BFid;
                    model.PickUpDate = o.PickUpDate;
                    model.PickUpTime = o.PickUpTime;
                    model.PickUpType = o.PickUpType;
                    model.PickUpPerson = o.PickUpPerson;
                    model.PickUpPersonPhone = o.PickUpPersonPhone;
                    model.PayTermCatId = o.PayTermCatId;
                    model.TaxIdnum = o.TaxIdnum;
                    model.OrderState = o.OrderState;
                    model.Memo = o.Memo;
                    model.OrderTime = o.OrderTime;
                    model.TotalAmount = o.TotalAmount;
                    model.OrderISerialId = o.OrderISerialId;
                    model.NmbName = o.NmbName;
                    vm.Add(model);
                }
                return View(vm);
            }
            return RedirectToAction("Blogin", "BusinessMember");

        }
        //訂單
        public IActionResult BList()
        {
            string state = "2";
            string json = "";
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
            {
                json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_Business);
                BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
                var order = _context.Orders.Where(o => o.BFid == member.Fid && (o.OrderState == state || o.OrderState == "3"));
                var Datas = order.Join(_context.NormalMembers, o => o.NFid, n => n.Fid, (o, n) => new
                {
                    Fid = o.Fid,
                    NFid = o.NFid,
                    BFid = o.BFid,
                    PickUpDate = o.PickUpDate,
                    PickUpTime = o.PickUpTime,
                    PickUpType = o.PickUpType,
                    PickUpPerson = o.PickUpPerson,
                    PickUpPersonPhone = o.PickUpPersonPhone,
                    PayTermCatId = o.PayTermCatId,
                    TaxIdnum = o.TaxIdnum,
                    OrderState = o.OrderState,
                    Memo = o.Memo,
                    OrderTime = o.OrderTime,
                    TotalAmount = o.TotalAmount,
                    OrderISerialId = o.OrderISerialId,
                    NmbName = n.MemberName

                });
                List<COrderViewModel> vm = new List<COrderViewModel>();
                foreach (var o in Datas)
                {
                    COrderViewModel model = new COrderViewModel();
                    model.Fid = o.Fid;
                    model.NFid = o.NFid;
                    model.BFid = o.BFid;
                    model.PickUpDate = o.PickUpDate;
                    model.PickUpTime = o.PickUpTime;
                    model.PickUpType = o.PickUpType;
                    model.PickUpPerson = o.PickUpPerson;
                    model.PickUpPersonPhone = o.PickUpPersonPhone;
                    model.PayTermCatId = o.PayTermCatId;
                    model.TaxIdnum = o.TaxIdnum;
                    model.OrderState = o.OrderState;
                    model.Memo = o.Memo;
                    model.OrderTime = o.OrderTime;
                    model.TotalAmount = o.TotalAmount;
                    model.OrderISerialId = o.OrderISerialId;
                    model.NmbName = o.NmbName;
                    vm.Add(model);
                }
                return View(vm);
            }
            return RedirectToAction("Blogin", "BusinessMember");

        }
        //舊訂單
        public IActionResult BOldList()
        {
            string state = "4";
            string json = "";
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
            {
                json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_Business);
                BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
                var order = _context.Orders.Where(o => o.BFid == member.Fid &&( o.OrderState == state ||o.OrderState =="5" || o.OrderState == "6") );
                var Datas = order.Join(_context.NormalMembers, o => o.NFid, n => n.Fid, (o, n) => new
                {
                    Fid = o.Fid,
                    NFid = o.NFid,
                    BFid = o.BFid,
                    PickUpDate = o.PickUpDate,
                    PickUpTime = o.PickUpTime,
                    PickUpType = o.PickUpType,
                    PickUpPerson = o.PickUpPerson,
                    PickUpPersonPhone = o.PickUpPersonPhone,
                    PayTermCatId = o.PayTermCatId,
                    TaxIdnum = o.TaxIdnum,
                    OrderState = o.OrderState,
                    Memo = o.Memo,
                    OrderTime = o.OrderTime,
                    TotalAmount = o.TotalAmount,
                    OrderISerialId = o.OrderISerialId,
                    NmbName = n.MemberName,

                });
                List<COrderViewModel> vm = new List<COrderViewModel>();
                foreach (var o in Datas)
                {
                    COrderViewModel model = new COrderViewModel();
                    model.Fid = o.Fid;
                    model.NFid = o.NFid;
                    model.BFid = o.BFid;
                    model.PickUpDate = o.PickUpDate;
                    model.PickUpTime = o.PickUpTime;
                    model.PickUpType = o.PickUpType;
                    model.PickUpPerson = o.PickUpPerson;
                    model.PickUpPersonPhone = o.PickUpPersonPhone;
                    model.PayTermCatId = o.PayTermCatId;
                    model.TaxIdnum = o.TaxIdnum;
                    model.OrderState = o.OrderState;
                    model.Memo = o.Memo;
                    model.OrderTime = o.OrderTime;
                    model.TotalAmount = o.TotalAmount;
                    model.OrderISerialId = o.OrderISerialId;
                    model.NmbName = o.NmbName;
                    vm.Add(model);
                }
                return View(vm);
            }
            return RedirectToAction("Blogin", "BusinessMember");

        }
        //改訂單狀態
        public IActionResult BEditStateApi(int? State,int?nextState,int?orderid)
        {
            string _Action = "";
            if (State == 1)
            {
                _Action = "BNewList";
            }
            else if (State == 2|| State == 3)
            {
                _Action = "BList";
            }
            else 
            {
                _Action = "BOldList";
            }

            var order = _context.Orders.FirstOrDefault(o => o.Fid == orderid);
            if (order == null)
            return RedirectToAction($"${_Action}");
            order.OrderState = nextState.ToString();
            _context.SaveChanges();
            return RedirectToAction($"{_Action}");
        }
        public IActionResult BListDetailApi(int? orderid)
        {
            var order = _context.Orders.Where(o => o.Fid == orderid);
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
                OrderISerialId = o.OrderISerialId
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
                ItemPrice = p.UnitPrice
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
