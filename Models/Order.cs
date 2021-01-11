using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avram_Maria_Furniture.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int FurnitureID { get; set; }
        public DateTime OrderDate { get; set; }


        public Customer Customer { get; set; }
        public Furniture Furniture { get; set; }
    }
}
