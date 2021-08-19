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
    public class CampusInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        public ActionResult Index(CampusInfoIndexViewModel model)
        {
            IEnumerable<CampusInfo> result;
            if (!string.IsNullOrEmpty(model.SearchName))
                result = context.CampusInfo.Where(p => p.CampusName.Contains(model.SearchName));
            else
                result = context.CampusInfo;

            int total = result.Count();
            result = model.OpenPaging(result, p => p.CampusID);

            model.CampusInfoList = result;
            model.RowCount = total;

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CampusInfo newCampusInfo)
        {
            try
            {
                context.CampusInfo.Add(newCampusInfo);
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
                CampusInfo campusInfo = context.CampusInfo.Find(id);
                if (campusInfo == null)
                    return Json(new { code = 1 });
                context.CampusInfo.Remove(campusInfo);
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
            string campusName = Request["CampusName"];
            var campusInfo = context.CampusInfo.SingleOrDefault(p => p.CampusName.Equals(campusName));
            return Json(new { code = campusInfo != null ? 0 : 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}