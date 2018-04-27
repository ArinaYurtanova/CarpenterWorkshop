using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ViewModels
{
    public class OrdProductViewModel
    {
        public int Id { get; set; }

        public int CustomerID { get; set; }

        public string CustomerFIO { get; set; }

        public int WoodCraftsID { get; set; }

        public string WoodCraftsName { get; set; }

        public int? WorkerID { get; set; }

        public string WorkerName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }

        public string DateCreate { get; set; }

        public string DateImplement { get; set; }
    }
}
