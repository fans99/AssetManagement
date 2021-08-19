using AssetManagement.CustomAuthorize;
using AssetManagement.Models;
using AssetManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetManagement.Controllers
{
    [Authorize]
    public class OfficeSuppliesRecordInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: OfficeSuppliesRecordInfo
        public ActionResult Index()
        {
            int empolyID = int.Parse(User.Identity.Name);
            OfficeSuppliesRecordInfoIndexViewModel model = new OfficeSuppliesRecordInfoIndexViewModel()
            {
                AssetInfoList = context.AssetInfo.Where(p => p.AssetTypeID == 2)
            };
            if (context.EmpolyInfo.Find(empolyID).RoleInfo.RoleID != 2)
                model.IsAdmin = true;
            else
                model.IsAdmin = false;

            return View(model);
        }

        public ActionResult GetOfficeSuppliesRecordInfos(OfficeSuppliesRecordInfoIndexViewModel model)
        {
            int empolyID = int.Parse(User.Identity.Name);
            IEnumerable<OfficeSuppliesRecordInfo> resultTemp;
            if (context.EmpolyInfo.Find(empolyID).RoleInfo.RoleID != 2)
            {
                resultTemp = context.OfficeSuppliesRecordInfo;
                model.IsAdmin = true;
            }
            else
            {            
                resultTemp = context.OfficeSuppliesRecordInfo.Where(p => p.OfficeApplylD == empolyID);
                model.IsAdmin = false;
            }

            if (!string.IsNullOrEmpty(model.SearchName))
                resultTemp = resultTemp.Where(p => context.AssetInfo.Find(p.AssetID).AssetName.Contains(model.SearchName));
            if (!string.IsNullOrEmpty(model.SearchEmpolyName))
                resultTemp = resultTemp.Where(p => context.EmpolyInfo.Find(p.OfficeApplylD).EmpolyName.Contains(model.SearchEmpolyName));
            if (model.SearchOfficeApplyState.HasValue && model.SearchOfficeApplyState >= 0)
                resultTemp = resultTemp.Where(p => p.OfficeApplyState == model.SearchOfficeApplyState.Value);

            var data = resultTemp.Select(p => new
            {
                p.OfficeID,
                p.AssetID,
                context.AssetInfo.Find(p.AssetID).AssetName,
                p.OfficeApplylD,
                context.EmpolyInfo.Find(p.OfficeApplylD).EmpolyName,
                OfficeApplyDate = p.OfficeApplyDate.ToString("yyyy-MM-dd"),
                p.OfficeApplyNum,
                p.OfficeApplyState,
                OfficeReceiveDate = p.OfficeReceiveDate.HasValue ? p.OfficeReceiveDate.Value.ToString("yyyy-MM-dd") : "",
                p.OfficeReceiveNum,
                p.OfficeRemark
            });

            model.RowCount = data.Count();
            data = model.OpenPaging(data, p => p.OfficeID);
            return Json(new
            {
                code = 0,
                count = model.RowCount,
                data
            });
        }

        [HttpGet]
        public ActionResult GetAssetCount(int id)
        {
            try
            {
                AssetInfo assetInfo = context.AssetInfo.Find(id);
                if (assetInfo == null)
                    return Json(new { code = 1 });

                int StorageCount = context.AssetPutInfo.Where(p => p.AssetID == assetInfo.AssetID).ToList().Sum(x => x.AssetPutCount);
                int UsedCount = context.OfficeSuppliesRecordInfo.Where(p => p.AssetID == assetInfo.AssetID && p.OfficeApplyState == 1).ToList().Sum(x => x.OfficeReceiveNum ?? 0);
                return Json(new
                {
                    code = 0,
                    count = StorageCount - UsedCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { code = 1 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Apply(OfficeSuppliesRecordInfo newOfficeSuppliesRecordInfo)
        {
            try
            {
                newOfficeSuppliesRecordInfo.OfficeApplylD = int.Parse(User.Identity.Name);
                newOfficeSuppliesRecordInfo.OfficeApplyState = 0;
                context.OfficeSuppliesRecordInfo.Add(newOfficeSuppliesRecordInfo);
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        [HttpPost]
        [AuthorizeRole(RoleID = "1")]
        public ActionResult Update(OfficeSuppliesRecordInfo officeSuppliesRecordInfo)
        {
            try
            {
                OfficeSuppliesRecordInfo updateOfficeSuppliesRecordInfo = context.OfficeSuppliesRecordInfo.Find(officeSuppliesRecordInfo.OfficeID);
                if (updateOfficeSuppliesRecordInfo == null)
                    return Json(new { code = 1 });

                updateOfficeSuppliesRecordInfo.OfficeReceiveDate = officeSuppliesRecordInfo.OfficeReceiveDate;
                updateOfficeSuppliesRecordInfo.OfficeReceiveNum = officeSuppliesRecordInfo.OfficeReceiveNum;
                updateOfficeSuppliesRecordInfo.OfficeApplyState = 1;
                context.Entry(updateOfficeSuppliesRecordInfo).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

    }
}