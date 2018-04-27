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
            List<WoodBlankViewModel> result = new List<WoodBlankViewModel>();
            for (int i = 0; i < source.WoodBlanks.Count; ++i)
            {
                result.Add(new WoodBlankViewModel
                {
                    Id = source.WoodBlanks[i].Id,
                    WoodBlanksName = source.WoodBlanks[i].WoodBlanksName
                });
            }
            return result;
        }

        public WoodBlankViewModel GetElement(int id)
        {
            for (int i = 0; i < source.WoodBlanks.Count; ++i)
            {
                if (source.WoodBlanks[i].Id == id)
                {
                    return new WoodBlankViewModel
                    {
                        Id = source.WoodBlanks[i].Id,
                        WoodBlanksName = source.WoodBlanks[i].WoodBlanksName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WoodBlanksBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.WoodBlanks.Count; ++i)
            {
                if (source.WoodBlanks[i].Id > maxId)
                {
                    maxId = source.WoodBlanks[i].Id;
                }
                if (source.WoodBlanks[i].WoodBlanksName == model.WoodBlanksName)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            source.WoodBlanks.Add(new WoodBlank
            {
                Id = maxId + 1,
                WoodBlanksName = model.WoodBlanksName
            });
        }

        public void UpdElement(WoodBlanksBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.WoodBlanks.Count; ++i)
            {
                if (source.WoodBlanks[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.WoodBlanks[i].WoodBlanksName == model.WoodBlanksName &&
                    source.WoodBlanks[i].Id != model.Id)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.WoodBlanks[index].WoodBlanksName = model.WoodBlanksName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.WoodBlanks.Count; ++i)
            {
                if (source.WoodBlanks[i].Id == id)
                {
                    source.WoodBlanks.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
