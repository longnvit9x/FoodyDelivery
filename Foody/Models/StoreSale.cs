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
    
    public partial class StoreSale
    {
        public int StoreSaleID { get; set; }
        public Nullable<int> Sale { get; set; }
        public Nullable<System.DateTime> StartSale { get; set; }
        public Nullable<System.DateTime> StopSale { get; set; }
        public Nullable<System.Guid> StoreID { get; set; }
        public string KeySale { get; set; }
    
        public virtual Store Store { get; set; }
    }
}
