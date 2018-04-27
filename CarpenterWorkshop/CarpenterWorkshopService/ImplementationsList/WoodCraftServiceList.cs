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
    public class WoodCraftServiceList : IWoodCraftService
    {
            private DataListSingleton source;

            public WoodCraftServiceList()
            {
                source = DataListSingleton.GetInstance();
            }

            public List<WoodCraftViewModel> GetList()
            {
                List<WoodCraftViewModel> result = new List<WoodCraftViewModel>();
                for (int i = 0; i < source.WoodCrafts.Count; ++i)
                {
                    // требуется дополнительно получить список компонентов для изделия и их количество
                    List<BlankCraftViewModel> blankCraft = new List<BlankCraftViewModel>();
                    for (int j = 0; j < source.BlanksCrafts.Count; ++j)
                    {
                        if (source.BlanksCrafts[j].WoodCraftsID == source.WoodCrafts[i].Id)
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
                        blankCraft.Add(new BlankCraftViewModel
                        {
                                Id = source.BlanksCrafts[j].Id,
                                WoodCraftsID = source.BlanksCrafts[j].WoodCraftsID,
                                WoodBlanksID = source.BlanksCrafts[j].WoodBlanksID,
                                WoodBlanksName = woodBlanksName,
                                Count = source.BlanksCrafts[j].Count
                            });
                        }
                    }
                    result.Add(new WoodCraftViewModel
                    {
                        Id = source.WoodCrafts[i].Id,
                        WoodCraftsName = source.WoodCrafts[i].WoodCraftsName,
                        Price = source.WoodCrafts[i].Price,
                        BlanksCrafts = blankCraft
                    });
                }
                return result;
            }

            public WoodCraftViewModel GetElement(int id)
            {
                for (int i = 0; i < source.WoodCrafts.Count; ++i)
                {
                    // требуется дополнительно получить список компонентов для изделия и их количество
                    List<BlankCraftViewModel> blankCraft = new List<BlankCraftViewModel>();
                    for (int j = 0; j < source.BlanksCrafts.Count; ++j)
                    {
                        if (source.BlanksCrafts[j].WoodCraftsID == source.WoodCrafts[i].Id)
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
                        blankCraft.Add(new BlankCraftViewModel
                        {
                                Id = source.BlanksCrafts[j].Id,
                                WoodCraftsID = source.BlanksCrafts[j].WoodCraftsID,
                                WoodBlanksID = source.BlanksCrafts[j].WoodBlanksID,
                                WoodBlanksName = woodBlanksName,
                                Count = source.BlanksCrafts[j].Count
                            });
                        }
                    }
                    if (source.WoodCrafts[i].Id == id)
                    {
                        return new WoodCraftViewModel
                        {
                            Id = source.WoodCrafts[i].Id,
                            WoodCraftsName = source.WoodCrafts[i].WoodCraftsName,
                            Price = source.WoodCrafts[i].Price,
                            BlanksCrafts = blankCraft
                        };
                    }
                }

                throw new Exception("Элемент не найден");
            }

            public void AddElement(WoodCraftBindingModel model)
            {
                int maxId = 0;
                for (int i = 0; i < source.WoodCrafts.Count; ++i)
                {
                    if (source.WoodCrafts[i].Id > maxId)
                    {
                        maxId = source.WoodCrafts[i].Id;
                    }
                    if (source.WoodCrafts[i].WoodCraftsName == model.WoodCraftsName)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                }
                source.WoodCrafts.Add(new WoodCraft
                {
                    Id = maxId + 1,
                    WoodCraftsName = model.WoodCraftsName,
                    Price = model.Price
                });
                // компоненты для изделия
                int maxPCId = 0;
                for (int i = 0; i < source.BlanksCrafts.Count; ++i)
                {
                    if (source.BlanksCrafts[i].Id > maxPCId)
                    {
                        maxPCId = source.BlanksCrafts[i].Id;
                    }
                }
                // убираем дубли по компонентам
                for (int i = 0; i < model.BlanksCrafts.Count; ++i)
                {
                    for (int j = 1; j < model.BlanksCrafts.Count; ++j)
                    {
                        if (model.BlanksCrafts[i].WoodBlanksID ==
                            model.BlanksCrafts[j].WoodBlanksID)
                        {
                            model.BlanksCrafts[i].Count +=
                                model.BlanksCrafts[j].Count;
                            model.BlanksCrafts.RemoveAt(j--);
                        }
                    }
                }
                // добавляем компоненты
                for (int i = 0; i < model.BlanksCrafts.Count; ++i)
                {
                    source.BlanksCrafts.Add(new BlankCraft
                    {
                        Id = ++maxPCId,
                        WoodCraftsID = maxId + 1,
                        WoodBlanksID = model.BlanksCrafts[i].WoodBlanksID,
                        Count = model.BlanksCrafts[i].Count
                    });
                }
            }

            public void UpdElement(WoodCraftBindingModel model)
            {
                int index = -1;
                for (int i = 0; i < source.WoodCrafts.Count; ++i)
                {
                    if (source.WoodCrafts[i].Id == model.Id)
                    {
                        index = i;
                    }
                    if (source.WoodCrafts[i].WoodCraftsName == model.WoodCraftsName &&
                        source.WoodCrafts[i].Id != model.Id)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                }
                if (index == -1)
                {
                    throw new Exception("Элемент не найден");
                }
                source.WoodCrafts[index].WoodCraftsName = model.WoodCraftsName;
                source.WoodCrafts[index].Price = model.Price;
                int maxPCId = 0;
                for (int i = 0; i < source.BlanksCrafts.Count; ++i)
                {
                    if (source.BlanksCrafts[i].Id > maxPCId)
                    {
                        maxPCId = source.BlanksCrafts[i].Id;
                    }
                }
                // обновляем существуюущие компоненты
                for (int i = 0; i < source.BlanksCrafts.Count; ++i)
                {
                    if (source.BlanksCrafts[i].WoodCraftsID == model.Id)
                    {
                        bool flag = true;
                        for (int j = 0; j < model.BlanksCrafts.Count; ++j)
                        {
                            // если встретили, то изменяем количество
                            if (source.BlanksCrafts[i].Id == model.BlanksCrafts[j].Id)
                            {
                                source.BlanksCrafts[i].Count = model.BlanksCrafts[j].Count;
                                flag = false;
                                break;
                            }
                        }
                        // если не встретили, то удаляем
                        if (flag)
                        {
                            source.BlanksCrafts.RemoveAt(i--);
                        }
                    }
                }
                // новые записи
                for (int i = 0; i < model.BlanksCrafts.Count; ++i)
                {
                    if (model.BlanksCrafts[i].Id == 0)
                    {
                        // ищем дубли
                        for (int j = 0; j < source.BlanksCrafts.Count; ++j)
                        {
                            if (source.BlanksCrafts[j].WoodCraftsID == model.Id &&
                                source.BlanksCrafts[j].WoodBlanksID == model.BlanksCrafts[i].WoodBlanksID)
                            {
                                source.BlanksCrafts[j].Count += model.BlanksCrafts[i].Count;
                                model.BlanksCrafts[i].Id = source.BlanksCrafts[j].Id;
                                break;
                            }
                        }
                        // если не нашли дубли, то новая запись
                        if (model.BlanksCrafts[i].Id == 0)
                        {
                            source.BlanksCrafts.Add(new BlankCraft
                            {
                                Id = ++maxPCId,
                                WoodCraftsID = model.Id,
                                WoodBlanksID = model.BlanksCrafts[i].WoodBlanksID,
                                Count = model.BlanksCrafts[i].Count
                            });
                        }
                    }
                }
            }

            public void DelElement(int id)
            {
                // удаяем записи по компонентам при удалении изделия
                for (int i = 0; i < source.BlanksCrafts.Count; ++i)
                {
                    if (source.BlanksCrafts[i].WoodCraftsID == id)
                    {
                        source.BlanksCrafts.RemoveAt(i--);
                    }
                }
                for (int i = 0; i < source.WoodCrafts.Count; ++i)
                {
                    if (source.WoodCrafts[i].Id == id)
                    {
                        source.WoodCrafts.RemoveAt(i);
                        return;
                    }
                }
                throw new Exception("Элемент не найден");
            }
        }
    
}

