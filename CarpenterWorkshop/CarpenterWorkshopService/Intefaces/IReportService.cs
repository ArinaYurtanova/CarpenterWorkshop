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
    [CustomInterface("Интерфейс для работы с отчетами")]
    public interface IReportService
    {
        [CustomMethod("Метод сохранения списка изделий в doc-файл")]
        void SaveProductPrice(ReportBindingModel model);
        [CustomMethod("Метод получения списка складов с количество компонент на них")]
        List<StoragesLoadViewModel> GetStoragesLoad();
        [CustomMethod("Метод сохранения списка списка складов с количество компонент на них в xls-файл")]
        void SaveStoragesLoad(ReportBindingModel model);
        [CustomMethod("Метод получения списка заказов клиентов")]
        List<CustomerOrdersModel> GetCustomerOrders(ReportBindingModel model);
        [CustomMethod("Метод сохранения списка заказов клиентов в pdf-файл")]
        void SaveCustomerOrders(ReportBindingModel model);
    }
}
