using CarpenterWorkshop;
using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using CarpenterWorkshopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace CarpenterWorkshopService.ImplementationsBD
{
    public class MainServiceBD : IMainService
    {
        private AbstractDbContext context;

        public MainServiceBD( AbstractDbContext context)
        {
            this.context = context;
        }

        public List<OrdProductViewModel> GetList()
        {
            List<OrdProductViewModel> result = context.OrdProducts
                .Select(rec => new OrdProductViewModel
                {
                    Id = rec.Id,
                    CustomerID = rec.CustomerID,
                    WoodCraftsID = rec.WoodCraftsID,
                    WorkerID = rec.WorkerID,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateImplement = rec.DateImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = rec.Customer.CustomerFIO,
                    WoodCraftsName = rec.WoodCraft.WoodCraftsName,
                    WorkerName = rec.Worker.WorkerFIO
                })
                .ToList();
            return result;
        }

        public void CreateOrder(OrdProductBindingModel model)
        {
            var OrdProduct = new OrdProduct
            {
                CustomerID = model.CustomerID,
                WoodCraftsID = model.WoodCraftsID,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = ReadyProduct.Принят
            };
            context.OrdProducts.Add(OrdProduct);
            context.SaveChanges();

            var Customer = context.Customers.FirstOrDefault(x => x.Id == model.CustomerID);
            SendEmail(Customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} создан успешно", OrdProduct.Id,
                OrdProduct.DateCreate.ToShortDateString()));
        }

        public void TakeOrderInWork(OrdProductBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    OrdProduct element = context.OrdProducts.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var BlankCrafts = context.BlankCrafts
                                                .Include(rec => rec.WoodBlank)
                                                .Where(rec => rec.WoodCraftsID == element.WoodCraftsID);
                    // списываем
                    foreach (var BlankCraft in BlankCrafts)
                    {
                        int countOnStorages = BlankCraft.Count * element.Count;
                        var storageBlanks = context.StorageBlanks
                                                    .Where(rec => rec.WoodBlanksID == BlankCraft.WoodBlanksID);
                        foreach (var storageBlank in storageBlanks)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (storageBlank.Count >= countOnStorages)
                            {
                                storageBlank.Count -= countOnStorages;
                                countOnStorages = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStorages -= storageBlank.Count;
                                storageBlank.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStorages > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                BlankCraft.WoodBlank.WoodBlanksName + " требуется " +
                                BlankCraft.Count + ", не хватает " + countOnStorages);
                        }
                    }
                    element.WorkerID = model.WorkerID;
                    element.DateImplement = DateTime.Now;
                    element.Status = ReadyProduct.Выполняется;
                    context.SaveChanges();
                    SendEmail(element.Customer.Mail, "Оповещение по заказам",
                        string.Format("Заказ №{0} от {1} передеан в работу", element.Id, element.DateCreate.ToShortDateString()));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        public void FinishOrder(int id)
        {
            OrdProduct element = context.OrdProducts.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ReadyProduct.Готов;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} передан на оплату", element.Id,
                element.DateCreate.ToShortDateString()));
        }

        public void PayOrder(int id)
        {
            OrdProduct element = context.OrdProducts.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ReadyProduct.Оплачен;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} оплачен успешно", element.Id, element.DateCreate.ToShortDateString()));
        }

        public void PutComponentOnStock(StorageBlankBindingModel model)
        {
            StorageBlank element = context.StorageBlanks
                                                .FirstOrDefault(rec => rec.StorageID == model.StorageID &&
                                                                    rec.WoodBlanksID == model.WoodBlanksID);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.StorageBlanks.Add(new StorageBlank
                {
                    StorageID = model.StorageID,
                    WoodBlanksID = model.WoodBlanksID,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;

            try
            {
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]);

                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}