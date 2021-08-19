using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManagement.Models;
using AssetManagement.ViewModels;
using System.Web.Security;
using System.Security.Authentication;

namespace AssetManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(HomeLoginViewModel model)
        {
            EmpolyInfo emp = context.EmpolyInfo.SingleOrDefault(p => p.EmpolyNum.Equals(model.UserName) && p.EmpolyPwd.Equals(model.PassWord));
            if (emp != null)
            {
                FormsAuthentication.SetAuthCookie(emp.EmpolyID.ToString(), model.RememberPwd);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
      
        public ActionResult Index()
        {           
            return View();
        }

        [HttpGet]
        [ActionName("Profile")]
        public ActionResult EmpProfile()
        {
            EmpolyInfo emp = context.EmpolyInfo.Find(int.Parse(User.Identity.Name));
            HomeProfileViewModel model = new HomeProfileViewModel()
            {
                EmpolyID = emp.EmpolyID,
                EmpolyName = emp.EmpolyName,
                DeptName = emp.DeptInfo.DeptName,
                RoleName = emp.RoleInfo.RoleName,
                EmpolyldCard = emp.EmpolyldCard,
                EmpolyLevel = emp.EmpolyLevel,
                InductionDate = emp.InductionDate.HasValue ? emp.InductionDate.Value.ToString("yyyy-MM-dd") : "",
                DepartureDate = emp.DepartureDate.HasValue ? emp.DepartureDate.Value.ToString("yyyy-MM-dd") : "",
                EmpolyReMark = emp.EmpolyReMark,
            };
            return View("Profile", model);
        }

        [HttpPost]
        [ActionName("Profile")]
        public ActionResult EmpProfile(HomeProfileViewModel model)
        {
            EmpolyInfo updateEmpolyInfo = context.EmpolyInfo.Find(model.EmpolyID);
            updateEmpolyInfo.EmpolyName = model.EmpolyName;
            updateEmpolyInfo.EmpolyReMark = model.EmpolyReMark;
            context.Entry(updateEmpolyInfo).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return View("Profile", model);
        }

        [HttpGet]
        public ActionResult GetImg(int id)
        {
            EmpolyInfo emp = context.EmpolyInfo.Find(id);
            if (emp == null)
                return Json(new { code = 1 }, JsonRequestBehavior.AllowGet);
            else
                return Json(new
                {
                    code = 0,
                    result = emp.Empolylmg
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadImg(int id)
        {
            try
            {
                EmpolyInfo emp = context.EmpolyInfo.Find(id);
                if (emp == null)
                    return Json(new { code = 1 });
                HttpPostedFileBase postFile = Request.Files["ImgFile"];
                string[] t = postFile.FileName.Split('.');
                string suffix = t[t.Length - 1];
                string customName = postFile.FileName.Substring(0, postFile.FileName.Length - suffix.Length) + Guid.NewGuid().ToString().Replace("-", "");
                postFile.SaveAs(Server.MapPath("~/Content/upload/user/") + customName + "." + suffix);
                emp.Empolylmg = customName + "." + suffix;
                context.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return Json(new { code = 0 });
            }
            catch
            {
                return Json(new { code = 1 });
            }
        }

        [HttpGet]
        public ActionResult Password()
        {
            EmpolyInfo emp = context.EmpolyInfo.Find(int.Parse(User.Identity.Name));
            HomePasswordViewModel model = new HomePasswordViewModel()
            {
                EmpolyID = emp.EmpolyID
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Password(HomePasswordViewModel model)
        {
            EmpolyInfo emp = context.EmpolyInfo.Find(model.EmpolyID);
            if (emp == null)
                return Json(new { code = 1, errMessage = "EmpolyID" });
            if (!emp.EmpolyPwd.Equals(model.OldPassword))
                return Json(new { code = 1, errMessage = "OldPassword" });

            emp.EmpolyPwd = model.NewPassword;
            context.Entry(emp).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return Json(new { code = 0 });
        }
    }
}