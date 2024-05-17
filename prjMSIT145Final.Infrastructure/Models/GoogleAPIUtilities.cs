using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;

namespace prjMSIT145Final.Infrastructure.Models
{
    public class GoogleAPIUtilities
    {
        public static string ConvertAddressToJsonString(string address,string GoogleAPIKey)
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + HttpUtility.UrlEncode(address, Encoding.UTF8) + "&key=" + GoogleAPIKey;
            string result = ""; 
            using (WebClient client = new WebClient())
            {
                //指定語言，否則Google預設回傳英文   
                client.Headers[HttpRequestHeader.AcceptLanguage] = "zh-TW";
                //不設定的話，會回傳中文亂碼
                client.Encoding = Encoding.UTF8;
                #region POST method才會用到
                //client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                //byte[] response = client.UploadValues("https://maps.googleapis.com/maps/api/geocode/json", new NameValueCollection()
                //{
                //        { "address", HttpUtility.UrlEncode(address, Encoding.UTF8)},
                //        { "key", GoogleAPIKey }
                //});
                //result = Encoding.UTF8.GetString(response);
                #endregion
                result = client.DownloadString(url);
            }
            return result;
        }

        public static double[] ConvertJsonTolanAndlng(string json)
        {
            //將Json字串轉成物件  
            GoogleGeocodingAPI.RootObject rootObj = JsonConvert.DeserializeObject<GoogleGeocodingAPI.RootObject>(json);
            double[] latLng = new double[2];
            if (rootObj.status == "OK")
            {
                double lat = rootObj.results[0].geometry.location.lat;//緯度  
                double lng = rootObj.results[0].geometry.location.lng;//經度   
                latLng[0] = lat;
                latLng[1] = lng;
            }
            return latLng;
        }
    }        
}
