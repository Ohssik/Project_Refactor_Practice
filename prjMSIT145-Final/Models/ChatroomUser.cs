using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ChatroomUser
    {
        public ChatroomUser()
        {
            Chat2Users = new HashSet<Chat2User>();
            ChatMessages = new HashSet<ChatMessage>();
        }

        public int ChatroomUserid { get; set; }
        public int? UserType { get; set; }
        public int? Memberfid { get; set; }
        public string? ConnectionId { get; set; }
        public DateTime? LastOnlineTime { get; set; }

        public virtual ICollection<Chat2User> Chat2Users { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
