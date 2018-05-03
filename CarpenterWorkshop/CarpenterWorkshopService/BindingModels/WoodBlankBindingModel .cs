using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.BindingModels
{
    [DataContract]
    public class WoodBlanksBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string WoodBlanksName { get; set; }
    }
}
