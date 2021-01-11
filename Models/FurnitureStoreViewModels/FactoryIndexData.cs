using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avram_Maria_Furniture.Models.FurnitureStoreViewModels
{
    public class FactoryIndexData
    {
        public IEnumerable<Factory> Factories { get; set; }
        public IEnumerable<Furniture> Furnitures { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
