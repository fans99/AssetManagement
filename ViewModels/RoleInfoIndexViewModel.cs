using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class RoleInfoIndexViewModel : BasePageViewModel
    {
        public IEnumerable<RoleInfo> RoleInfoList { get; set; }

        public string SearchName { get; set; }
    }
}