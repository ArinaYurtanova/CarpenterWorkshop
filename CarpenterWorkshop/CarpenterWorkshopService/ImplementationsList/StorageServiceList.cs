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
            List<StorageViewModel> result = new List<StorageViewModel>();
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<StorageBlankViewModel> storageBlanks = new List<StorageBlankViewModel>();
                for (int j = 0; j < source.StorageBlanks.Count; ++j)
                {
                    if (source.StorageBlanks[j].StorageID == source.Storages[i].Id)
                    {
                        string woodBlanksName = string.Empty;
                        for (int k = 0; k < source.WoodBlanks.Count; ++k)
                        {
                            if (source.StorageBlanks[j].WoodBlanksID == source.WoodBlanks[k].Id)
                            {
                                woodBlanksName = source.WoodBlanks[k].WoodBlanksName;
                                break;
                            }
                        }
                        storageBlanks.Add(new StorageBlankViewModel
                        {
                            Id = source.StorageBlanks[j].Id,
                            StorageID = source.StorageBlanks[j].StorageID,
                            WoodBlanksID = source.StorageBlanks[j].WoodBlanksID,
                            WoodBlanksName = woodBlanksName,
                            Count = source.StorageBlanks[j].Count
                        });
                    }
                }
                result.Add(new StorageViewModel
                {
                    Id = source.Storages[i].Id,
                    StorageName = source.Storages[i].StorageName,
                    StorageBlanks = storageBlanks
                });
            }
            return result;
        }

        public StorageViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<StorageBlankViewModel> storageBlanks = new List<StorageBlankViewModel>();
                for (int j = 0; j < source.StorageBlanks.Count; ++j)
                {
                    if (source.StorageBlanks[j].StorageID == source.Storages[i].Id)
                    {
                        string woodBlanksName = string.Empty;
                        for (int k = 0; k < source.WoodBlanks.Count; ++k)
                        {
                            if (source.BlanksCrafts[j].WoodBlanksID == source.WoodBlanks[k].Id)
                            {
                                woodBlanksName = source.WoodBlanks[k].WoodBlanksName;
                                break;
                            }
                        }
                        storageBlanks.Add(new StorageBlankViewModel
                        {
                            Id = source.StorageBlanks[j].Id,
                            StorageID = source.StorageBlanks[j].StorageID,
                            WoodBlanksID = source.StorageBlanks[j].WoodBlanksID,
                            WoodBlanksName = woodBlanksName,
                            Count = source.StorageBlanks[j].Count
                        });
                    }
                }
                if (source.Storages[i].Id == id)
                {
                    return new StorageViewModel
                    {
                        Id = source.Storages[i].Id,
                        StorageName = source.Storages[i].StorageName,
                        StorageBlanks = storageBlanks
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(StorageBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                if (source.Storages[i].Id > maxId)
                {
                    maxId = source.Storages[i].Id;
                }
                if (source.Storages[i].StorageName == model.StorageName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Storages.Add(new Storage
            {
                Id = maxId + 1,
                StorageName = model.StorageName
            });
        }

        public void UpdElement(StorageBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                if (source.Storages[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Storages[i].StorageName == model.StorageName &&
                    source.Storages[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Storages[index].StorageName = model.StorageName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.StorageBlanks.Count; ++i)
            {
                if (source.StorageBlanks[i].StorageID == id)
                {
                    source.StorageBlanks.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Storages.Count; ++i)
            {
                if (source.Storages[i].Id == id)
                {
                    source.Storages.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
