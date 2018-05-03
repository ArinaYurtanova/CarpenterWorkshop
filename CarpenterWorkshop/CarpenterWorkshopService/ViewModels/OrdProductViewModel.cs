using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ViewModels
{
    [DataContract]
    public class OrdProductViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }
        [DataMember]
        public int WoodCraftsID { get; set; }
        [DataMember]
        public string WoodCraftsName { get; set; }
        [DataMember]
        public int? WorkerID { get; set; }
        [DataMember]
        public string WorkerName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string DateCreate { get; set; }
        [DataMember]
        public string DateImplement { get; set; }
    }
}
