using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.Intefaces
{
    public interface IMainService
    {
        List<OrdProductViewModel> GetList();

        void CreateOrder(OrdProductBindingModel model);

        void TakeOrderInWork(OrdProductBindingModel model);

        void FinishOrder(int id);

        void PayOrder(int id);

        void PutComponentOnStock(StorageBlankBindingModel model);
    }
}
