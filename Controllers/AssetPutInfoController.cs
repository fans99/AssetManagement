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
    public class AssetPutInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: AssetPutInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAssetPutInfos(AssetPutInfoIndexViewModel model)
        {
            IEnumerable<AssetPutInfo> resultTemp = context.AssetPutInfo;
            if (!string.IsNullOrEmpty(model.SearchName))
                resultTemp = resultTemp.Where(p => p.AssetInfo.AssetName.Contains(model.SearchName));
            if (model.SearchAssetTypeID > 0)
                resultTemp = resultTemp.Where(p => p.AssetInfo.AssetTypeID == model.SearchAssetTypeID);
            if (!string.IsNullOrEmpty(model.SearchEmpolyName)) {
                resultTemp = resultTemp.Where(p =>
                {
                    var emp = context.EmpolyInfo.Find(p.EmpolyID);
                    if (emp != null && emp.EmpolyName.Contains(model.SearchEmpolyName))
                        return true;
                    else
                        return false;
                });
            }

            var data = resultTemp.Select(p => new
            {
                p.AssetPutID,
                p.AssetID,
                p.AssetInfo.AssetName,
                p.AssetInfo.AssetTypeID,
                p.AssetInfo.AssetTypeInfo.AssetTypeName,
                p.AssetPutCount,
                p.EmpolyID,
                context.EmpolyInfo.Find(p.EmpolyID).EmpolyName,
                AssetPutDate = p.AssetPutDate.ToString("yyyy-MM-dd"),
                p.AssetPutReMark
            });
            model.RowCount = data.Count();
            data = model.OpenPaging(data, p => p.AssetPutID);
            return Json(new
            {
                code = 0,
                count = model.RowCount,
                data
            });
        }

        public ActionResult Create(AssetPutInfo newAssetPutInfo)
        {
            try
            {
                newAssetPutInfo.EmpolyID = int.Parse(User.Identity.Name);
                context.AssetPutInfo.Add(newAssetPutInfo);
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch(Exception err)
            {
                Console.WriteLine(err);
                return Json(new { code = 1 });
            }
        }

        [HttpPost]
        public ActionResult Update(AssetPutInfo updateAssetPutInfo)
        {
            try
            {
                AssetPutInfo assetPutInfo = context.AssetPutInfo.Find(updateAssetPutInfo.AssetPutID);
                if (assetPutInfo == null)
                    return Json(new { code = 1 });
                assetPutInfo.AssetPutCount = updateAssetPutInfo.AssetPutCount;
                assetPutInfo.AssetPutDate = updateAssetPutInfo.AssetPutDate;
                assetPutInfo.AssetPutReMark = updateAssetPutInfo.AssetPutReMark;
                context.Entry(assetPutInfo).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        //[HttpPost]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        AssetPutInfo assetPutInfo = context.AssetPutInfo.Find(id);
        //        if (assetPutInfo == null)
        //            return Json(new { code = 1 });

        //        context.AssetPutInfo.Remove(assetPutInfo);
        //        context.SaveChanges();
        //        return Json(new { code = 0 });
        //    }
        //    catch
        //    {
        //        return Json(new { code = 1 });
        //    }
        //}
    }
}