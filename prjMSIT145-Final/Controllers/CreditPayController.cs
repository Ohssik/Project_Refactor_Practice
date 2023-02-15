using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using prjMSIT145_Final.Models;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using prjMSIT145_Final.Service;

namespace prjMSIT145_Final.Controllers
{
	public class CreditPayController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration Config;

		public CreditPayController(ILogger<HomeController> logger)
		{
			_logger = logger;
			Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
		}
		//取得選擇的類別
		private ICommerce GetPayType(string option)
		{
			switch (option)
			{
				case "ECPay":
					return new ECPayService();

				default: throw new ArgumentException("No Such option");
			}
		}
		//付款鈕=>SendToNewebPay=>到這=>service.GetCallBack(inModel)
		private string GetReturnValue(ICommerce service, SendToNewebPayIn inModel)
		{
			switch (inModel.PayOption)
			{
				case "ECPay":
					return service.GetCallBack(inModel);

				default: throw new ArgumentException("No Such option");
			}
		}

		public IActionResult Index() //專題付款最後一頁
		{
			ViewData["MerchantOrderNo"] = DateTime.Now.ToString("yyyyMMddHHmmss");  //訂單編號
			ViewData["ExpireDate"] = DateTime.Now.AddDays(3).ToString("yyyyMMdd"); //繳費有效期限       
			return View();
		}

		//按下付款鍵
		public IActionResult SendToNewebPay(SendToNewebPayIn inModel)
		{
			var service = GetPayType(inModel.PayOption);

			return Json(GetReturnValue(service, inModel));
		}

		/// <summary>
		/// 支付完成返回網址
		/// </summary>
		/// <returns></returns>string option
		public IActionResult CallbackReturn()
		{
			//var service = GetPayType(option);
			//var result = service.GetCallbackResult(Request.Form);
			//ViewData["ReceiveObj"] = result.ReceiveObj;
			//ViewData["TradeInfo"] = result.TradeInfo;

			//return RedirectToAction("")  返回歷史訂單明細OrderDetial/List
			return RedirectToAction("List", "OrderDetial");
		}

		//沒用到
		/// <summary>
		/// 支付通知網址
		/// </summary>
		/// <returns></returns>  原bank
		public HttpResponseMessage CallbackNotify(string option)
		{
			var service = GetPayType(option);
			var result = service.GetCallbackResult(Request.Form);

			//TODO 支付成功後 可做後續訂單處理

			return ResponseOK();
		}


		/// <summary>
		/// 回傳給 綠界 失敗
		/// </summary>
		/// <returns></returns>
		private HttpResponseMessage ResponseError()
		{
			var response = new HttpResponseMessage();
			response.Content = new StringContent("0|Error");
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
			return response;
		}

		/// <summary>
		/// 回傳給 綠界 成功
		/// </summary>
		/// <returns></returns>
		private HttpResponseMessage ResponseOK()
		{
			var response = new HttpResponseMessage();
			response.Content = new StringContent("1|OK");
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
			return response;
		}
	}
}
