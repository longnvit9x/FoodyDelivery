using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.Models.Entity
{
   public class CustomerEntity
    {
        public string CustomerID { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public string UserType { get; set; }
    }
}
