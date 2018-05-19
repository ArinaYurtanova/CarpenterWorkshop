using CarpenterWorkshop;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ImplementationsList
{

    public class MainServiceList : IMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<OrdProductViewModel> GetList()
        {
            List<OrdProductViewModel> result = source.OrdProducts
                .Select(rec => new OrdProductViewModel
                {
                    Id = rec.Id,
                    CustomerID = rec.CustomerID,
                    WoodCraftsID = rec.WoodCraftsID,
                    WorkerID = rec.WorkerID,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateImplement = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = source.Сustomers
                                    .FirstOrDefault(recC => recC.Id == rec.CustomerID)?.CustomerFIO,
                    WoodCraftsName = source.WoodCrafts
                                    .FirstOrDefault(recP => recP.Id == rec.WoodCraftsID)?.WoodCraftsName,
                    WorkerName = source.Workers
                                    .FirstOrDefault(recI => recI.Id == rec.WorkerID)?.WorkerFIO
                })
                .ToList();
            return result;
        }

        public void CreateOrder(OrdProductBindingModel model)
        {
            int maxId = source.OrdProducts.Count > 0 ? source.OrdProducts.Max(rec => rec.Id) : 0;
            source.OrdProducts.Add(new OrdProduct
            {
                Id = maxId + 1,
                CustomerID = model.CustomerID,
                WoodCraftsID = model.WoodCraftsID,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = ReadyProduct.Принят
            });
        }

        public void TakeOrderInWork(OrdProductBindingModel model)
        {
            OrdProduct element = source.OrdProducts.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            var StorageBlanks = source.BlanksCrafts.Where(rec => rec.WoodCraftsID == element.WoodCraftsID);
            foreach (var blankCraft in StorageBlanks)
            {
                int countOnStocks = source.StorageBlanks
                                            .Where(rec => rec.WoodBlanksID == blankCraft.WoodBlanksID)
                                            .Sum(rec => rec.Count);
                if (countOnStocks < blankCraft.Count * element.Count)
                {
                    var woodBlanksName = source.WoodBlanks
                                    .FirstOrDefault(rec => rec.Id == blankCraft.WoodBlanksID);
                    throw new Exception("Не достаточно компонента " + woodBlanksName?.WoodBlanksName +
                        " требуется " + blankCraft.Count + ", в наличии " + countOnStocks);
                }
            }
            // списываем
            foreach (var blankCraft in StorageBlanks)
            {
                int countOnStorage = blankCraft.Count * element.Count;
                var storageBlanks = source.StorageBlanks
                                            .Where(rec => rec.WoodBlanksID == blankCraft.WoodBlanksID);
                foreach (var storageBlank in storageBlanks)
                {
                    // компонентов на одном слкаде может не хватать
                    if (storageBlank.Count >= countOnStorage)
                    {
                        storageBlank.Count -= countOnStorage;
                        break;
                    }
                    else
                    {
                        countOnStorage -= storageBlank.Count;
                        storageBlank.Count = 0;
                    }
                }
            }
            element.WorkerID = model.WorkerID;
            element.DateImplement = DateTime.Now;
            element.Status = ReadyProduct.Выполняется;
        }

        public void FinishOrder(int id)
        {
            OrdProduct element = source.OrdProducts.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ReadyProduct.Готов;
        }

        public void PayOrder(int id)
        {
            OrdProduct element = source.OrdProducts.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ReadyProduct.Оплачен;
        }

        public void PutComponentOnStock(StorageBlankBindingModel model)
        {
            StorageBlank element = source.StorageBlanks
                                                .FirstOrDefault(rec => rec.StorageID == model.StorageID &&
                                                                    rec.WoodBlanksID == model.WoodBlanksID);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.StorageBlanks.Count > 0 ? source.StorageBlanks.Max(rec => rec.Id) : 0;
                source.StorageBlanks.Add(new StorageBlank
                {
                    Id = ++maxId,
                    StorageID = model.StorageID,
                    WoodBlanksID = model.WoodBlanksID,
                    Count = model.Count
                });
            }
        }
    }
}
    

