using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModels;

namespace prjMSIT145_Final.Controllers
{
    public class CouponController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public CouponController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {           
            return View();
        }
        public IActionResult ACouponList()
        {
            List<CACouponViewModel> list = new List<CACouponViewModel>();
            //if (k == null)
            //{

            IEnumerable<Coupon> coupons = from c in _context.Coupons
                                                  select c;
            //}
            if (coupons != null)
            {
                foreach (Coupon c in coupons)
                {
                    CACouponViewModel cvm = new CACouponViewModel();
                    cvm.coupon = c;
                    list.Add(cvm);
                }

            }

            return View(list);
            
        }
        public IActionResult ACouponDetails()
        {
            return View();
        }
    }
}
