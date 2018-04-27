using System;
using CarpenterWorkshopService.Intefaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarpenterWorkshop;
using CarpenterWorkshopService.ViewModels;
using CarpenterWorkshopService.BindingModels;

namespace CarpenterWorkshopService.ImplementationsList
{
    public class WorkerServiceList: IWorkerService
    {
        private DataListSingleton source;

        public WorkerServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<WorkerVeiwModel> GetList()
        {
            List<WorkerVeiwModel> result = source.Workers
                .Select(rec => new WorkerVeiwModel
                {
                    Id = rec.Id,
                    WorkerFIO = rec.WorkerFIO
                })
                .ToList();
            return result;
        }

        public WorkerVeiwModel GetElement(int id)
        {
            Worker element = source.Workers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new WorkerVeiwModel
                {
                    Id = element.Id,
                    WorkerFIO = element.WorkerFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WorkerBindingModel model)
        {
            Worker element = source.Workers.FirstOrDefault(rec => rec.WorkerFIO == model.WorkerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            int maxId = source.Workers.Count > 0 ? source.Workers.Max(rec => rec.Id) : 0;
            source.Workers.Add(new Worker
            {
                Id = maxId + 1,
                WorkerFIO = model.WorkerFIO
            });
        }

        public void UpdElement(WorkerBindingModel model)
        {
            Worker element = source.Workers.FirstOrDefault(rec =>
                                        rec.WorkerFIO == model.WorkerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = source.Workers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WorkerFIO = model.WorkerFIO;
        }

        public void DelElement(int id)
        {
            Worker element = source.Workers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Workers.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}

    

