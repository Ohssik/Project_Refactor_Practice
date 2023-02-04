using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ChatMessage
    {
        public int Fid { get; set; }
        public int Chatid { get; set; }
        public int Senderid { get; set; }
        public string? Message { get; set; }
        public DateTime? SendTime { get; set; }

        public virtual Chatroom Chat { get; set; } = null!;
        public virtual ChatroomUser Sender { get; set; } = null!;
    }
}
