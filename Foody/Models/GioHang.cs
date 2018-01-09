using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Foody.Models
{
    public class GioHang
    {
        FoodDeliveryEntities db = new FoodDeliveryEntities();
        public string StoreID { get; set; }
        public string FoodID { get; set; }
        public string FoodName { get; set; }
        public string FoodImage { get; set; }
        public double PriceFood { get; set; }
        public string FoodSize { get; set; }
        public double PriceSize { get; set; }
        public string FoodType { get; set; }
        public double PriceType { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien
        {
            get {return SoLuong * (PriceFood + PriceSize + PriceType); }
        }

        public GioHang(string FoodID, int FoodSizeID, int FoodTypeID, int sl = 1)
        {

            FoodSize size = db.FoodSizes.Where(n => n.FoodSizeID == FoodSizeID).SingleOrDefault();
            if(size == null)
            {
                FoodSize = "";
                PriceSize = 0;
            }
            else
            {
                FoodSize = size.SizeName;
                PriceSize = double.Parse(size.PriceSize.ToString());
            }

            FoodType type = db.FoodTypes.Where(n => n.FoodTypeID == FoodTypeID).SingleOrDefault();
            if (type == null)
            {
                FoodType = "";
                PriceType = 0;
            }
            else
            {
                FoodType = size.SizeName;
                PriceType = double.Parse(type.PriceType.ToString());
            }
            var fdID = Guid.Parse(FoodID);
            this.FoodID = FoodID;
            Food food = db.Foods.SingleOrDefault(n => n.FoodID == fdID);
            StoreID = food.StoreID.ToString();
            FoodName = food.FoodName;
            FoodImage = food.FoodImage;
            PriceFood = double.Parse(food.Price.ToString());
            SoLuong = sl;
        }
    }
}