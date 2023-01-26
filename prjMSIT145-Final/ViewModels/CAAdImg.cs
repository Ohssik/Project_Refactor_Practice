using prjMSIT145_Final.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace prjMSIT145_Final.ViewModels
{
    public class CAAdImg
    {
        private AdImg _adImg;
        
        public CAAdImg()
        {
            _adImg = new AdImg();
            
        }
        public AdImg adImg
        {
            get { return _adImg; }
            set { _adImg = value; }
        }
        [DisplayName("ID")]
        public int Fid { get {return _adImg.Fid; } set {_adImg.Fid=value; } }
        public string sFid { get; set; }
            
        [DisplayName("檔案名稱")]
        public string? ImgName { get { return _adImg.ImgName; } set { _adImg.ImgName=value; } }
        [DisplayName("開始投放時間")]
        public string? st
        {
            get
            {
                string st = "";
                if (_adImg.StartTime!=null)
                    st=Convert.ToDateTime(_adImg.StartTime).ToString("yyyy-MM-dd");
                return st;
            }
            
        }
        public DateTime? StartTime { 
            get {
                
                return _adImg.StartTime; 
            } 
            set { 
                _adImg.StartTime=value; 
            } 
        }
        [DisplayName("結束投放時間")]
        public string? et
        {
            get
            {
                string et = "";
                if (_adImg.EndTime!=null)
                    et=Convert.ToDateTime(_adImg.EndTime).ToString("yyyy-MM-dd");
                return et;
            }

        }
        public DateTime? EndTime { get { return _adImg.EndTime; } set { _adImg.EndTime=value; } }
        [DisplayName("圖片連結")]
        public string? Hyperlink { get { return _adImg.Hyperlink; } set { _adImg.Hyperlink=value; } }
        
        public int? BFid { get { return _adImg.BFid; } set { _adImg.BFid=value; } }
        [DisplayName("投放主")]
        public string? ImgBelongTo { get ; set; }
        public int? OrderBy { get { return _adImg.OrderBy; } set { _adImg.OrderBy=value; } }

        //[JsonConverter(typeof(JsonToByteArrayConverter))]
        public byte[]? icon { get; set; }
        public string? fileType { get; set; }

        //internal sealed class JsonToByteArrayConverter : JsonConverter<byte[]?>
        //{
        //    // Converts base64 encoded string to byte[].
        //    public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //    {
        //        if (!reader.TryGetBytesFromBase64(out byte[]? result) || result == default)
        //        {
        //            throw new Exception("Add your fancy exception message here...");
        //        }
        //        return result;
        //    }

        //    // Converts byte[] to base64 encoded string.
        //    public override void Write(Utf8JsonWriter writer, byte[]? value, JsonSerializerOptions options)
        //    {
        //        writer.WriteBase64StringValue(value);
        //    }
        //}
    }
}
