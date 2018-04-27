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
    public class CustomerServiceList: ICustomerService
    {
        private DataListSingleton source;

        public CustomerServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<CustomerViewModel> GetList()
        {
            List<CustomerViewModel> result = new List<CustomerViewModel>();
            for (int i = 0; i < source.Сustomers.Count; ++i)
            {
                result.Add(new CustomerViewModel
                {
                    Id = source.Сustomers[i].Id,
                    CustomerFIO = source.Сustomers[i].CustomerFIO
                });
            }
            return result;
        }

        public CustomerViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Сustomers.Count; ++i)
            {
                if (source.Сustomers[i].Id == id)
                {
                    return new CustomerViewModel
                    {
                        Id = source.Сustomers[i].Id,
                        CustomerFIO = source.Сustomers[i].CustomerFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CustomerBidingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Сustomers.Count; ++i)
            {
                if (source.Сustomers[i].Id > maxId)
                {
                    maxId = source.Сustomers[i].Id;
                }
                if (source.Сustomers[i].CustomerFIO == model.CustomerFIO)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            source.Сustomers.Add(new Сustomer
            {
                Id = maxId + 1,
                CustomerFIO = model.CustomerFIO
            });
        }

        public void UpdElement(CustomerBidingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Сustomers.Count; ++i)
            {
                if (source.Сustomers[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Сustomers[i].CustomerFIO == model.CustomerFIO &&
                    source.Сustomers[i].Id != model.Id)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Сustomers[index].CustomerFIO = model.CustomerFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Сustomers.Count; ++i)
            {
                if (source.Сustomers[i].Id == id)
                {
                    source.Сustomers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}

