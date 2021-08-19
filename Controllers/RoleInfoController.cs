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
    public class RoleInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: RoleInfo
        public ActionResult Index(RoleInfoIndexViewModel model)
        {
            IEnumerable<RoleInfo> result = context.RoleInfo;
            if (!string.IsNullOrEmpty(model.SearchName))
                result = result.Where(p => p.RoleName.Contains(model.SearchName));

            int total = result.Count();
            result = model.OpenPaging(result, p => p.RoleID);

            model.RowCount = total;
            model.RoleInfoList = result;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RoleInfo newRoleInfo)
        {
            try
            {
                context.RoleInfo.Add(newRoleInfo);
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
                var roleInfo = context.RoleInfo.Find(id);
                if (roleInfo == null)
                    return Json(new { code = 1 });
                context.RoleInfo.Remove(roleInfo);
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        [HttpPost]
        public ActionResult Update(RoleInfo updateRoleInfo)
        {
            try
            {
                context.Entry(updateRoleInfo).State = System.Data.Entity.EntityState.Modified;
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
            string roleName = Request["RoleName"];
            var roleInfo = context.RoleInfo.SingleOrDefault(p => p.RoleName.Equals(roleName));
            return Json(new { code = roleInfo != null ? 0 : 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}