using Microsoft.AspNetCore.Mvc;
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
            var order = _context.Orders.Where(o => o.Fid == orderid);
            //合併items
            var ordertoitems = order.Join(_context.OrderItems, o => o.Fid, i => i.OrderFid, (o, i) => new
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
                OrderItemfid = i.Fid,
                ProductFid = i.ProductFid,
                Qty = i.Qty
            });
            //合併細節
            var orderTodeital = ordertoitems.Join(_context.OrderOptionsDetails, o => o.OrderItemfid, d => d.ItemFid, (o, d) => new
            {
                Fid = o.Fid,
                NFid = o.NFid,
                BFid = o.BFid,
                PickUpDate = o.PickUpDate,
                PickUpTime = o.PickUpTime,
                PickUpType = o.PickUpType,
                PickUpPerson = o.PickUpPerson,
                PickUpPersonPhone = o.PickUpPersonPhone,
                PayTermCatId = o.PayTermCatId,//還須合併付款方法
                TaxIdnum = o.TaxIdnum,
                OrderState = o.OrderState,
                Memo = o.Memo,
                OrderTime = o.OrderTime,
                TotalAmount = o.TotalAmount,
                OrderISerialId = o.OrderISerialId,
                ProductFid = o.ProductFid,//還須合併產品名稱
                Qty = o.Qty,
                OptionFid = d.OptionFid//還須合併配料名稱
            });
            //合併客戶名稱
            var orderdeitaltoMbName = orderTodeital.Join(_context.NormalMembers, o => o.NFid, N => N.Fid, (o, N) => new
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
                ProductFid = o.ProductFid,//還須合併產品名稱
                Qty = o.Qty,
                OptionFid = o.OptionFid//還須合併配料名稱
            });
            //合併產品名
            var orderdeital = orderdeitaltoMbName.Join(_context.Products, o => o.ProductFid, p => p.Fid, (o, p) => new
            {
                Fid = o.Fid,
                NFid = o.NFid,
                BFid = o.BFid,
                MbName = o.MbName,
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
                //ProductFid = o.ProductFid,
                ProductName = p.ProductName,
                Qty = o.Qty,
                OptionFid = o.OptionFid//還須合併配料名稱
            });
           

            List<COrderListViewModel> list = new List<COrderListViewModel>();
            foreach(var datas in orderdeital)
            {
                COrderListViewModel vm= new COrderListViewModel();
                vm.Fid = datas.Fid;
                vm.BFid = datas.BFid;
                vm.NFid= datas.NFid;
                vm.MbName= datas.MbName;
                vm.PickUpDate = datas.PickUpDate;
                vm.PickUpTime = datas.PickUpTime;
                vm.PickUpType = datas.PickUpType;
                vm.PickUpPerson= datas.PickUpPerson;
                vm.PickUpPersonPhone= datas.PickUpPersonPhone;
                vm.PayTermCatId= datas.PayTermCatId;
                vm.TaxIdnum= datas.TaxIdnum;
                vm.OrderState= datas.OrderState;
                vm.Memo= datas.Memo;
                vm.OrderTime = datas.OrderTime;
                vm.TotalAmount = datas.TotalAmount;
                vm.OrderISerialId= datas.OrderISerialId;
                vm.ProductName = datas.ProductName;
                vm.Qty = datas.Qty;
                //合併配料
                var orderitem = _context.ProductOptions.Where(P => P.Fid == datas.OptionFid);
                foreach(var item in orderitem)
                {
                    vm.OptionName.Add(item.OptionName);
                    vm.OptionPrice += item.UnitPrice;
                }
                list.Add(vm);
            }

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
        
    }
}
