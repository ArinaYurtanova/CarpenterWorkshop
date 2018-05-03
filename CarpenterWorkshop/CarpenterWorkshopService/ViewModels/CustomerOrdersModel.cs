using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ViewModels
{
    [DataContract]
    public class CustomerOrdersModel
    {
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string DateCreate { get; set; }
        [DataMember]
        public string WoodCraftsName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
        [DataMember]
        public string ReadyProduct { get; set; }
    }
}
