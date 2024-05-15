using prjMSIT145Final.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace prjMSIT145Final.Web.Service
{
	public interface ICommerce
	{
		string GetCallBack(SendToNewebPayIn inModel);
		//string GetPeriodCallBack(SendToNewebPayIn inModel);
		Result GetCallbackResult(IFormCollection form);
	}
}
