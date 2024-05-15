using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Service.Dtos
{
    public class NormalMemberDto
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
        public int? ChatroomUserid { get; set; }
        public string? GoogleEmail { get; set; }
        public string? LineUserid { get; set; }

    }
}
