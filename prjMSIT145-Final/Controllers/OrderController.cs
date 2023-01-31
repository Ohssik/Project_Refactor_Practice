using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
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
            List<VOrdersViewModel> OrderList = new List<VOrdersViewModel>();
            var Orderrdatas = from O in _context.Orders
                              where O.NFid == NFid
                              select new
                              {
                                  Fid = O.Fid,
                                  NFid = O.NFid,
                                  BFid = O.BFid,
                                  PickUpDate = O.PickUpDate,
                                  PickUpTime = O.PickUpTime,
                                  PickUpType = O.PickUpType,
                                  PickUpPerson= O.PickUpPerson,
                                  PickUpPersonPhone= O.PickUpPersonPhone,
                                  PayTermCatId= O.PayTermCatId,
                                  TaxIdnum= O.TaxIdnum,
                                  OrderState= O.OrderState,
                                  Memo= O.Memo,
                                  OrderTime= O.OrderTime,
                                  TotalAmount= O.TotalAmount,
                                  OrderISerialId= O.OrderISerialId,
                              };
                             
            foreach (var item in Orderrdatas)
            {
                OrderList.Add(new VOrdersViewModel
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
            CUtility.OrderList = OrderList;
            return View(CUtility.OrderList);
        }

        //public IActionResult CCartAddOrder(int? OrderID)
        //{
        //    NFid = 1;
        //    if (NFid == null)
        //        return Redirect("/CustomerMember/Login");
        //    List<VOrdersViewModel> OrderList = new List<VOrdersViewModel>();
        //    var Orderrdatas = from O in _context.Orders
        //                      where O.NFid == NFid
        //                      select new
        //                      {
        //                          Fid = O.Fid,
        //                          NFid = O.NFid,
        //                          BFid = O.BFid,
        //                          PickUpDate = O.PickUpDate,
        //                          PickUpTime = O.PickUpTime,
        //                          PickUpType = O.PickUpType,
        //                          PickUpPerson = O.PickUpPerson,
        //                          PickUpPersonPhone = O.PickUpPersonPhone,
        //                          PayTermCatId = O.PayTermCatId,
        //                          TaxIdnum = O.TaxIdnum,
        //                          OrderState = O.OrderState,
        //                          Memo = O.Memo,
        //                          OrderTime = O.OrderTime,
        //                          TotalAmount = O.TotalAmount,
        //                          OrderISerialId = O.OrderISerialId,
        //                      };

        //    foreach (var item in Orderrdatas)
        //    {
        //        OrderList.Add(new VOrdersViewModel
        //        {
        //            Fid = item.Fid,
        //            NFid = item.NFid,
        //            BFid = item.BFid,
        //            PickUpDate = item.PickUpDate,
        //            PickUpTime = item.PickUpTime,
        //            PickUpType = item.PickUpType,
        //            PickUpPerson = item.PickUpPerson,
        //            PickUpPersonPhone = item.PickUpPersonPhone,
        //            PayTermCatId = item.PayTermCatId,
        //            TaxIdnum = item.TaxIdnum,
        //            OrderState = item.OrderState,
        //            Memo = item.Memo,
        //            OrderTime = item.OrderTime,
        //            TotalAmount = item.TotalAmount,
        //            OrderISerialId = item.OrderISerialId,
        //        });
        //    }
        //    CUtility.OrderList = OrderList;
        //    return View(CUtility.OrderList);
        //}
    }
}
