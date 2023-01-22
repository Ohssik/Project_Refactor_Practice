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
			var proC = _context.ProductCategories.Where(o => o.BFid == member.Fid).OrderBy(o => o.CategoryName);
			List<ProductCategory> list = new List<ProductCategory>();
			foreach (var d in proC)
			{
				if (d.CategoryName != null)
				{
					ProductCategory vm = new ProductCategory();
					vm = d;
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
		//商品類別新修
		public IActionResult BItemTypeAddnEdit()
		{
			string json = "";
			if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
			{
				json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
				BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
				ProductCategory proC = new ProductCategory();
				proC.BFid = member.Fid;
				return View(proC);
			}
			return RedirectToAction("BLogin", "BusinessMember");
		}
		//按下submit
		public ActionResult BItemTypeSubmit(ProductCategory vm)
		{
			if (vm != null)
			{
				var data = _context.ProductCategories.FirstOrDefault(o => o.Fid == vm.Fid);
				if (data != null)
				{
					data.CategoryName = vm.CategoryName;
				}
				else
					_context.ProductCategories.Add(vm);

				_context.SaveChanges();
			}
			return RedirectToAction("BItemTypeAddnEdit");
		}
		//商品類別刪除
		public ActionResult BItemTypeDelete(ProductCategory proC, int? id)
		{
			if (id != null)
			{
				proC = _context.ProductCategories.FirstOrDefault(o => o.Fid == id);
				if (proC != null)
				{
					_context.ProductCategories.Remove(proC);
					//_context.SaveChanges();
				}
			}
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
