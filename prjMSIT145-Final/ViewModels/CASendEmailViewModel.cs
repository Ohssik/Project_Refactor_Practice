namespace prjMSIT145_Final.ViewModels
{
    public class CASendEmailViewModel
    {
        public int? memberId { get;set; }
        public int? IsSuspensed { get;set; }
        public string? txtRecipient { get; set; }
        public string? txtMessage { get; set; }
        public string? memberType { get; set; }
    }
}
