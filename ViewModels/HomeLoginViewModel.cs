using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagement.ViewModels
{
    public class HomeLoginViewModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool RememberPwd { get; set; }
    }
}