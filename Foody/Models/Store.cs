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
    
    public partial class Store
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Store()
        {
            this.Categories = new HashSet<Category>();
            this.Invoices = new HashSet<Invoice>();
            this.StoreImages = new HashSet<StoreImage>();
            this.StoreSales = new HashSet<StoreSale>();
            this.Posts = new HashSet<Post>();
        }
    
        public System.Guid StoreID { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public Nullable<System.TimeSpan> OpenDoor { get; set; }
        public Nullable<System.TimeSpan> CloserDoor { get; set; }
        public Nullable<decimal> ServiceCharge { get; set; }
        public Nullable<decimal> ShippingFee { get; set; }
        public string Manner { get; set; }
        public string Website { get; set; }
        public string StoreType { get; set; }
        public string StoreBanner { get; set; }
        public Nullable<double> Rating { get; set; }
        public Nullable<int> Evaluation { get; set; }
        public Nullable<decimal> ConditionShip { get; set; }
        public Nullable<int> StartDate { get; set; }
        public Nullable<int> EndDate { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public Nullable<int> FileID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreImage> StoreImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoreSale> StoreSales { get; set; }
        public virtual FileData FileData { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
