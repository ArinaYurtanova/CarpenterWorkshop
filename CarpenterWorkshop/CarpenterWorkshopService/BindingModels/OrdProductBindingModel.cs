using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.BindingModels
{
    public class OrdProductBindingModel
    {
        public int Id { get; set; }

        public int CustomerID { get; set; }

        public int WoodCraftsID { get; set; }

        public int? WorkerID { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
