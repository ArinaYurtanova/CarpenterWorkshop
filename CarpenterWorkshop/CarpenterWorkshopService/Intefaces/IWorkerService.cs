using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.Intefaces
{
    public interface IWorkerService
    {
        List<WorkerVeiwModel> GetList();

        WorkerVeiwModel GetElement(int id);

        void AddElement(WorkerBindingModel model);

        void UpdElement(WorkerBindingModel model);

        void DelElement(int id);
    }
}
