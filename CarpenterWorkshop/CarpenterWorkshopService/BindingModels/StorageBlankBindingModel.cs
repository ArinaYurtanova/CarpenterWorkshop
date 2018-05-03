using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.BindingModels
{
    [DataContract]
    public class StorageBlankBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int StorageID { get; set; }
        [DataMember]
        public int WoodBlanksID { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
