﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly IWebHostEnvironment _host;
		

        public CreditPayController(ILogger<HomeController> logger, IWebHostEnvironment host)
		{
			_logger = logger;
			Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
			_host = host;
            
        }
        string url;
        private string getUrl()
		{
			string urlStr;
            //urlStr = HttpContext.Request.Scheme;
            urlStr = "https";
            urlStr += "://";
            urlStr += HttpContext.Request.Host;
            urlStr += HttpContext.Request.PathBase;
			return urlStr; 
        }

		//取得選擇的類別
		private ICommerce GetPayType(string option)
		{
            url= getUrl();
            switch (option)
			{
				case "ECPay":
					return new ECPayService(url);

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

        [HttpPost]
        public async Task<IActionResult> GetReturn(SendToNewebPayIn inModel)
        {
            url = getUrl();
            var obj = await new ECPayService(url).GetQueryCallBack(inModel.MerchantOrderNo, inModel.Amt);
            return Json(obj);
        }

        /// <summary>
        /// 支付完成返回網址
        /// </summary>
        /// <returns></returns>string option
        public IActionResult CallbackReturn()
		{
			return RedirectToAction("List", "OrderDetial");            
        }

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
