using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;
using System.Text.Json;

namespace prjMSIT145_Final.Controllers
{
    public class TypeController : Controller
    {
        private readonly ispanMsit145shibaContext _context;
        public TypeController(ispanMsit145shibaContext context)
        {
            _context = context;
        }
		//商品類別
		public ActionResult BCategoryList()
		{
			string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
			BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
			var proC = _context.ProductCategories.Where(o => o.BFid == member.Fid).OrderBy(o=>o.CategoryName);
			List<CProductsViewModel> list = new List<CProductsViewModel>();
			foreach (var d in proC)
			{
				if (d.CategoryName != null)
				{
					CProductsViewModel vm = new CProductsViewModel();
					vm.proCategory = d;
					list.Add(vm);
				}
			}
			return Json(list);
		}
		//配料類別
		public ActionResult BOptionGroup()
		{
			string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
			BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
			var data = (_context.ProductOptionGroups.Where(b => b.BFid == member.Fid)).OrderBy(o => o.OptionGroupName);
			return Json(data);
		}
		//商品類別新刪修
		public IActionResult BItemTypeAddnEdit(ProductCategory proC,int? id)
        {
			string json = "";
			if(HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
			{
				json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
				BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
				proC.BFid = member.Fid;
				if(id != null)
				{
					proC = _context.ProductCategories.FirstOrDefault(f => f.Fid == id);
				}
				return View(proC);
			}
			return RedirectToAction("BLogin", "BusinessMember");
        }
		public ActionResult BItemTypeDelete(int? id)
		{
			return RedirectToAction("BItemTypeAddnEdit");
		}
		//配料類別新刪修
		public IActionResult BMaterialTypeAddnEdit(int? id)
		{
			return View();
		}
		public ActionResult BMaterialTypeDelete(int? id)
		{
			return RedirectToAction("BMaterialTypeAddnEdit");
		}
	}
}
