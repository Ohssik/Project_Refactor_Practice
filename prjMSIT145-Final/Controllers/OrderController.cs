using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;
using System.Linq;
using System.Diagnostics.Metrics;


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
            var o = _context.Orders.FirstOrDefault();
            return View();
        }
        [HttpPost]
        public IActionResult BList()
        {
            var order = _context.Orders.Join(_context.OrderItems, o => o.Fid, i => i.OrderFid, (o, i) => new
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
            var orderdeital = order.Join(_context.OrderOptionsDetails, o => o.OrderItemfid, d => d.ItemFid, (o, d) => new
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
     
            return View();
        }
        public IActionResult BList(string? state)
        {
          
            string json = "";
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
               json= HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
               BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
               List<Order> order =_context.Orders.Where(o => o.Fid == member.Fid).ToList(); 
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
