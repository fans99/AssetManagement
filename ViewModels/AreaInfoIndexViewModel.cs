using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class AreaInfoIndexViewModel : BasePageViewModel
    {
        public IEnumerable<AreaTypeInfo> AreaTypeInfoList { get; set; }

        public string SearchName { get; set; }

        public int SearchAreaTypeID { get; set; }
    }
}