using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;
using System.Text;
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
			try
			{
				if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
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
							if (!list.Any(o => o.CategoryName == vm.CategoryName))
								list.Add(vm);
						}
					}
					return Json(list);

				}
				else
					return RedirectToAction("Blogin", "BusinessMember");
			}
			catch
			{
				return Json("商品類別載入有誤");
			}
		}
		//配料類別
		public ActionResult BOptionGroup()
		{
			if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
			{
				string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
				BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
				var data = (_context.ProductOptionGroups.Where(b => b.BFid == member.Fid)).OrderBy(o => o.OptionGroupName);
				List<ProductOptionGroup> list = new List<ProductOptionGroup>();
				foreach (var d in data)
				{
					ProductOptionGroup gp = new ProductOptionGroup();
					gp = d;
					if (!list.Any(o => o.OptionGroupName == gp.OptionGroupName))
						list.Add(gp);
				}
				return Json(list);
			}
			else
				return RedirectToAction("Blogin", "BusinessMember");
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
			else
				return RedirectToAction("BLogin", "BusinessMember");
		}
		//按下submit
		public ActionResult BItemTypeSubmit(ProductCategory proC)
		{
			if (proC != null)
			{
				var data = _context.ProductCategories.FirstOrDefault(o => o.Fid == proC.Fid);
				if (data != null)
				{
					data.CategoryName = proC.CategoryName;
				}
				else
					_context.ProductCategories.Add(proC);

				_context.SaveChanges();
			}
			return RedirectToAction("BItemTypeAddnEdit");
		}
		//刪除類別時,判斷有無產品使用此類別,並提醒使用者
		public ActionResult BItemTypeDeleteList(int? id)
		{
			if (_context.Products.Any(p => p.CategoryFid == id))
			{
				var count = _context.Products.Where(p => p.CategoryFid == id).Count();
				return Content($"此類別尚有\"{count}\"筆產品,若刪除類別,則產品會一併刪除,確定刪除嗎?", "text/plain", Encoding.UTF8);
			}
			else
				return Content("確定刪除此類別嗎?", "text/plain", Encoding.UTF8);
		}
		//商品類別刪除
		public ActionResult BItemTypeDelete(ProductCategory proC, int? id)
		{
			//若商品中有使用此類別則一併刪除
			if (id != null)
			{
				proC = _context.ProductCategories.FirstOrDefault(o => o.Fid == id);
				if (proC != null)
				{

					if (_context.Products.Any(p => p.CategoryFid == proC.Fid))
					{
						var pro = _context.Products.Where(p => p.CategoryFid == proC.Fid);
						_context.Products.RemoveRange(pro);
					}
					_context.ProductCategories.Remove(proC);
					_context.SaveChanges();
				}
			}
			return RedirectToAction("BItemTypeAddnEdit");
		}
		//配料類別新修
		public IActionResult BMaterialTypeAddnEdit()
		{
			string json = "";
			if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
			{
				json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
				BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
				ProductOptionGroup optGp = new ProductOptionGroup();
				optGp.BFid = member.Fid;
				return View(optGp);
			}
			else
				return RedirectToAction("BLogin", "BusinessMember");
		}
		//按下儲存鈕
		public ActionResult BMaterialTypeSubmit(ProductOptionGroup optGp)
		{
			if (optGp != null)
			{
				var data = _context.ProductOptionGroups.FirstOrDefault(o => o.Fid == optGp.Fid);
				if (data != null)
				{
					data.OptionGroupName = optGp.OptionGroupName;
					data.IsMultiple = optGp.IsMultiple;
					data.Memo = optGp.Memo;
				}
				else
					_context.ProductOptionGroups.Add(optGp);

				_context.SaveChanges();
			}
			return RedirectToAction("BMaterialTypeAddnEdit");
		}
		//刪除類別時判斷有無配料使用並提醒使用者
		public ActionResult BMaterialTypeDeleteList(int? id)
		{
			if (_context.ProductOptions.Any(o => o.OptionGroupFid == id))
			{
				var count = _context.ProductOptions.Where(o => o.OptionGroupFid == id).Count();
				return Content($"此類別尚有\"{count}\"筆配料使用,若刪除類別會一併刪除,確定刪除嗎?", "text/plain", Encoding.UTF8);
			}
			else
				return Content("確定刪除此類別嗎?", "text/plain", Encoding.UTF8);
		}
		public ActionResult BMaterialTypeDelete(int? id)
		{
			//若商品中有使用此類別則一併刪除
			if (id != null)
			{
				var data = _context.ProductOptionGroups.FirstOrDefault(o => o.Fid == id);
				if (_context.ProductOptions.Any(o => o.OptionGroupFid == id))
				{
					var op = _context.ProductOptions.Where(o => o.OptionGroupFid == id);
					_context.ProductOptions.RemoveRange(op);
				}

				_context.ProductOptionGroups.Remove(data);
				_context.SaveChanges();
			}
			return RedirectToAction("BMaterialTypeAddnEdit");
		}
	}
}
