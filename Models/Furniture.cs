using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avram_Maria_Furniture.Models
{
    public class Furniture
    {
        public int ID { get; set; }
        [Display(Name = "Furniture name")]
        [Required, StringLength(150, MinimumLength = 2)]
        public string Title { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9&_\.-:'\s']+$"),
StringLength(250, MinimumLength = 0)]
        public string Description { get; set; }
        public string Color { get; set; }
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ManufacturedFurniture> ManufacturedFurniture { get; set; }
    }
}
