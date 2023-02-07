using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prjMSIT145_Final.Models
{
    public partial class BusinessMember
    {
        public int Fid { get; set; }
        [Required]
        public string? MemberName { get; set; }
        [Required]
        public string? Brand { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public TimeSpan? OpenTime { get; set; }
        [Required]
        public TimeSpan? CloseTime { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? ShopType { get; set; }
        [Required]
        public string? Email { get; set; }
        public DateTime? RegisterTime { get; set; }
        [Required]
        public string? ContactPerson { get; set; }
        public string? MemberAccount { get; set; }
        public int? IsSuspensed { get; set; }
        public int? EmailCertified { get; set; }
        public string? Gps { get; set; }
        public int? IsOpened { get; set; }
        public int? ChatroomUserid { get; set; }
    }
}
