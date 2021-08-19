using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class OfficeSuppliesInfoIndexViewModel : BasePageViewModel
    {
        public string SearchName { get; set; }

        public IEnumerable<OfficeSuppliesInfo> OfficeSuppliesInfoList { get; set; }

        public class OfficeSuppliesInfo
        {
            public int AssetID { get; set; }

            public string AssetName { get; set; }

            public int? AssetPutCount { get; set; }

            public int? UsedCount { get; set; }

            public int RemainCount { get; set; }
        }
    }
}