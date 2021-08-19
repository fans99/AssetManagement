using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManagement.CustomAuthorize;
using AssetManagement.Models;
using AssetManagement.ViewModels;

namespace AssetManagement.Controllers
{
    [AuthorizeRole(RoleID = "1")]
    public class OfficeSuppliesInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: OfficeSuppliesInfo
        public ActionResult Index(OfficeSuppliesInfoIndexViewModel model)
        {
            IEnumerable<AssetInfo> resultTemp = context.AssetInfo.Where(p => p.AssetTypeID == 2);
            if (!string.IsNullOrEmpty(model.SearchName))
                resultTemp = resultTemp.Where(p => p.AssetName.Contains(model.SearchName));

            model.OfficeSuppliesInfoList = resultTemp.Select(p =>
            {
                var tempModel = new OfficeSuppliesInfoIndexViewModel.OfficeSuppliesInfo
                {
                    AssetID = p.AssetID,
                    AssetName = p.AssetName,
                    AssetPutCount = context.AssetPutInfo.Where(x => x.AssetID == p.AssetID).ToList().Sum(y => y.AssetPutCount),
                    UsedCount = context.OfficeSuppliesRecordInfo.Where(x => x.AssetID == p.AssetID && !x.OfficeReceiveDate.HasValue).ToList().Sum(y => y.OfficeReceiveNum ?? 0),
                };
                tempModel.RemainCount = tempModel.AssetPutCount ?? 0 - tempModel.UsedCount ?? 0;
                return tempModel;
            });

            model.RowCount = model.OfficeSuppliesInfoList.Count();
            model.OfficeSuppliesInfoList = model.OpenPaging(model.OfficeSuppliesInfoList, p => p.AssetID);
            return View(model);
        }
    }
}