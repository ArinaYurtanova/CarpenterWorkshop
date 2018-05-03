using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ViewModels
{
    [DataContract]
    public class StoragesLoadViewModel
    {
        [DataMember]
        public string StorageName { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public IEnumerable<Tuple<string, int>> WoodBlanks { get; set; }
    }
}
