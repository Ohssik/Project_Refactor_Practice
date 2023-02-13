using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace prjMSIT145_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayTermAPIController : ControllerBase
    {
        private IMemoryCache _cache;
        public PayTermAPIController(IMemoryCache cache)
        {
            _cache = cache;
        }
        /// <summary>
        /// 綠界回傳 付款資訊
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddPayInfo(JObject info)
        {
            try
            {
                //var cache = new MemoryCache(new MemoryCacheOptions());
                //var cache = MemoryCache.Default;
                _cache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
                return ResponseOK();
            }
            catch (Exception e)
            {
                return ResponseError();
            }
        }

        /// <summary>
        /// 綠界回傳 虛擬帳號資訊
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddAccountInfo(JObject info)
        {
            try
            {
                //var cache = new MemoryCache(new MemoryCacheOptions());
                //var cache = MemoryCache.Default;
                _cache.Set(info.Value<string>("MerchantTradeNo"), info, DateTime.Now.AddMinutes(60));
                return ResponseOK();
            }
            catch (Exception e)
            {
                return ResponseError();
            }
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
