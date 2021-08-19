using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagement.ViewModels
{
    public class AssetPutInfoIndexViewModel : BasePageViewModel
    {
        public string SearchName { get; set; }

        public string SearchEmpolyName { get; set; }

        public int SearchAssetTypeID { get; set; }
    }
}