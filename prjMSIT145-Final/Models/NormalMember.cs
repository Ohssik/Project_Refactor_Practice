using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class NormalMember
    {
        public int Fid { get; set; }
        public string? MemberName { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Gender { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressArea { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Email { get; set; }
        public DateTime? RegisterTime { get; set; }
        public string? MemberPhotoFile { get; set; }
        public int? IsSuspensed { get; set; }
        public int? EmailCertified { get; set; }
    }
}
