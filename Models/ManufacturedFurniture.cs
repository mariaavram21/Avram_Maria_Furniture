using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avram_Maria_Furniture.Models
{
    public class ManufacturedFurniture
    {
        public int FactoryID { get; set; }
        public int FurnitureID { get; set; }
        public Factory Factory { get; set; }
        public Furniture Furniture { get; set; }
    }
}
