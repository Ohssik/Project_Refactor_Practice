namespace prjMSIT145Final.Web.ViewModels
{
    public class SendEmailParameter
    {
        public int? MemberId { get;set; }
        public int? IsSuspensed { get;set; }
        public string? TxtRecipient { get; set; }
        public string? TxtMessage { get; set; }
        public string? MemberType { get; set; }
    }
}
