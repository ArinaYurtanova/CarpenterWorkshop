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
    public class StorageServiceList : IStorageService
    {
        private DataListSingleton source;

        public StorageServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<StorageViewModel> GetList()
        {
            List<StorageViewModel> result = source.Storages
                .Select(rec => new StorageViewModel
                {
                    Id = rec.Id,
                    StorageName = rec.StorageName,
                    StorageBlanks = source.StorageBlanks
                            .Where(recPC => recPC.StorageID == rec.Id)
                            .Select(recPC => new StorageBlankViewModel
                            {
                                Id = recPC.Id,
                                StorageID = recPC.StorageID,
                                WoodBlanksID = recPC.WoodBlanksID,
                                WoodBlanksName = source.WoodBlanks
                                    .FirstOrDefault(recC => recC.Id == recPC.WoodBlanksID)?.WoodBlanksName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public StorageViewModel GetElement(int id)
        {
            Storage element = source.Storages.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new StorageViewModel
                {
                    Id = element.Id,
                    StorageName = element.StorageName,
                    StorageBlanks = source.StorageBlanks
                            .Where(recPC => recPC.StorageID == element.Id)
                            .Select(recPC => new StorageBlankViewModel
                            {
                                Id = recPC.Id,
                                StorageID = recPC.StorageID,
                                WoodBlanksID = recPC.WoodBlanksID,
                                WoodBlanksName = source.WoodBlanks
                                    .FirstOrDefault(recC => recC.Id == recPC.WoodBlanksID)?.WoodBlanksName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(StorageBindingModel model)
        {
            Storage element = source.Storages.FirstOrDefault(rec => rec.StorageName == model.StorageName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            int maxId = source.Storages.Count > 0 ? source.Storages.Max(rec => rec.Id) : 0;
            source.Storages.Add(new Storage
            {
                Id = maxId + 1,
                StorageName = model.StorageName
            });
        }

        public void UpdElement(StorageBindingModel model)
        {
            Storage element = source.Storages.FirstOrDefault(rec =>
                                        rec.StorageName == model.StorageName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = source.Storages.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.StorageName = model.StorageName;
        }

        public void DelElement(int id)
        {
            Storage element = source.Storages.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // при удалении удаляем все записи о компонентах на удаляемом складе
                source.StorageBlanks.RemoveAll(rec => rec.StorageID == id);
                source.Storages.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
