using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.BindingModels
{
    public class BlankCraftBindingModel
    {
        public int Id { get; set; }
        public int WoodBlanksID { get; set; }
        public int WoodCraftsID { get; set; }
        public int Count { get; set; }
    }
}
