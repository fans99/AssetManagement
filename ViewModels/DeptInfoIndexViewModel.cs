using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class DeptInfoIndexViewModel : BasePageViewModel
    {
        public IEnumerable<DeptInfo> DeptInfoList { get; set; }

        public IEnumerable<CampusInfo> CampusInfoList { get; set; }

        public string SearchName { get; set; }

        public int SearchCampusID { get; set; }
    }
}