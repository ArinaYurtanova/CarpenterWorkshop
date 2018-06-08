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
    [CustomInterface("Интерфейс для работы с заказами")]
    public interface IMainService
    {
        [CustomMethod("Метод получения списка заказов")]
        List<OrdProductViewModel> GetList();
        [CustomMethod("Метод создания заказа")]
        void CreateOrder(OrdProductBindingModel model);
        [CustomMethod("Метод передачи заказа в работу")]
        void TakeOrderInWork(OrdProductBindingModel model);
        [CustomMethod("Метод передачи заказа на оплату")]
        void FinishOrder(int id);
        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void PayOrder(int id);
        [CustomMethod("Метод пополнения компонент на складе")]
        void  PutComponentOnStock(StorageBlankBindingModel model);
    }
}
