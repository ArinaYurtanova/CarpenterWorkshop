using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshop
{
    //класс изделие 
    public class WoodCraft
    {
        public int Id { get; set; }
        [Required]
        public string WoodCraftsName { get; set; }
        [Required]
        public decimal Price { get; set; }

        [ForeignKey("WoodCraftsID")]
        public virtual List<OrdProduct> OrdProducts { get; set; }

        [ForeignKey("WoodCraftsID")]
        public virtual List<BlankCraft> BlankCrafts { get; set; }
    }
}
