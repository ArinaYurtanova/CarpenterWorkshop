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
    public class WoodBlankServiceList : IWoodBlankService
    {
        private DataListSingleton source;

        public WoodBlankServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<WoodBlankViewModel> GetList()
        {
            List<WoodBlankViewModel> result = source.WoodBlanks
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
            WoodBlank element = source.WoodBlanks.FirstOrDefault(rec => rec.Id == id);
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
            WoodBlank element = source.WoodBlanks.FirstOrDefault(rec => rec.WoodBlanksName == model.WoodBlanksName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            int maxId = source.WoodBlanks.Count > 0 ? source.WoodBlanks.Max(rec => rec.Id) : 0;
            source.WoodBlanks.Add(new WoodBlank
            {
                Id = maxId + 1,
                WoodBlanksName = model.WoodBlanksName
            });
        }

        public void UpdElement(WoodBlanksBindingModel model)
        {
            WoodBlank element = source.WoodBlanks.FirstOrDefault(rec =>
                                        rec.WoodBlanksName == model.WoodBlanksName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = source.WoodBlanks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WoodBlanksName = model.WoodBlanksName;
        }

        public void DelElement(int id)
        {
            WoodBlank element = source.WoodBlanks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.WoodBlanks.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
