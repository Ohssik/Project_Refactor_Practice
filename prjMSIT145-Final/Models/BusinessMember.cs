using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prjMSIT145_Final.Models
{
    public partial class BusinessMember
    {
        public int Fid { get; set; }
        [Display(Name ="會員名稱")]
        public string? MemberName { get; set; }
        [Display(Name = "品牌名稱")]
        public string? Brand { get; set; }
        [Display(Name = "電話")]
        public string? Phone { get; set; }
        [Display(Name = "密碼")]
        public string? Password { get; set; }
        [Display(Name = "開店時間")]
        public TimeSpan? OpenTime { get; set; }
        [Display(Name = "閉店時間")]
        public TimeSpan? CloseTime { get; set; }
        [Display(Name = "地址")]
        public string? Address { get; set; }
        [Display(Name = "類別")]
        public string? ShopType { get; set; }
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Display(Name = "註冊時間")]
        public DateTime? RegisterTime { get; set; }
        [Display(Name = "負責人")]
        public string? ContactPerson { get; set; }
        [Display(Name = "帳號")]
        public string? MemberAccount { get; set; }
        public int? IsSuspensed { get; set; }
        public int? EmailCertified { get; set; }
        public string? Gps { get; set; }
        public int? IsOpened { get; set; }
    }
}
