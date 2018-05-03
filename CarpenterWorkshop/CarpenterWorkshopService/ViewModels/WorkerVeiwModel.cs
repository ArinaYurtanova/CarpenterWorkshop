using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ViewModels
{
    [DataContract]
    public class WorkerVeiwModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string WorkerFIO { get; set; }
    }
}
