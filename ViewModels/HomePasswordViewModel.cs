using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagement.ViewModels
{
    public class HomePasswordViewModel
    {
        public int EmpolyID { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}