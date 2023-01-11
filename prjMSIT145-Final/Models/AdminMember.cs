using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class AdminMember
    {
        public int Fid { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
        public int? RoleLevel { get; set; }
    }
}
