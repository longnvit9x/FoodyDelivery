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
    
    public partial class GET_LIST_INVOICE_BY_STOREID_Result
    {
        public System.Guid InvoiceID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string AddressDelivery { get; set; }
        public Nullable<System.Guid> StoreID { get; set; }
        public Nullable<decimal> Sale { get; set; }
        public Nullable<decimal> ShippingFee { get; set; }
        public Nullable<decimal> ServiceChange { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
