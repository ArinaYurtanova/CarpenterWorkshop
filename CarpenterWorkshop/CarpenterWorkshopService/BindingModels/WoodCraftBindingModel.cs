using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.BindingModels
{
    [DataContract]
    public class WoodCraftBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string WoodCraftsName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public List<BlankCraftBindingModel> BlanksCrafts { get; set; }
    }
}
