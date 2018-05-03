using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshop
{
    //связь склад компонент
    public class StorageBlank
    {
        public int Id { get; set; }
        public int StorageID { get; set; } 
        public int WoodBlanksID { get; set; }
        public int Count { get; set; }
        public virtual Storage Storage { get; set; }
        public virtual WoodBlank WoodBlank { get; set; }
    }
}
