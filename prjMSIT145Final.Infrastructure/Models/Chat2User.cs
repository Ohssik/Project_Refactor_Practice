using System;
using System.Collections.Generic;

namespace prjMSIT145Final.Infrastructure.Models
{
    public partial class Chat2User
    {
        public int Fid { get; set; }
        public int Chatid { get; set; }
        public int Userid { get; set; }

        public virtual Chatroom Chat { get; set; } = null!;
        public virtual ChatroomUser User { get; set; } = null!;
    }
}
