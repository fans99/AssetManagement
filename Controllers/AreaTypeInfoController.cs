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
    public class AreaTypeInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: AreaTypeInfo
        public ActionResult Index(AreaTypeInfoIndexViewModel model)
        {
            IEnumerable<AreaTypeInfo> result = context.AreaTypeInfo;
            if (!string.IsNullOrEmpty(model.SearchName))
                result = result.Where(p => p.AreaTypeName.Contains(model.SearchName));

            model.RowCount = result.Count();
            result = model.OpenPaging(result, p => p.AreaTypeID);
            model.AreaTypeInfoList = result;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AreaTypeInfo newAreaTypeInfo)
        {
            try
            {
                context.AreaTypeInfo.Add(newAreaTypeInfo);
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
                var areaTypeInfo = context.AreaTypeInfo.Find(id);
                if (areaTypeInfo == null)
                    return Json(new { code = 1 });
                context.AreaTypeInfo.Remove(areaTypeInfo);
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        [HttpPost]
        public ActionResult Update(AreaTypeInfo updateAreaTypeInfo)
        {
            try
            {
                context.Entry(updateAreaTypeInfo).State = System.Data.Entity.EntityState.Modified;
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