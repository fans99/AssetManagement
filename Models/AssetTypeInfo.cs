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
    
    public partial class AssetTypeInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AssetTypeInfo()
        {
            this.AssetInfo = new HashSet<AssetInfo>();
        }
    
        public int AssetTypeID { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetTypeReMark { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetInfo> AssetInfo { get; set; }
    }
}
