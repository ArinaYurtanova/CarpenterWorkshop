using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ViewModels
{
    [DataContract]
    public class BlankCraftViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int WoodBlanksID { get; set; }
        [DataMember]
        public string WoodBlanksName { get; set; }
        [DataMember]
        public int WoodCraftsID { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
