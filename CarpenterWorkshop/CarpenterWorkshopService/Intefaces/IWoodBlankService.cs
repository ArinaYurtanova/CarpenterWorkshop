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
    [CustomInterface("Интерфейс для работы с компонентами")]
    public interface IWoodBlankService
    {
        [CustomMethod("Метод получения списка компонент")]
        List<WoodBlankViewModel> GetList();
        [CustomMethod("Метод получения компонента по id")]
        WoodBlankViewModel GetElement(int id);
        [CustomMethod("Метод добавления компонента")]
        void AddElement(WoodBlanksBindingModel model);
        [CustomMethod("Метод изменения данных по компоненту")]
        void UpdElement(WoodBlanksBindingModel model);
        [CustomMethod("Метод удаления компонента")]
        void DelElement(int id);
    }
}
