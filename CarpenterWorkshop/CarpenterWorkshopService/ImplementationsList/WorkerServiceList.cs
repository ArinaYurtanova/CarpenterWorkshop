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
            List<WorkerVeiwModel> result = new List<WorkerVeiwModel>();
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                result.Add(new WorkerVeiwModel
                {
                    Id = source.Workers[i].Id,
                    WorkerFIO = source.Workers[i].WorkerFIO
                });
            }
            return result;
        }

        public WorkerVeiwModel GetElement(int id)
        {
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                if (source.Workers[i].Id == id)
                {
                    return new WorkerVeiwModel
                    {
                        Id = source.Workers[i].Id,
                        WorkerFIO = source.Workers[i].WorkerFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WorkerBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                if (source.Workers[i].Id > maxId)
                {
                    maxId = source.Workers[i].Id;
                }
                if (source.Workers[i].WorkerFIO == model.WorkerFIO)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            source.Workers.Add(new Worker
            {
                Id = maxId + 1,
                WorkerFIO = model.WorkerFIO
            });
        }

        public void UpdElement(WorkerBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                if (source.Workers[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Workers[i].WorkerFIO == model.WorkerFIO &&
                    source.Workers[i].Id != model.Id)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Workers[index].WorkerFIO = model.WorkerFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                if (source.Workers[i].Id == id)
                {
                    source.Workers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}

    

