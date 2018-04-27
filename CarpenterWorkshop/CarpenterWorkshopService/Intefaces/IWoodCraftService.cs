using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.Intefaces
{
    public interface IWoodCraftService
    {
        List<WoodCraftViewModel> GetList();

        WoodCraftViewModel GetElement(int id);

        void AddElement(WoodCraftBindingModel model);

        void UpdElement(WoodCraftBindingModel model);

        void DelElement(int id);
    }
}
