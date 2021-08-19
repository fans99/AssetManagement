using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class EmpolyInfoIndexViewModel : BasePageViewModel
    {
        public IEnumerable<DeptInfo> DeptInfoList { get; set; }

        public IEnumerable<RoleInfo> RoleInfoList { get; set; }

        public string SearchName { get; set; }

        public int SearchDeptID { get; set; }

        public int SearchRoleID { get; set; }

        public string SearchSex { get; set; }

    }
}