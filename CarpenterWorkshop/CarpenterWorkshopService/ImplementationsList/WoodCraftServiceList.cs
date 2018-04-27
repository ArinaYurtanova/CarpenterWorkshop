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
            List<WoodCraftViewModel> result = source.WoodCrafts
                .Select(rec => new WoodCraftViewModel
                {
                    Id = rec.Id,
                    WoodCraftsName = rec.WoodCraftsName,
                    Price = rec.Price,
                    BlanksCrafts = source.BlanksCrafts
                            .Where(recPC => recPC.WoodCraftsID == rec.Id)
                            .Select(recPC => new BlankCraftViewModel
                            {
                                Id = recPC.Id,
                                WoodCraftsID = recPC.WoodCraftsID,
                                WoodBlanksID= recPC.WoodBlanksID,
                                WoodBlanksName = source.WoodBlanks
                                    .FirstOrDefault(recC => recC.Id == recPC.WoodBlanksID)?.WoodBlanksName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public WoodCraftViewModel GetElement(int id)
        {
            WoodCraft element = source.WoodCrafts.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new WoodCraftViewModel
                {
                    Id = element.Id,
                    WoodCraftsName = element.WoodCraftsName,
                    Price = element.Price,
                    BlanksCrafts = source.BlanksCrafts
                            .Where(recPC => recPC.WoodCraftsID == element.Id)
                            .Select(recPC => new BlankCraftViewModel
                            {
                                Id = recPC.Id,
                                WoodCraftsID = recPC.WoodCraftsID,
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

        public void AddElement(WoodCraftBindingModel model)
        {
            WoodCraft element = source.WoodCrafts.FirstOrDefault(rec => rec.WoodCraftsName == model.WoodCraftsName);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            int maxId = source.WoodCrafts.Count > 0 ? source.WoodCrafts.Max(rec => rec.Id) : 0;
            source.WoodCrafts.Add(new WoodCraft
            {
                Id = maxId + 1,
                WoodCraftsName = model.WoodCraftsName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = source.BlanksCrafts.Count > 0 ?
                                    source.BlanksCrafts.Max(rec => rec.Id) : 0;
            // убираем дубли по компонентам
            var groupComponents = model.BlanksCrafts
                                        .GroupBy(rec => rec.WoodBlanksID)
                                        .Select(rec => new
                                        {
                                            WoodBlanksID = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            // добавляем компоненты
            foreach (var groupComponent in groupComponents)
            {
                source.BlanksCrafts.Add(new BlankCraft
                {
                    Id = ++maxPCId,
                    WoodCraftsID = maxId + 1,
                    WoodBlanksID = groupComponent.WoodBlanksID,
                    Count = groupComponent.Count
                });
            }
        }

        public void UpdElement(WoodCraftBindingModel model)
        {
            WoodCraft element = source.WoodCrafts.FirstOrDefault(rec =>
                                        rec.WoodCraftsName == model.WoodCraftsName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            element = source.WoodCrafts.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WoodCraftsName = model.WoodCraftsName;
            element.Price = model.Price;

            int maxPCId = source.BlanksCrafts.Count > 0 ? source.BlanksCrafts.Max(rec => rec.Id) : 0;
            // обновляем существуюущие компоненты
            var compIds = model.BlanksCrafts.Select(rec => rec.WoodBlanksID).Distinct();
            var updateComponents = source.BlanksCrafts
                                            .Where(rec => rec.WoodCraftsID == model.Id &&
                                           compIds.Contains(rec.WoodBlanksID));
            foreach (var updateComponent in updateComponents)
            {
                updateComponent.Count = model.BlanksCrafts
                                                .FirstOrDefault(rec => rec.Id == updateComponent.Id).Count;
            }
            source.BlanksCrafts.RemoveAll(rec => rec.WoodCraftsID == model.Id &&
                                       !compIds.Contains(rec.WoodBlanksID));
            // новые записи
            var groupComponents = model.BlanksCrafts
                                        .Where(rec => rec.Id == 0)
                                        .GroupBy(rec => rec.WoodBlanksID)
                                        .Select(rec => new
                                        {
                                            WoodBlanksID = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupComponent in groupComponents)
            {
                BlankCraft elementPC = source.BlanksCrafts
                                        .FirstOrDefault(rec => rec.WoodCraftsID == model.Id &&
                                                        rec.WoodBlanksID == groupComponent.WoodBlanksID);
                if (elementPC != null)
                {
                    elementPC.Count += groupComponent.Count;
                }
                else
                {
                    source.BlanksCrafts.Add(new BlankCraft
                    {
                        Id = ++maxPCId,
                        WoodCraftsID = model.Id,
                        WoodBlanksID = groupComponent.WoodBlanksID,
                        Count = groupComponent.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            WoodCraft element = source.WoodCrafts.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.BlanksCrafts.RemoveAll(rec => rec.WoodCraftsID == id);
                source.WoodCrafts.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
    
}

