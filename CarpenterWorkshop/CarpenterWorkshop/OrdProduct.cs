using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshop
{
    //заказ
    public class OrdProduct
    {
        public int Id { get; set; }

        public int CustomerID { get; set; }

        public int WoodCraftsID { get; set; }

        public int? WorkerID { get; set; } //int? – конструкция, подразумевающая возможность хранения значения null

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public ReadyProduct Status { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateImplement { get; set; }
    }
}
