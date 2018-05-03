using CarpenterWorkshopService.BindingModels;
using CarpenterWorkshopService.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarpenterWorkshopRestApi.Controllers
{
    public class MainController : ApiController
    {
        private readonly IMainService _service;

        public MainController(IMainService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void CreateOrder(OrdProductBindingModel model)
        {
            _service.CreateOrder(model);
        }

        [HttpPost]
        public void TakeOrderInWork(OrdProductBindingModel model)
        {
            _service.TakeOrderInWork(model);
        }

        [HttpPost]
        public void FinishOrder(OrdProductBindingModel model)
        {
            _service.FinishOrder(model.Id);
        }

        [HttpPost]
        public void PayOrder(OrdProductBindingModel model)
        {
            _service.PayOrder(model.Id);
        }

        [HttpPost]
        public void  PutComponentOnStock(StorageBlankBindingModel model)
        {
            _service.PutComponentOnStock(model);
        }
    }
}
