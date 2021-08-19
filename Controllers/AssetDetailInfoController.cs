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
    public class AssetDetailInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: AssetDetailInfo
        public ActionResult Index()
        {
            AssetDetailInfoIndexViewModel model = new AssetDetailInfoIndexViewModel()
            {
                AssetDetailInfoList = context.AssetDetailInfo.Where(p => p.AssetInfo.AssetTypeID == 1),
                EmpolyInfoList = context.EmpolyInfo,
                AreaInfoList = context.AreaInfo
            };
            return View(model);
        }

        public ActionResult Total()
        {
            AssetDetailInfoTotalViewModel model = new AssetDetailInfoTotalViewModel()
            {
                AssetDetailInfoList = context.AssetDetailInfo,
                AreaInfoList = context.AreaInfo
            };
            return View(model);
        }

        public ActionResult GetAssetDetailInfos(AssetDetailInfoIndexViewModel model)
        {
            IEnumerable<AssetDetailInfo> resultTemp = context.AssetDetailInfo;
            if (!string.IsNullOrEmpty(model.SearchName))
                resultTemp = resultTemp.Where(p => p.AssetInfo.AssetName.Contains(model.SearchName));
            if (model.SearchAssetDetailUseState.HasValue && model.SearchAssetDetailUseState.Value >= 0)
                resultTemp = resultTemp.Where(p => p.AssetDetailUseState == model.SearchAssetDetailUseState);

            var data = resultTemp.Select(p => new
            {
                p.AssetDetailID,
                p.AssetID,
                p.AssetInfo.AssetName,
                p.AssetDetailNum,
                p.AssetDetailUseState,
                AssetDetailUseDate = p.AssetDetailUseDate.HasValue ? p.AssetDetailUseDate.Value.ToString("yyyy-MM-dd") : "",
                AssetDetailReturnDate = p.AssetDetailReturnDate.HasValue ? p.AssetDetailReturnDate.Value.ToString("yyyy-MM-dd") : "",
                p.AssetDetailServiceState,
                p.AssetDetailDumState,
                p.EmpolyID,
                EmpolyName = p.EmpolyID > 0 ? context.EmpolyInfo.Find(p.EmpolyID).EmpolyName : "仓库",
                p.AreaID,
                AreaName = p.AreaID > 0 ? context.AreaInfo.Find(p.AreaID).AreaName : "仓库",
                p.AssetAreaReMark
            });
            model.RowCount = data.Count();
            data = model.OpenPaging(data, p => p.AssetDetailID);
            return Json(new
            {
                code = 0,
                count = model.RowCount,
                data
            });
        }

        [HttpPost]
        public ActionResult Distribution(AssetDetailInfoDistributionViewModel model)
        {
            try
            {
                AssetDetailInfo assetDetailInfo = context.AssetDetailInfo.Find(model.AssetDetailID);
                if (assetDetailInfo == null)
                    return Json(new { code = 1 });

                int StorageCount = context.AssetPutInfo.Where(p => p.AssetID == assetDetailInfo.AssetID).ToList().Sum(p => p.AssetPutCount);
                int UsedCount = context.AssetDetailRecordInfo.Where(p => p.AssetDetailID == model.AssetDetailID && !p.AssetDetailRecordReturnDate.HasValue).ToList().Sum(p => p.AssetNum);

                if (StorageCount - UsedCount < model.AssetNum)
                    return Json(new { code = 2, remain = StorageCount - UsedCount });

                assetDetailInfo.AssetDetailUseState = 0;
                assetDetailInfo.AssetDetailUseDate = model.AssetDetailRecordUseDate;
                assetDetailInfo.EmpolyID = model.EmpolyID;
                assetDetailInfo.AreaID = model.AreaID;
                context.Entry(assetDetailInfo).State = System.Data.Entity.EntityState.Modified;

                AssetDetailRecordInfo newAssetDetailRecordInfo = new AssetDetailRecordInfo()
                {
                    AssetDetailID = model.AssetDetailID,
                    EmpolyID = model.EmpolyID,
                    AreaID = model.AreaID,
                    AssetNum = model.AssetNum,
                    AssetDetailRecordUseDate = model.AssetDetailRecordUseDate,
                    AssetDetailRecordReMark = model.AssetDetailRecordReMark
                };
                context.AssetDetailRecordInfo.Add(newAssetDetailRecordInfo);
                context.SaveChanges();
                return Json(new { code = 0, remain = StorageCount - UsedCount - model.AssetNum });
            }
            catch(Exception err)
            {
                Console.WriteLine(err);
                return Json(new { code = 1 });
            }
        }
    }
}