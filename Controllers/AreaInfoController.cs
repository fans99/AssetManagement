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
    public class AreaInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: AreaInfo
        public ActionResult Index(AreaInfoIndexViewModel model)
        {
            model.AreaTypeInfoList = context.AreaTypeInfo;
            return View(model);
        }

        public ActionResult GetAreaInfos(AreaInfoIndexViewModel model)
        {
            IEnumerable<AreaInfo> resultTemp = context.AreaInfo;
            if (!string.IsNullOrEmpty(model.SearchName))
                resultTemp = resultTemp.Where(p => p.AreaName.Contains(model.SearchName));
            if (model.SearchAreaTypeID > 0)
                resultTemp = resultTemp.Where(p => p.AreaTypeID == model.SearchAreaTypeID);

            var data = resultTemp.Select(p => new
            {
                p.AreaID,
                p.AreaName,
                p.AreaReMark,
                p.AreaTypeID,
                p.AreaTypeInfo.AreaTypeName
            });

            model.RowCount = data.Count();
            data = model.OpenPaging(data, p => p.AreaID);
            return Json(new
            {
                code = 0,
                count = model.RowCount,
                data
            });
        }

        public ActionResult Create(AreaInfo newAreaInfo)
        {
            try
            {
                context.AreaInfo.Add(newAreaInfo);
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        [HttpPost]
        public ActionResult Update(AreaInfo updateAreaInfo)
        {
            try
            {
                context.Entry(updateAreaInfo).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                AreaInfo areaInfo = context.AreaInfo.Find(id);
                if (areaInfo == null)
                    return Json(new { code = 1 });

                context.AreaInfo.Remove(areaInfo);
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