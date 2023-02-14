namespace prjMSIT145_Final.ViewModel
{
    public class CChatroomitemViewModel
    {
        public int chatroomid { get; set; }
        public int chatroomUserid { get; set; }
        public int Memberfid { get; set; }
        public string? MemberName { get; set; }
        public int? UserType { get; set; }
        public string? MemberImg { get; set; }
        public DateTime? LastOnlineTime { get; set; }
    }
}
