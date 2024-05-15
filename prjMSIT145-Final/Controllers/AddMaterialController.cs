using Microsoft.AspNetCore.Mvc;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.Web.ViewModel;
using System.Text.Json;

namespace prjMSIT145Final.Web.Controllers
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
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
                return View();

            return RedirectToAction("Blogin", "BusinessMember");
        }
		public ActionResult BMaterialList(string keyword)
		{
			string json = "";
			if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
			{
				json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_Business);
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
			if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
			{
				string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_Business);
				BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
				CProductOptionViewModel vm = new CProductOptionViewModel();
				vm.BFid = member.Fid;
				return View(vm);
			}
			else
				return RedirectToAction("Blogin", "BusinessMember");
		}
		[HttpPost]
		public ActionResult BCreate(CProductOptionViewModel vm)
		{
			if (_context.ProductOptions.Any(op => op.OptionName == vm.OptionName && op.BFid == vm.BFid))
				return RedirectToAction("BList");
			else
			{
				var optGp = _context.ProductOptionGroups.FirstOrDefault(o => o.OptionGroupName == vm.OptionGroupName && o.BFid == vm.BFid);
				vm.options.OptionGroupFid = optGp.Fid;

				_context.ProductOptions.Add(vm.options);
				_context.SaveChanges();
				return RedirectToAction("BList");
			}
		}
		
		public ActionResult BEdit(CProductOptionViewModel vm)
		{
			if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_Business))
			{
					ProductOptionGroup optGp = _context.ProductOptionGroups.FirstOrDefault(o => o.OptionGroupName == vm.OptionGroupName && o.BFid == vm.BFid);
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
			else
				return RedirectToAction("Blogin", "BusinessMember");
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
