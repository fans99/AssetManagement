using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class OfficeSuppliesRecordInfoIndexViewModel : BasePageViewModel
    {
        public string SearchName { get; set; }

        public string SearchEmpolyName { get; set; }

        public int? SearchOfficeApplyState { get; set; }

        public bool IsAdmin { get; set; }

        public IEnumerable<AssetInfo> AssetInfoList { get; set; }
    }
}