using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class CampusInfoIndexViewModel : BasePageViewModel
    {
        public IEnumerable<CampusInfo> CampusInfoList { get; set; }

        public string SearchName { get; set; }
    }
}