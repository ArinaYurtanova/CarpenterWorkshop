using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ViewModels
{
    public class CustomerOrdersModel
    {
        public string CustomerName { get; set; }

        public string DateCreate { get; set; }

        public string WoodCraftsName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string ReadyProduct { get; set; }
    }
}
