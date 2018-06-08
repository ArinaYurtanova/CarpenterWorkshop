using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.BindingModels
{
    [DataContract]
    public class StorageBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string StorageName { get; set; }
    }
}
