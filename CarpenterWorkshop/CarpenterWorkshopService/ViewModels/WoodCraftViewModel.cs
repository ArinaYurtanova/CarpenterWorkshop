using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ViewModels
{
    public class WoodCraftViewModel
    {
        public int Id { get; set; }
        public string WoodCraftsName { get; set; }
        public decimal Price { get; set; }
        public List<BlankCraftViewModel> BlanksCrafts { get; set; }
    }
}
