using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class AssetDetailInfoIndexViewModel : BasePageViewModel
    {
        public string SearchName { get; set; }

        public int? SearchAssetDetailUseState { get; set; }

        public IEnumerable<AssetDetailInfo> AssetDetailInfoList { get; set; }

        public IEnumerable<EmpolyInfo> EmpolyInfoList { get; set; }

        public IEnumerable<AreaInfo> AreaInfoList { get; set; }
    }
}