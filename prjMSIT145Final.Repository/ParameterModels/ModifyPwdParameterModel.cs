using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjMSIT145Final.Repository.ParameterModels
{
    public class ModifyPwdParameterModel
    {
        public int Fid { get; set; }
        public string Password { get; set; }

        public string ConfirmPwd { get; set; }
        public string Email { get; set; }


    }
}
