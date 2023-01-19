﻿using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using prjMSIT145_Final.ViewModel;
using System.Text.Json;

namespace prjMSIT145_Final.Controllers
{
	public class ProductController : Controller
	{
		private readonly ispanMsit145shibaContext _context;
		private readonly IWebHostEnvironment _host;
		public ProductController(ispanMsit145shibaContext context, IWebHostEnvironment host)
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

				//_context.ProductCategories.Where(p => p.BFid == login.Business_fId).Join(_context.Products, proC => proC.Fid, pro => pro.CategoryFid, (proC, pro) => new
				var datas = (from pro in _context.Products
							 join proC in _context.ProductCategories
							 on pro.CategoryFid equals proC.Fid
							 select new
							 {
								 pro.Fid,
								 pro.BFid,
								 pro.UnitPrice,
								 pro.IsForSale,
								 pro.Memo,
								 pro.Photo,
								 pro.ProductName,
								 pro.CategoryFid,
								 proC.CategoryName
							 }).Where(p => p.BFid == member.Fid).OrderBy(b => b.CategoryFid);
				if (keyword != null)
					datas = datas.Where(k => k.ProductName.Contains(keyword) || k.CategoryName.Contains(keyword)).OrderBy(o => o.CategoryName);

				List<CProductsViewModel> list = new List<CProductsViewModel>();
				foreach (var p in datas)
				{
					CProductsViewModel vm = new CProductsViewModel();
					vm.BFid = p.BFid;
					vm.Fid = p.Fid;
					vm.CategoryFid = p.CategoryFid;
					vm.UnitPrice = p.UnitPrice;
					vm.ProductName = p.ProductName;
					vm.IsForSale = p.IsForSale;
					vm.Memo = p.Memo;
					vm.Photo = p.Photo;
					vm.CategoryName = p.CategoryName;
					list.Add(vm);
				}
				return Json(list);
			}
			return RedirectToAction("Blogin", "BusinessMember");
		}
		public ActionResult BCreate()
		{
			string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
			BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
			CProductsViewModel vm = new CProductsViewModel();
			vm.BFid = member.Fid;

			return View(vm);
		}
		[HttpPost]
		public ActionResult BCreate(CProductsViewModel vm, IFormFile file,string CategoryName)
		{
			if (file != null)
			{
				string fileName = Guid.NewGuid().ToString()+".jpg";
				string uploadFile = Path.Combine(_host.WebRootPath, "images", fileName);
				using (var fileStream = new FileStream(uploadFile, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}
				vm.product.Photo = fileName;
			}
			var proCFid = _context.ProductCategories.FirstOrDefault(o => o.CategoryName == CategoryName);
			vm.product.CategoryFid = proCFid.Fid;
			vm.product.BFid = vm.BFid;
			_context.Products.Add(vm.product);
			_context.SaveChanges();
			return RedirectToAction("BList");
		}
		public ActionResult BCategoryList()
		{
			string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
			BusinessMember member = JsonSerializer.Deserialize<BusinessMember>(json);
			var proC = _context.ProductCategories.Where(o => o.BFid == member.Fid);
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
	}
}
