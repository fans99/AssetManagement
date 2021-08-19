using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetManagement.ViewModels
{
    public static class Paging
    {
        public static IEnumerable<TSource> OpenPaging<TSource, TKey>(this BasePageViewModel model, IEnumerable<TSource> result, Func<TSource, TKey> keySelector)
        {
            if (model.Page != null && model.Limit != null)
            {
                int offset = (model.Page.Value - 1) * model.Limit.Value;
                return result.OrderBy(keySelector).Skip(offset).Take(model.Limit.Value);
            }
            else
            {
                model.Limit = 5;
                return result.OrderBy(keySelector).Take(model.Limit.Value);
            }
        }
    }
}