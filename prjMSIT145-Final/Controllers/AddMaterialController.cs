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
			string json = "";
			if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
			{
				json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
				BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
				
					var datas = (from pro in _context.ProductOptions
								 join proC in _context.ProductOptionGroups
								 on pro.OptionGroupFid equals proC.Fid
								 select new
								 {
									 pro.Fid,
									 pro.BFid,
									 pro.UnitPrice,
									 pro.OptionName,
									 pro.OptionGroupFid,
									 proC.OptionGroupName,
									 proC.Memo
								 }).Where(p => p.BFid == member.Fid).OrderBy(b => b.OptionGroupName);
					//var datas = _context.ViewOptionsToGroups.Where(p => p.BFid == member.Fid).OrderBy(o => o.OptionGroupName);
					if (keyword != null)
						datas = datas.Where(k => k.OptionName.Contains(keyword) || k.OptionGroupName.Contains(keyword)).OrderBy(o => o.OptionGroupName);

					List<CProductOptionViewModel> materialList = new List<CProductOptionViewModel>();
					foreach (var data in datas)
					{
						CProductOptionViewModel vm = new CProductOptionViewModel();
						vm.Fid = data.Fid;
						vm.BFid = data.BFid;
						vm.OptionGroupFid = data.OptionGroupFid;
						vm.UnitPrice = Math.Round(Convert.ToDouble(data.UnitPrice));
						vm.OptionGroupName = data.OptionGroupName;
						vm.OptionName = data.OptionName;

						materialList.Add(vm);
					}
					return Json(materialList);
			}
			return RedirectToAction("BLogin", "BusinessMember");
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
				vm.UnitPrice = Math.Round(Convert.ToDouble(d.UnitPrice));
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
				opt.UnitPrice = Convert.ToDecimal(vm.UnitPrice);
				opt.OptionName = vm.OptionName;
				opt.OptionGroupFid = optGp.Fid;
				_context.SaveChanges();
			}
			return RedirectToAction("BList");
		}
		public ActionResult BDelete(ProductOption opt, int? id)
		{
			if (id != null)
			{
				opt = _context.ProductOptions.FirstOrDefault(o => o.Fid == id);
				if (opt != null)
				{
					_context.ProductOptions.Remove(opt);
					_context.SaveChanges();
				}
			}
			return RedirectToAction("BList");
		}
	}
}
