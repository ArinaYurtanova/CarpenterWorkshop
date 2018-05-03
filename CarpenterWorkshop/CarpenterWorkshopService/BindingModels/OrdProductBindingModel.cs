using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.BindingModels
{
    [DataContract]
    public class OrdProductBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int WoodCraftsID { get; set; }
        [DataMember]
        public int? WorkerID { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
    }
}
