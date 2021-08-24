using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AssetManagement.Models;
using AssetManagement.ViewModels;
using System.Text;
using AssetManagement.CustomAuthorize;

namespace AssetManagement.Controllers
{
    [AuthorizeRole(RoleID = "1")]
    public class AssetInfoController : Controller
    {
        private readonly AssetDBEntities context = new AssetDBEntities();

        // GET: AssetInfo
        public ActionResult Index(AssetInfoIndexViewModel model)
        {
            model.AssetTypeInfoList = context.AssetTypeInfo;
            return View(model);
        }

        public ActionResult GetAssetInfos(AssetInfoIndexViewModel model)
        {
            IEnumerable<AssetInfo> resultTemp = context.AssetInfo;
            if (!string.IsNullOrEmpty(model.SearchName))
                resultTemp = resultTemp.Where(p => p.AssetName.Contains(model.SearchName));
            if (model.SearchAssetTypeID > 0)
                resultTemp = resultTemp.Where(p => p.AssetTypeID == model.SearchAssetTypeID);

            var data = resultTemp.Select(p => new
            {
                p.AssetID,
                p.AssetName,
                p.AssetTypeID,
                p.AssetTypeInfo.AssetTypeName,
                p.AssetModel,
                p.AssetCompany,
                p.AssetReMark
            });
            model.RowCount = data.Count();
            data = model.OpenPaging(data, p => p.AssetID);
            return Json(new
            {
                code = 0,
                count = model.RowCount,
                data
            });
        }

        private string GetNum()
        {
            StringBuilder str = new StringBuilder("GDZC");
            AssetDetailInfo lastAssetDetailInfo = context.AssetDetailInfo.OrderByDescending(p => p.AssetDetailID).FirstOrDefault();
            if (lastAssetDetailInfo == null)
                str.Append("000001");
            else
            {
                string temp = (int.Parse(lastAssetDetailInfo.AssetDetailNum.Replace("GDZC", ""))+1).ToString();
                while (temp.Length < 6)
                    temp = "0" + temp;
                str.Append(temp);
            }
            return str.ToString();
        }

        public ActionResult Create(AssetInfo newAssetInfo)
        {
            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    AssetInfo assetInfo = context.AssetInfo.Add(newAssetInfo);
                    context.SaveChanges();
                    if (assetInfo.AssetTypeID != 1)
                    {
                        transaction.Commit();
                        return Json(new { code = 0 });
                    }

                    AssetDetailInfo assetDetailInfo = new AssetDetailInfo()
                    {
                        AssetID = assetInfo.AssetID,
                        AssetDetailNum = GetNum(),
                        AssetDetailUseState = 1,
                        AssetDetailUseDate = DateTime.Now,
                        AssetDetailServiceState = 0,
                        EmpolyID = 0,
                        AreaID = 0,
                        AssetAreaReMark = ""
                    };
                    context.AssetDetailInfo.Add(assetDetailInfo);
                    context.SaveChanges();
                    transaction.Commit();
                    return Json(new { code = 0 });
                }
                catch(Exception err)
                {
                    Console.WriteLine(err);
                    transaction.Rollback();
                    return Json(new { code = 1 });
                }
            }
        }

        [HttpPost]
        public ActionResult Update(AssetInfo updateAssetInfo)
        {
            try
            {
                context.Entry(updateAssetInfo).State = System.Data.Entity.EntityState.Modified;
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
                AssetInfo assetInfo = context.AssetInfo.Find(id);
                if (assetInfo == null)
                    return Json(new { code = 1 });

                context.AssetInfo.Remove(assetInfo);
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