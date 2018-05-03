using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ViewModels
{
    [DataContract]
    public class WoodCraftViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string WoodCraftsName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public List<BlankCraftViewModel> BlanksCrafts { get; set; }
    }
}
