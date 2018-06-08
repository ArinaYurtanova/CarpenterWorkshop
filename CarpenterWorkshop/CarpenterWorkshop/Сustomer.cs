using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshop
{
    //заказчик
    public class Сustomer
    {
        public int Id { get; set; }
        [Required]
        public string CustomerFIO { get; set; }
        public string Mail { get; set; }
        [ForeignKey("CustomerID")]
        public virtual List<OrdProduct> OrdProducts { get; set; }
        [ForeignKey("CustomerID")] 
        public virtual List<MessageInfo> MessageInfos { get; set; }
    }
}
