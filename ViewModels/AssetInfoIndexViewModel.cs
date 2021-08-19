using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class AssetInfoIndexViewModel : BasePageViewModel
    {
        public IEnumerable<AssetTypeInfo> AssetTypeInfoList { get; set; }

        public string SearchName { get; set; }

        public int SearchAssetTypeID { get; set; }
    }
}