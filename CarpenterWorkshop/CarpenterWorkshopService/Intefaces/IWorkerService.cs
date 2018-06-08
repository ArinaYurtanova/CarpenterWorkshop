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
    [CustomInterface("Интерфейс для работы с работниками")]
    public interface IWorkerService
    {
        [CustomMethod("Метод получения списка работников")]
        List<WorkerVeiwModel> GetList();
        [CustomMethod("Метод получения работника по id")]
        WorkerVeiwModel GetElement(int id);
        [CustomMethod("Метод добавления работника")]
        void AddElement(WorkerBindingModel model);
        [CustomMethod("Метод изменения данных по работнику")]
        void UpdElement(WorkerBindingModel model);
        [CustomMethod("Метод удаления работника")]
        void DelElement(int id);
    }
}
