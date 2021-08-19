using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using AssetManagement.Models;
using AssetManagement.ViewModels;
using System.Data.Entity;
using AssetManagement.CustomAuthorize;

namespace AssetManagement.Controllers
{
    [Authorize]
    public class DamageRecordInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: DamageRecordInfo
        public ActionResult Index()
        {
            int empolyID = int.Parse(User.Identity.Name);
            DamageRecordInfoIndexViewModel model = new DamageRecordInfoIndexViewModel();
            EmpolyInfo empolyInfo = context.EmpolyInfo.Find(empolyID);
            if (empolyInfo.RoleID != 2)
                model.IsAdmin = true;
            else
                model.IsAdmin = false;

            return View(model);
        }

        [HttpPost]
        public ActionResult GetDamageRecordInfos(DamageRecordInfoIndexViewModel model)
        {
            int empolyID = int.Parse(User.Identity.Name);
            EmpolyInfo empolyInfo = context.EmpolyInfo.Find(empolyID);
            IEnumerable<DamageRecordInfo> resultTemp;
            if (empolyInfo.RoleID != 2)
                resultTemp = context.DamageRecordInfo;
            else
                resultTemp = context.DamageRecordInfo.Where(p => p.EmpolyID == empolyInfo.EmpolyID);

            if (!string.IsNullOrEmpty(model.SearchName))
                resultTemp = resultTemp.Where(p => context.AssetDetailInfo.Find(p.AssetDetailID).AssetInfo.AssetName.Contains(model.SearchName));
            if (model.SearchRecordState.HasValue && model.SearchRecordState >= 0)
                resultTemp = resultTemp.Where(p => p.RecordState == model.SearchRecordState);

            var data = resultTemp.Select(p => new
            {
                p.DamageRecordlID,
                p.AssetDetailID,
                context.AssetDetailInfo.Find(p.AssetDetailID).AssetInfo.AssetName,
                p.EmpolyID,
                context.EmpolyInfo.Find(p.EmpolyID).EmpolyName,
                DamageDate = p.DamageDate.ToString("yyyy-MM-dd"),
                p.DamageCauses,
                p.ProblemDescription,
                p.Problemlmange,
                p.RecordState,
                p.Repairman,
                RepairDates = p.RepairDates.HasValue ? p.RepairDates.Value.ToString("yyyy-MM-dd") : "",
                p.DamageRecordReMark
            });
            model.RowCount = data.Count();
            data = model.OpenPaging(data, p => p.DamageRecordlID);
            return Json(new
            {
                code = 0,
                count = model.RowCount,
                data
            });
        }

        [HttpPost]
        public ActionResult UploadImg()
        {
            try
            {
                HttpPostedFileBase postFile = Request.Files["ImgFile"];
                string[] t = postFile.FileName.Split('.');
                string suffix = t[t.Length - 1];
                string customName = postFile.FileName.Substring(0, postFile.FileName.Length - suffix.Length) + Guid.NewGuid().ToString().Replace("-", "");
                postFile.SaveAs(Server.MapPath("~/Content/upload/damage/") + customName + "." + suffix);
                return Json(new {
                    code = 0,
                    result = customName + "." + suffix
                });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        [HttpPost]
        public ActionResult Create(DamageRecordInfo newDamageRecordInfo)
        {
            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    newDamageRecordInfo.EmpolyID = int.Parse(User.Identity.Name);
                    newDamageRecordInfo.RecordState = 0;
                    context.DamageRecordInfo.Add(newDamageRecordInfo);
                    context.SaveChanges();

                    AssetDetailInfo assetDetailInfo = context.AssetDetailInfo.Find(newDamageRecordInfo.AssetDetailID);
                    assetDetailInfo.AssetDetailServiceState = 1;
                    context.Entry(assetDetailInfo).State = EntityState.Modified;
                    context.SaveChanges();
                    transaction.Commit();
                    return Json(new { code = 0 });
                }
                catch
                {
                    transaction.Rollback();
                    return Json(new { code = 1 });
                }
            }           
        }

        [HttpPost]
        [AuthorizeRole(RoleID = "1")]
        public ActionResult Repair(int id)
        {
            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    int empolyID = int.Parse(User.Identity.Name);
                    DamageRecordInfo damageRecordInfo = context.DamageRecordInfo.Find(id);
                    damageRecordInfo.RecordState = 1;
                    damageRecordInfo.Repairman = context.EmpolyInfo.Find(empolyID).EmpolyName;
                    damageRecordInfo.RepairDates = DateTime.Now;
                    context.Entry(damageRecordInfo).State = EntityState.Modified;
                    context.SaveChanges();

                    AssetDetailInfo assetDetailInfo = context.AssetDetailInfo.Find(damageRecordInfo.AssetDetailID);
                    assetDetailInfo.AssetDetailServiceState = 0;
                    context.Entry(assetDetailInfo).State = EntityState.Modified;
                    context.SaveChanges();
                    transaction.Commit();
                    return Json(new { code = 0 });
                }
                catch
                {
                    return Json(new { code = 1 });
                }
            }
        }

        [HttpGet]
        public ActionResult GetUsedAssetInfos()
        {
            int empolyID = int.Parse(User.Identity.Name);
            string sql = @"select AssetDetailInfo.AssetID, AssetDetailID, AssetName from AssetDetailInfo 
            left join AssetInfo on AssetDetailInfo.AssetID = AssetInfo.AssetID
            where AssetDetailID in (select AssetDetailID from AssetDetailRecordInfo where EmpolyID = @EmpolyID and AssetDetailRecordReturnDate is NULL)";
            IEnumerable<UsedAssetInfo> result = context.Database.SqlQuery<UsedAssetInfo>(sql, new SqlParameter("@EmpolyID", empolyID));
            return Json(new
            {
                code = 0,
                data = result
            }, JsonRequestBehavior.AllowGet);
        }

        private class UsedAssetInfo
        {
            public int AssetID { get; set; }

            public int AssetDetailID { get; set; }

            public string AssetName { get; set; }
        }

    }
}