using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagement.ViewModels
{
    public class DamageRecordInfoIndexViewModel : BasePageViewModel
    {
        public string SearchName { get; set; }

        public int? SearchRecordState { get; set; }

        public bool IsAdmin { get; set; }
    }
}