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
    public class WoodBlankServiceBD : IWoodBlankService
    {
        private AbstractDbContext context;

        public WoodBlankServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<WoodBlankViewModel> GetList()
        {
            List<WoodBlankViewModel> result = context.WoodBlanks
                .Select(rec => new WoodBlankViewModel
                {
                    Id = rec.Id,
                    WoodBlanksName = rec.WoodBlanksName
                })
                .ToList();
            return result;
        }

        public WoodBlankViewModel GetElement(int id)
        {
            WoodBlank element = context.WoodBlanks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new WoodBlankViewModel
                {
                    Id = element.Id,
                    WoodBlanksName = element.WoodBlanksName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WoodBlanksBindingModel model)
        {
            WoodBlank element = context.WoodBlanks.FirstOrDefault(rec => rec.WoodBlanksName == model.WoodBlanksName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            context.WoodBlanks.Add(new WoodBlank
            {
                WoodBlanksName = model.WoodBlanksName
            });
            context.SaveChanges();
        }

        public void UpdElement(WoodBlanksBindingModel model)
        {
            WoodBlank element = context.WoodBlanks.FirstOrDefault(rec =>
                                        rec.WoodBlanksName == model.WoodBlanksName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = context.WoodBlanks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WoodBlanksName = model.WoodBlanksName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            WoodBlank element = context.WoodBlanks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.WoodBlanks.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
