using CarpenterWorkshopService.Attributies;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.Intefaces
{
    [CustomInterface("Интерфейс для работы с изделиями")]
    public interface IWoodCraftService
    {
        [CustomMethod("Метод получения списка изделий")]
        List<WoodCraftViewModel> GetList();
        [CustomMethod("Метод получения изделия по id")]
        WoodCraftViewModel GetElement(int id);
        [CustomMethod("Метод добавления изделия")]
        void AddElement(WoodCraftBindingModel model);
        [CustomMethod("Метод изменения данных по изделию")]
        void UpdElement(WoodCraftBindingModel model);
        [CustomMethod("Метод удаления изделия")]
        void DelElement(int id);
    }
}
