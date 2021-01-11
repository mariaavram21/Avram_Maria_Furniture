using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Avram_Maria_Furniture.Models
{
    public class Factory
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Factory Name")]
        [StringLength(50)]
        public string FactoryName { get; set; }

        [StringLength(70)]
        public string Address { get; set; }
        public ICollection<ManufacturedFurniture> ManufacturedFurniture { get; set; }

    }
}
