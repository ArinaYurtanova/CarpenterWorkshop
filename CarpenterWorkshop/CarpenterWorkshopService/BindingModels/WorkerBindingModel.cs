using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.BindingModels
{
    [DataContract]
    public class WorkerBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string WorkerFIO { get; set; }
    }
}
