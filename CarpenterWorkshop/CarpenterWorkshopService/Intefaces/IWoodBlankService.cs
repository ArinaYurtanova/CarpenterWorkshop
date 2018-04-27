using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.Intefaces
{
    public interface IWoodBlankService
    {
        List<WoodBlankViewModel> GetList();

        WoodBlankViewModel GetElement(int id);

        void AddElement(WoodBlanksBindingModel model);

        void UpdElement(WoodBlanksBindingModel model);

        void DelElement(int id);
    }
}
