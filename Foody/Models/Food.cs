//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Foody.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Food
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Food()
        {
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
            this.FoodSizes = new HashSet<FoodSize>();
            this.FoodTypes = new HashSet<FoodType>();
        }
    
        public System.Guid FoodID { get; set; }
        public string FoodName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string FoodImage { get; set; }
        public Nullable<System.Guid> CategoryID { get; set; }
        public Nullable<System.Guid> StoreID { get; set; }
        public Nullable<int> FileID { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FoodSize> FoodSizes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FoodType> FoodTypes { get; set; }
        public virtual FileData FileData { get; set; }
    }
}
