using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManagement.Models;

namespace AssetManagement.CustomAuthorize
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public string RoleID { get; set; }

        private readonly AssetDBEntities context = new AssetDBEntities();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (base.AuthorizeCore(httpContext))
            {
                int empolyID = int.Parse(httpContext.User.Identity.Name);
                int[] RoleList = Array.ConvertAll(RoleID.Replace(" ", "").Split(','), p => int.Parse(p));
                EmpolyInfo emp = context.EmpolyInfo.Find(empolyID);
                if (RoleList.Contains(emp.RoleID))
                    return true;
                else
                    return false;
            }
            else
                return false;
            
        }
    }
}