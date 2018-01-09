using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foody.Models.Dto
{
    public class FoodAddDto
    {
        public string FoodID { get; set; }
        public int FoodSizeID { get; set; }
        public int FoodTypeID { get; set; }
        public int SoLuong { get; set; }
    }
}