using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManagement.Models;

namespace AssetManagement.ViewModels
{
    public class AssetDetailInfoDistributionViewModel
    {
        public int AssetDetailID { get; set; }

        public int EmpolyID { get; set; }

        public int AreaID { get; set; }

        public int AssetNum { get; set; }

        public DateTime AssetDetailRecordUseDate { get; set; }

        public string AssetDetailRecordReMark { get; set; }
    }
}