﻿using System;
using System.Collections.Generic;

namespace prjMSIT145_Final.Models
{
    public partial class ProblemQuestion
    {
        public int Fid { get; set; }
        public string Question { get; set; } = null!;
        public int AnswerFid { get; set; }
    }
}
