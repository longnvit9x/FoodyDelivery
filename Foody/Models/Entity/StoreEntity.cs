using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Models.Entity
{
    public class StoreEntity
    {
        public String StoreID { get; set; }
        public String StoreName { get; set; }
        public String Address { get; set; }
        public String OpenDoor { get; set; }
        public String CloserDoor { get; set; }
        public Decimal ServiceCharge { get; set; }
        public Decimal ShippingFee { get; set; }
        public String Manner { get; set; }
        public String Website { get; set; }
        public String StoreType { get; set; }
        public String StoreBanner { get; set; }
        public Double LocationX { get; set; }
        public Double LocationY { get; set; }
        public Double Rating { get; set; }
        public Int16 Evaluation { get; set; }
        public Decimal ConditionShip { get; set; }
        public Int16 StartDate { get; set; }
        public Int16 EndDate { get; set; }
        public Int16 Status { get; set; }
    }
}
