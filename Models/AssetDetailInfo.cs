//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AssetManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssetDetailInfo
    {
        public int AssetDetailID { get; set; }
        public int AssetID { get; set; }
        public string AssetDetailNum { get; set; }
        public int AssetDetailUseState { get; set; }
        public Nullable<System.DateTime> AssetDetailUseDate { get; set; }
        public Nullable<System.DateTime> AssetDetailReturnDate { get; set; }
        public int AssetDetailServiceState { get; set; }
        public int AssetDetailDumState { get; set; }
        public int EmpolyID { get; set; }
        public int AreaID { get; set; }
        public string AssetAreaReMark { get; set; }
    
        public virtual AssetInfo AssetInfo { get; set; }
    }
}