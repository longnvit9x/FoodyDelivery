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
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien
        {
            get {return SoLuong * DonGia;}
        }

        public GioHang(string FoodID)
        {
            var stdId = Guid.Parse(FoodID);
            this.FoodID = FoodID;
            Food food = db.Foods.SingleOrDefault(n => n.FoodID ==stdId);
            StoreID = food.StoreID.ToString();
            FoodName = food.FoodName;
            FoodImage = food.FoodImage;
            DonGia = double.Parse(food.Price.ToString());
            SoLuong = 1;
        }
    }
}