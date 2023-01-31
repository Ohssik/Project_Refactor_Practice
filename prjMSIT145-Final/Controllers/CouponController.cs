using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

            var coupons = from c in _context.Coupons
                          join cn in _context.Coupon2NormalMembers on c.Fid equals cn.CouponId
                          into sGroup
                          from s in sGroup.DefaultIfEmpty()
                          join n in _context.NormalMembers on s.MemberId equals n.Fid
                          into sg2
                          from sg in sg2.DefaultIfEmpty()
                          select new
                          {
                              c.Fid,
                              c.Price,
                              c.CouponCode,
                              c.Memo,
                              c.IsUsed,
                              NmemberID = s.MemberId,
                              NmemberName = sg.MemberName
                          };

            if (coupons != null)
            {
                foreach (var c in coupons.ToList())
                {
                    CACouponViewModel cvm = new CACouponViewModel();
                    cvm.Fid = c.Fid;
                    cvm.Price = c.Price;
                    cvm.CouponCode = c.CouponCode;
                    cvm.Memo = c.Memo;
                    cvm.IsUsed = c.IsUsed;
                    cvm.NmemberID=c.NmemberID;
                    cvm.NmemberName=c.NmemberName;
                    list.Add(cvm);
                }

            }

            return View(list);
            
        }
        public IActionResult ACouponEdit(string data)
        {
            string result = "0";
            CACouponViewModel cac=JsonConvert.DeserializeObject<CACouponViewModel>(data);
            if (cac != null)
            {
                if (cac.Fid > 0)
                {
                    var coup = _context.Coupons.FirstOrDefault(t => t.Fid == cac.Fid);
                    if (coup != null)
                    {
                        coup.Price = cac.Price;
                        coup.Memo = cac.Memo;
                        coup.IsUsed = cac.IsUsed;
                        
                        Coupon2NormalMember c2n = _context.Coupon2NormalMembers.FirstOrDefault(c => c.CouponId==cac.Fid);
                        if (c2n!=null)
                        {
                            c2n.MemberId = cac.NmemberID;
                        }
                        else
                        {
                            c2n=new Coupon2NormalMember();
                            c2n.MemberId = cac.NmemberID;
                            c2n.CouponId=cac.Fid;
                            _context.Coupon2NormalMembers.Add(c2n);
                                
                        }
                            
                        
                    }
                }else if (cac.Fid==0)
                {
                    Coupon newCoup = new Coupon();                    
                    newCoup.Price = cac.Price;
                    newCoup.Memo = cac.Memo;
                    newCoup.IsUsed = cac.IsUsed;
                    newCoup.CouponCode=Guid.NewGuid().ToString().Substring(0,10);
                    _context.Coupons.Add(newCoup);
                    if (cac.NmemberID!=null)
                    {
                        Coupon2NormalMember c2n = new Coupon2NormalMember();
                                                        
                        c2n.MemberId = cac.NmemberID;
                        c2n.CouponId=cac.Fid;
                        _context.Coupon2NormalMembers.Add(c2n);                            

                    }
                }
                

                _context.SaveChanges();
                result="1";
            }

            //return RedirectToAction("ACouponList");
            return NoContent();

        }
        
        public IActionResult ACouponDelete(int id)
        {
            //var coup = await _context.Coupons.FindAsync(id);
            var coup = _context.Coupons.FirstOrDefault(c => c.Fid==id);
            if (coup == null)
            {
                return NotFound();
            }
            var c2n = _context.Coupon2NormalMembers.FirstOrDefault(c => c.CouponId==id);
            if (c2n!=null)
                _context.Coupon2NormalMembers.Remove(c2n);
            _context.Coupons.Remove(coup);
            _context.SaveChanges();

            return RedirectToAction("ACouponList");
        }
    }
}
