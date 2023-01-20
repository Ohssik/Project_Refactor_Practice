using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;
using System.Text.Json;

namespace prjMSIT145_Final.Controllers
{
	public class AddMaterialController : Controller
	{
		ispanMsit145shibaContext _context;
		IWebHostEnvironment _host;
		public AddMaterialController(ispanMsit145shibaContext context, IWebHostEnvironment host)
		{
			_context = context;
			_host = host;
		}

		public IActionResult BList()
		{
			return View();
		}
		public ActionResult BSearch(string keyword)
		{
			string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
			BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
			//_context.ProductCategories.Where(p => p.BFid == login.Business_fId).Join(_context.Products, proC => proC.Fid, pro => pro.CategoryFid, (proC, pro) => new
			//var datas = (from pro in _context.Products
			//			 join proC in _context.ProductCategories
			//			 on pro.CategoryFid equals proC.Fid
			//			 select new
			//			 {
			//				 pro.Fid,
			//				 pro.BFid,
			//				 pro.UnitPrice,
			//				 pro.IsForSale,
			//				 pro.Memo,
			//				 pro.Photo,
			//				 pro.ProductName,
			//				 pro.CategoryFid,
			//				 proC.CategoryName
			//			 }).Where(p => p.BFid == member.Fid).OrderBy(b => b.CategoryFid);
			var datas = _context.ViewOptionsToGroups.Where(p => p.BFid == member.Fid).OrderBy(o => o.OptionGroupName);
			if (keyword != null)
				datas = datas.Where(k => k.OptionName.Contains(keyword) || k.OptionGroupName.Contains(keyword)).OrderBy(o => o.OptionGroupName);

			//List<CProductOptionViewModel> materialList = new List<CProductOptionViewModel>();
			//foreach (var data in datas)
			//{
			//	CProductOptionViewModel vm = new CProductOptionViewModel();
			//	vm.Fid = data.Fid;
			//	vm.BFid = data.BFid;
			//	vm.OptionGroupFid = data.OptionGroupFid;
			//	vm.UnitPrice = data.UnitPrice;
			//	vm.OptionGroupName = data.OptionGroupName;
			//	vm.OptionName = data.OptionName;
			//	materialList.Add(vm);
			//}
			return Json(datas);
		}
		public ActionResult BCreate()
		{
			string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
			BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
			CProductOptionViewModel vm = new CProductOptionViewModel();
			vm.BFid = member.Fid;
			return View(vm);
		}
		[HttpPost]
		public ActionResult BCreate(CProductOptionViewModel vm)
		{
			var optGp = _context.ProductOptionGroups.FirstOrDefault(o => o.OptionGroupName == vm.OptionGroupName);
			vm.options.OptionGroupFid = optGp.Fid;
			_context.ProductOptions.Add(vm.options);
			_context.SaveChanges();
			return RedirectToAction("BList");
		}
		public ActionResult BOptionGroup()
		{
			string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
			BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
			var data = (_context.ProductOptionGroups.Where(b => b.BFid == member.Fid)).OrderBy(o=>o.OptionGroupName);
			return Json(data);
		}
		public ActionResult BEdit(int? id)
		{
			string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
			BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
			var datas = _context.ViewOptionsToGroups.Where(o => o.Fid == id);
			CProductOptionViewModel vm = new CProductOptionViewModel();
			foreach (var d in datas)
			{
				vm.Fid = d.Fid;
				vm.BFid = member.Fid;
				vm.UnitPrice = d.UnitPrice;
				vm.OptionGroupFid = d.OptionGroupFid;
				vm.OptionName = d.OptionName;
				vm.OptionGroupName = d.OptionGroupName;
			}
			return View(vm);
		}
		[HttpPost]
		public ActionResult BEdit(CProductOptionViewModel vm)
		{
			ProductOptionGroup optGp = _context.ProductOptionGroups.FirstOrDefault(o => o.OptionGroupName == vm.OptionGroupName);
			ProductOption opt = _context.ProductOptions.FirstOrDefault(o => o.Fid == vm.Fid);
			if (optGp != null && opt != null)
			{
				opt = vm.options;
				opt.OptionGroupFid = optGp.Fid;
				_context.SaveChanges();
			}
			return RedirectToAction("BList");
		}
		public ActionResult BDelete(ProductOption opt, int? id)
		{

			opt = _context.ProductOptions.FirstOrDefault(o => o.Fid == id);
			if (opt != null)
			{
				_context.ProductOptions.Remove(opt);
				_context.SaveChanges();
			}
			return RedirectToAction("BList");
		}
	}
}
