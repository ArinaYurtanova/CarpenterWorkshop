using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshop
{
    //исполнитель
    public class Worker
    {
        public int Id { get; set; }
        [Required]
        public string WorkerFIO{ get; set; }
        [ForeignKey("WorkerID")]
        public virtual List<OrdProduct> OrdProducts { get; set; }
    }
}
