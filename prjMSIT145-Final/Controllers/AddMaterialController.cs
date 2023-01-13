using Microsoft.AspNetCore.Mvc;
using prjMSIT145_Final.Models;
using System.Text.Json;

namespace prjMSIT145_Final.Controllers
{
    public class AddMaterialController : Controller
    {
        ispanMsit145shibaContext _context;
        public AddMaterialController(ispanMsit145shibaContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            return View();
        }
        public ActionResult Search(string keyword)
        {
            string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_BUSINESS_USER);
            CBusinessLoginVIewModel login = JsonSerializer.Deserialize<CBusinessLoginVIewModel>(json);

            var datas = _context.ViewOptionsToGroups.Where(b => b.BFid == login.Business_fId);
            if (keyword != null)
                datas = datas.Where(k => k.OptionName.Contains(keyword) || k.OptionGroupName.Contains(keyword));

            List<CAddMaterialViewModel> materialList = new List<CAddMaterialViewModel>();
            foreach (var data in datas)
            {
                CAddMaterialViewModel vm = new CAddMaterialViewModel();
                vm.Fid = data.Fid;
                vm.BFid = data.BFid;
                vm.OptionGroupFid = data.OptionGroupFid;
                vm.UnitPrice = data.UnitPrice;
                vm.OptionGroupName = data.OptionGroupName;
                vm.OptionName = data.OptionName;
                materialList.Add(vm);
            }
            return Json(materialList);
        }
    }
}
