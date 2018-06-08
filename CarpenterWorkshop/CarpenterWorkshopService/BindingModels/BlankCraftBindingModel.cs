using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CarpenterWorkshopService.BindingModels
{
    [DataContract]
    public class BlankCraftBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int WoodBlanksID { get; set; }
        [DataMember]
        public int WoodCraftsID { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
