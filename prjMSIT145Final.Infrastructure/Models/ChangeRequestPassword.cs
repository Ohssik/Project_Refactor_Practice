using System;
using System.Collections.Generic;

namespace prjMSIT145Final.Infrastructure.Models
{
    public partial class ChangeRequestPassword
    {
        public string Token { get; set; } = null!;
        public string? Account { get; set; }
        public string? Email { get; set; }
        public DateTime? Expire { get; set; }
    }
}
