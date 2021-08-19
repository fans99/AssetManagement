using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManagement.Models;
using AssetManagement.ViewModels;

namespace AssetManagement.Controllers
{
    [Authorize] 
    public class AssetDetailRecordInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: AssetDetailRecordInfo
        public ActionResult Index(AssetDetailRecordInfoIndexViewModel model)
        {
            int empolyID = int.Parse(User.Identity.Name);
            IEnumerable<AssetDetailRecordInfo> result;
            if (context.EmpolyInfo.Find(empolyID).RoleInfo.RoleID == 2)
            {
                result = context.AssetDetailRecordInfo.Where(p => p.EmpolyID == empolyID);
                model.IsAdmin = false;
            }
            else
            {
                result = context.AssetDetailRecordInfo;
                model.IsAdmin = true;
            }

            if (!string.IsNullOrEmpty(model.SearchAssetDetailNum))
                result = result.Where(p => context.AssetDetailInfo.Find(p.AssetDetailID).AssetDetailNum.Contains(model.SearchAssetDetailNum));
            model.RowCount = result.Count();
            result = model.OpenPaging(result, p => p.AssetDetailRecordID);
            model.AssetDetailRecordInfoList = result;
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(int id)
        {
            try
            {
                AssetDetailRecordInfo assetDetailRecordInfo = context.AssetDetailRecordInfo.Find(id);
                if (assetDetailRecordInfo == null)
                    return Json(new { code = 1 });

                assetDetailRecordInfo.AssetDetailRecordReturnDate = DateTime.Now;
                context.Entry(assetDetailRecordInfo).State = System.Data.Entity.EntityState.Modified;

                AssetDetailInfo assetDetailInfo = context.AssetDetailInfo.Find(assetDetailRecordInfo.AssetDetailID);
                assetDetailInfo.AssetDetailReturnDate = assetDetailRecordInfo.AssetDetailRecordReturnDate;
                context.Entry(assetDetailInfo).State = System.Data.Entity.EntityState.Modified;
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