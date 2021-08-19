using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class AssetDetailInfoTotalViewModel
    {
        public IEnumerable<AssetDetailInfo> AssetDetailInfoList { get; set; }

        public IEnumerable<AreaInfo> AreaInfoList { get; set; }
    }
}