using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.Intefaces
{
    public interface IReportService
    {
        void SaveProductPrice(ReportBindingModel model);

        List<StoragesLoadViewModel> GetStoragesLoad();

        void SaveStoragesLoad(ReportBindingModel model);

        List<CustomerOrdersModel> GetCustomerOrders(ReportBindingModel model);

        void SaveCustomerOrders(ReportBindingModel model);
    }
}
