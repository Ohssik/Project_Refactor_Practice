using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Service.Dtos
{
    public class AdminMemberDto
    {
        public int Fid { get; set; }
        public string? Account { get; set; }
        public string? Password { get; set; }
        public int? RoleLevel { get; set; }
        public string? Email { get; set; }
        public int? ChatroomUserid { get; set; }
    }
}
