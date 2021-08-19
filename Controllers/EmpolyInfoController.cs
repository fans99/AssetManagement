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
    public class EmpolyInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();
        // GET: EmpolyInfo
        public ActionResult Index()
        {
            EmpolyInfoIndexViewModel model = new EmpolyInfoIndexViewModel()
            {
                DeptInfoList = context.DeptInfo,
                RoleInfoList = context.RoleInfo
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GetEmpolyInfos(EmpolyInfoIndexViewModel model)
        {
            IEnumerable<EmpolyInfo> resultTemp = context.EmpolyInfo;
            if (!string.IsNullOrEmpty(model.SearchName))
                resultTemp = resultTemp.Where(p => p.EmpolyName.Contains(model.SearchName));
            if (!string.IsNullOrEmpty(model.SearchSex))
                resultTemp = resultTemp.Where(p => p.EmpolySex.Equals(model.SearchSex));
            if (model.SearchDeptID > 0)
                resultTemp = resultTemp.Where(p => p.DeptID == model.SearchDeptID);
            if (model.SearchRoleID > 0)
                resultTemp = resultTemp.Where(p => p.RoleID == model.SearchRoleID);

            var data = resultTemp.Select(p => new
            {
                p.EmpolyID,
                p.EmpolyNum,
                p.EmpolyName,
                p.EmpolySex,
                p.EmpolyldCard,
                p.DeptID,
                p.DeptInfo.DeptName,
                p.RoleID,
                p.RoleInfo.RoleName,
                p.EmpolyLevel,
                InductionDate = p.InductionDate.HasValue ? p.InductionDate.Value.ToString("yyyy-MM-dd") : "",
                DepartureDate = p.DepartureDate.HasValue ? p.DepartureDate.Value.ToString("yyyy-MM-dd") : "",
                p.EmpolyReMark
            });
            model.RowCount = data.Count();
            data = model.OpenPaging(data, p => p.EmpolyID);
            
            return Json(new
            {
                code = 0,
                count = model.RowCount,
                data
            });
        }

        [HttpPost]
        public ActionResult Create(EmpolyInfo newEmpolyInfo)
        {
            try
            {
                newEmpolyInfo.EmpolyPwd = "123456";
                if (newEmpolyInfo.Empolylmg == null)
                    newEmpolyInfo.Empolylmg = "";
                context.EmpolyInfo.Add(newEmpolyInfo);
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
        public ActionResult Update(EmpolyInfo updateEmpolyInfo)
        {
            try
            {
                EmpolyInfo empolyInfo = context.EmpolyInfo.Find(updateEmpolyInfo.EmpolyID);
                empolyInfo.EmpolyName = updateEmpolyInfo.EmpolyName;
                empolyInfo.EmpolySex = updateEmpolyInfo.EmpolySex;
                empolyInfo.EmpolyldCard = updateEmpolyInfo.EmpolyldCard;
                empolyInfo.DeptID = empolyInfo.DeptID;
                empolyInfo.EmpolyLevel = updateEmpolyInfo.EmpolyLevel;
                empolyInfo.RoleID = updateEmpolyInfo.RoleID;
                empolyInfo.InductionDate = updateEmpolyInfo.InductionDate;
                empolyInfo.DepartureDate = updateEmpolyInfo.DepartureDate;
                empolyInfo.EmpolyReMark = updateEmpolyInfo.EmpolyReMark;
                context.Entry(empolyInfo).State = System.Data.Entity.EntityState.Modified;
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
                EmpolyInfo empolyInfo = context.EmpolyInfo.Find(id);
                if (empolyInfo == null)
                    return Json(new { code = 1 });

                context.EmpolyInfo.Remove(empolyInfo);
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
            string empolyNum = Request["EmpolyNum"];
            EmpolyInfo empolyInfo = context.EmpolyInfo.SingleOrDefault(p => p.EmpolyNum.Equals(empolyNum));
            return Json(new { code = empolyInfo != null ? 0 : 1 }, JsonRequestBehavior.AllowGet);
        }
    }
}