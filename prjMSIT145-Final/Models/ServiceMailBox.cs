using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ServiceMailBox
    {
        public int Fid { get; set; }
        public string? SenderName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Subject { get; set; }
        public string? Context { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public DateTime? ReadTime { get; set; }
        public string? Reply { get; set; }
    }
}
