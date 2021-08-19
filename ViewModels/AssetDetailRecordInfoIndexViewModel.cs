using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class AssetDetailRecordInfoIndexViewModel : BasePageViewModel
    {
        public IEnumerable<AssetDetailRecordInfo> AssetDetailRecordInfoList { get; set; }

        public string SearchAssetDetailNum { get; set; }

        public bool IsAdmin { get; set; }
    }
}