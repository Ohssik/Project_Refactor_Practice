﻿using prjMSIT145_Final.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DisplayName("檔案名稱")]
        public string? ImgName { get { return _adImg.ImgName; } set { _adImg.ImgName=value; } }
        [DisplayName("開始投放時間")]
        public DateTime? StartTime { get { return _adImg.StartTime; } set { _adImg.StartTime=value; } }
        [DisplayName("結束投放時間")]
        public DateTime? EndTime { get { return _adImg.EndTime; } set { _adImg.EndTime=value; } }
        [DisplayName("圖片連結")]
        public string? Hyperlink { get { return _adImg.Hyperlink; } set { _adImg.Hyperlink=value; } }
        
        public int? BFid { get { return _adImg.BFid; } set { _adImg.BFid=value; } }
        [DisplayName("投放主")]
        public string? ImgBelongTo { get ; set; }
        public int? OrderBy { get { return _adImg.OrderBy; } set { _adImg.OrderBy=value; } }
    }
}
