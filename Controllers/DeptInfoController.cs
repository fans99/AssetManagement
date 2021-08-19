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
    public class DeptInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: DeptInfo
        public ActionResult Index(DeptInfoIndexViewModel model)
        {
            IEnumerable<DeptInfo> result = context.DeptInfo;
            if (!string.IsNullOrEmpty(model.SearchName))
                result = result.Where(p => p.DeptName.Contains(model.SearchName));

            if (model.SearchCampusID > 0)
                result = result.Where(p => p.CampusID.Value == model.SearchCampusID);

            int total = result.Count();
            result = model.OpenPaging(result, p => p.DeptID);

            model.CampusInfoList = context.CampusInfo;
            model.DeptInfoList = result;
            model.RowCount = total;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DeptInfo newDeptInfo)
        {
            try
            {
                context.DeptInfo.Add(newDeptInfo);
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
                var deptInfo = context.DeptInfo.Find(id);
                if (deptInfo == null)
                    return Json(new { code = 1 });
                context.DeptInfo.Remove(deptInfo);
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        [HttpPost]
        public ActionResult Update(DeptInfo updateDeptInfo)
        {
            try
            {
                context.Entry(updateDeptInfo).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        [HttpGet]
        public ActionResult Exist()
        {
            string deptName = Request["DeptName"];
            var deptInfo = context.DeptInfo.SingleOrDefault(p => p.DeptName.Equals(deptName));
            return Json(new { code = deptInfo != null ? 0 : 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}