using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshop
{
    //связь компонент изделие
    public class BlankCraft
    {
        public int Id { get; set; }
        public int WoodBlanksID { get; set; }
        public int WoodCraftsID { get; set; }
        public int Count { get; set; }
    }
}
