using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Repository.ParameterModels
{
    public class SendEmailParameterModel
    {
        public int? MemberId { get; set; }
        public int? IsSuspensed { get; set; }
        public string? TxtRecipient { get; set; }
        public string? TxtMessage { get; set; }
        public string? MemberType { get; set; }

    }
}
