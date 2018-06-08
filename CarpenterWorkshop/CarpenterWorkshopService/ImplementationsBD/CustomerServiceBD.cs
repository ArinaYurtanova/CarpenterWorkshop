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
    public class CustomerServiceBD : ICustomerService
    {
        private AbstractDbContext context;

        public CustomerServiceBD( AbstractDbContext context)
        {
            this.context = context;
        }

        public List<CustomerViewModel> GetList()
        {
            List<CustomerViewModel> result = context.Customers
                .Select(rec => new CustomerViewModel
                {
                    Id = rec.Id,
                    CustomerFIO = rec.CustomerFIO,
                    Mail = rec.Mail
                })
                .ToList();
            return result;
        }

        public CustomerViewModel GetElement(int id)
        {
            Сustomer element = context.Customers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CustomerViewModel
                {
                    Id = element.Id,
                    CustomerFIO = element.CustomerFIO,
                    Mail = element.Mail,
                    Messages = context.MessageInfos
                            .Where(recM => recM.CustomerID == element.Id)
                            .Select(recM => new MessageInfoViewModel
                            {
                                MessageId = recM.MessageId,
                                DateDelivery = recM.DateDelivery,
                                Subject = recM.Subject,
                                Body = recM.Body
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(CustomerBidingModel model)
        {
            Сustomer element = context.Customers.FirstOrDefault(rec => rec.CustomerFIO == model.CustomerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Customers.Add(new Сustomer
            {
                CustomerFIO = model.CustomerFIO,
                Mail = model.Mail
            });
            context.SaveChanges();
        }

        public void UpdElement(CustomerBidingModel model)
        {
            Сustomer element = context.Customers.FirstOrDefault(rec =>
                                    rec.CustomerFIO == model.CustomerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Customers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.CustomerFIO = model.CustomerFIO;
            element.Mail = model.Mail;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Сustomer element = context.Customers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Customers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}