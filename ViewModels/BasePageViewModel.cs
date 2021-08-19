using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagement.ViewModels
{
    public class BasePageViewModel
    {
        public int RowCount { get; set; }

        public int? Page { get; set; }

        public int? Limit { get; set; }
    }
}