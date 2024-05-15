using System;
using System.Collections.Generic;

namespace prjMSIT145Final.Infrastructure.Models
{
    public partial class Chatroom
    {
        public Chatroom()
        {
            Chat2Users = new HashSet<Chat2User>();
            ChatMessages = new HashSet<ChatMessage>();
        }

        public int Chatid { get; set; }
        public string? ChatName { get; set; }

        public virtual ICollection<Chat2User> Chat2Users { get; set; }
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
