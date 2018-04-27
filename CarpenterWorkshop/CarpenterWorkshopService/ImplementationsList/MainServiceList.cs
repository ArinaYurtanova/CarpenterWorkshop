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
                List<OrdProductViewModel> result = new List<OrdProductViewModel>();
                for (int i = 0; i < source.OrdProducts.Count; ++i)
                {
                    string customerFIO = string.Empty;
                    for (int j = 0; j < source.Сustomers.Count; ++j)
                    {
                        if (source.Сustomers[j].Id == source.OrdProducts[i].CustomerID)
                        {
                            customerFIO = source.Сustomers[j].CustomerFIO;
                            break;
                        }
                    }
                    string woodCraftsName = string.Empty;
                    for (int j = 0; j < source.WoodCrafts.Count; ++j)
                    {
                        if (source.WoodCrafts[j].Id == source.OrdProducts[i].WoodCraftsID)
                        {
                            woodCraftsName = source.WoodCrafts[j].WoodCraftsName;
                            break;
                        }
                    }
                    string workerFIO = string.Empty;
                    if (source.OrdProducts[i].WorkerID.HasValue)
                    {
                        for (int j = 0; j < source.Workers.Count; ++j)
                        {
                            if (source.Workers[j].Id == source.OrdProducts[i].WorkerID.Value)
                            {
                                workerFIO = source.Workers[j].WorkerFIO;
                                break;
                            }
                        }
                    }
                    result.Add(new OrdProductViewModel
                    {
                        Id = source.OrdProducts[i].Id,
                        CustomerID = source.OrdProducts[i].CustomerID,
                        CustomerFIO = customerFIO,
                        WoodCraftsID = source.OrdProducts[i].WoodCraftsID,
                        WoodCraftsName = woodCraftsName,
                        WorkerID = source.OrdProducts[i].WorkerID,
                        WorkerName = workerFIO,
                        Count = source.OrdProducts[i].Count,
                        Sum = source.OrdProducts[i].Sum,
                        DateCreate = source.OrdProducts[i].DateCreate.ToLongDateString(),
                        DateImplement = source.OrdProducts[i].DateImplement?.ToLongDateString(),
                        Status = source.OrdProducts[i].Status.ToString()
                    });
                }
                return result;
            }

            public void CreateOrder(OrdProductBindingModel model)
            {
                int maxId = 0;
                for (int i = 0; i < source.OrdProducts.Count; ++i)
                {
                    if (source.OrdProducts[i].Id > maxId)
                    {
                        maxId = source.Сustomers[i].Id;
                    }
                }
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
                int index = -1;
                for (int i = 0; i < source.OrdProducts.Count; ++i)
                {
                    if (source.OrdProducts[i].Id == model.Id)
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    throw new Exception("Элемент не найден");
                }
                // смотрим по количеству компонентов на складах
                for (int i = 0; i < source.BlanksCrafts.Count; ++i)
                {
                    if (source.BlanksCrafts[i].WoodCraftsID == source.OrdProducts[index].WoodCraftsID)
                    {
                        int countOnStocks = 0;
                        for (int j = 0; j < source.StorageBlanks.Count; ++j)
                        {
                            if (source.StorageBlanks[j].WoodBlanksID == source.BlanksCrafts[i].WoodBlanksID)
                            {
                                countOnStocks += source.StorageBlanks[j].Count;
                            }
                        }
                        if (countOnStocks < source.BlanksCrafts[i].Count * source.OrdProducts[index].Count)
                        {
                            for (int j = 0; j < source.WoodBlanks.Count; ++j)
                            {
                                if (source.WoodBlanks[j].Id == source.BlanksCrafts[i].WoodBlanksID)
                                {
                                    throw new Exception("Не достаточно компонента " + source.WoodBlanks[j].WoodBlanksName +
                                        " требуется " + source.BlanksCrafts[i].Count + ", в наличии " + countOnStocks);
                                }
                            }
                        }
                    }
                }
                // списываем
                for (int i = 0; i < source.BlanksCrafts.Count; ++i)
                {
                    if (source.BlanksCrafts[i].WoodCraftsID == source.OrdProducts[index].WoodCraftsID)
                    {
                        int countOnStocks = source.BlanksCrafts[i].Count * source.OrdProducts[index].Count;
                        for (int j = 0; j < source.StorageBlanks.Count; ++j)
                        {
                            if (source.StorageBlanks[j].WoodBlanksID == source.BlanksCrafts[i].WoodBlanksID)
                            {
                                // компонентов на одном слкаде может не хватать
                                if (source.StorageBlanks[j].Count >= countOnStocks)
                                {
                                    source.StorageBlanks[j].Count -= countOnStocks;
                                    break;
                                }
                                else
                                {
                                    countOnStocks -= source.StorageBlanks[j].Count;
                                    source.StorageBlanks[j].Count = 0;
                                }
                            }
                        }
                    }
                }
                source.OrdProducts[index].WorkerID = model.WorkerID;
                source.OrdProducts[index].DateImplement = DateTime.Now;
                source.OrdProducts[index].Status = ReadyProduct.Выполняется;
            }

            public void FinishOrder(int id)
            {
                int index = -1;
                for (int i = 0; i < source.OrdProducts.Count; ++i)
                {
                    if (source.Сustomers[i].Id == id)
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    throw new Exception("Элемент не найден");
                }
                source.OrdProducts[index].Status = ReadyProduct.Готов;
            }

            public void PayOrder(int id)
            {
                int index = -1;
                for (int i = 0; i < source.OrdProducts.Count; ++i)
                {
                    if (source.Сustomers[i].Id == id)
                    {
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    throw new Exception("Элемент не найден");
                }
                source.OrdProducts[index].Status = ReadyProduct.Оплачен;
            }

            public void PutComponentOnStock(StorageBlankBindingModel model)
            {
                int maxId = 0;
                for (int i = 0; i < source.StorageBlanks.Count; ++i)
                {
                    if (source.StorageBlanks[i].StorageID == model.StorageID &&
                        source.StorageBlanks[i].WoodBlanksID == model.WoodBlanksID)
                    {
                        source.StorageBlanks[i].Count += model.Count;
                        return;
                    }
                    if (source.StorageBlanks[i].Id > maxId)
                    {
                        maxId = source.StorageBlanks[i].Id;
                    }
                }
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

