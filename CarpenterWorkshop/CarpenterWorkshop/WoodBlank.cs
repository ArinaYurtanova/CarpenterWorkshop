using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshop
{
    //класс Компонент
    public class WoodBlank
    {
        public int Id { get; set; }
        [Required]
        public string WoodBlanksName { get; set; }
        [ForeignKey("WoodBlanksID")]
        public virtual List<BlankCraft> BlankCrafts { get; set; }

        [ForeignKey("WoodBlanksID")]
        public virtual List<StorageBlank> StorageBlanks { get; set; }

    }
}
