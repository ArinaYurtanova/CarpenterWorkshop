using CarpenterWorkshop;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpenterWorkshopService.ImplementationsBD
{
    public class WoodCraftServiceBD : IWoodCraftService
    {
        private AbstractDbContext context;

        public WoodCraftServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<WoodCraftViewModel> GetList()
        {
            List<WoodCraftViewModel> result = context.WoodCrafts
                .Select(rec => new WoodCraftViewModel
                {
                    Id = rec.Id,
                    WoodCraftsName = rec.WoodCraftsName,
                    Price = rec.Price,
                    BlanksCrafts = context.BlankCrafts
                            .Where(recPC => recPC.WoodCraftsID == rec.Id)
                            .Select(recPC => new BlankCraftViewModel
                            {
                                Id = recPC.Id,
                                WoodCraftsID = recPC.WoodCraftsID,
                                WoodBlanksID = recPC.WoodBlanksID,
                                WoodBlanksName = recPC.WoodBlank.WoodBlanksName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public WoodCraftViewModel GetElement(int id)
        {
            WoodCraft element = context.WoodCrafts.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new WoodCraftViewModel
                {
                    Id = element.Id,
                    WoodCraftsName = element.WoodCraftsName,
                    Price = element.Price,
                    BlanksCrafts = context.BlankCrafts
                            .Where(recPC => recPC.WoodCraftsID == element.Id)
                            .Select(recPC => new BlankCraftViewModel
                            {
                                Id = recPC.Id,
                                WoodCraftsID = recPC.WoodCraftsID,
                                WoodBlanksID = recPC.WoodBlanksID,
                                WoodBlanksName = recPC.WoodBlank.WoodBlanksName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WoodCraftBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    WoodCraft element = context.WoodCrafts.FirstOrDefault(rec => rec.WoodCraftsName == model.WoodCraftsName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new WoodCraft
                    {
                        WoodCraftsName = model.WoodCraftsName,
                        Price = model.Price
                    };
                    context.WoodCrafts.Add(element);
                    context.SaveChanges();
                    // убираем дубли по компонентам
                    var groupComponents = model.BlanksCrafts
                                                .GroupBy(rec => rec.WoodBlanksID)
                                                .Select(rec => new
                                                {
                                                    WoodBlankID = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    // добавляем компоненты
                    foreach (var groupComponent in groupComponents)
                    {
                        context.BlankCrafts.Add(new BlankCraft
                        {
                            WoodCraftsID = element.Id,
                            WoodBlanksID = groupComponent.WoodBlankID,
                            Count = groupComponent.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdElement(WoodCraftBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    WoodCraft element = context.WoodCrafts.FirstOrDefault(rec =>
                                        rec.WoodCraftsName == model.WoodCraftsName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.WoodCrafts.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.WoodCraftsName = model.WoodCraftsName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты
                    var compIds = model.BlanksCrafts.Select(rec => rec.WoodCraftsID).Distinct();
                    var updateComponents = context.BlankCrafts
                                                    .Where(rec => rec.WoodCraftsID == model.Id &&
                                                        compIds.Contains(rec.WoodBlanksID));
                    foreach (var updateComponent in updateComponents)
                    {
                        updateComponent.Count = model.BlanksCrafts
                                                        .FirstOrDefault(rec => rec.Id == updateComponent.Id).Count;
                    }
                    context.SaveChanges();
                    context.BlankCrafts.RemoveRange(
                                        context.BlankCrafts.Where(rec => rec.WoodCraftsID == model.Id &&
                                                                            !compIds.Contains(rec.WoodBlanksID)));
                    context.SaveChanges();
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
                        BlankCraft elementPC = context.BlankCrafts
                                                .FirstOrDefault(rec => rec.WoodCraftsID == model.Id &&
                                                                rec.WoodBlanksID == groupComponent.WoodBlanksID);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupComponent.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.BlankCrafts.Add(new BlankCraft
                            {
                                WoodCraftsID = model.Id,
                                WoodBlanksID = groupComponent.WoodBlanksID,
                                Count = groupComponent.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    WoodCraft element = context.WoodCrafts.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.BlankCrafts.RemoveRange(
                                            context.BlankCrafts.Where(rec => rec.WoodCraftsID == id));
                        context.WoodCrafts.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
