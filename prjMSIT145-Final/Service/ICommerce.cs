using prjMSIT145_Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace prjMSIT145_Final.Service
{
	public interface ICommerce
	{
		string GetCallBack(SendToNewebPayIn inModel);
		//string GetPeriodCallBack(SendToNewebPayIn inModel);
		Result GetCallbackResult(IFormCollection form);
	}
}
