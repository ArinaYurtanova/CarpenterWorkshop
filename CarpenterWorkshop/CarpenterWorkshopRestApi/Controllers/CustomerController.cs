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
    public class CustomerController : ApiController
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(CustomerBidingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(CustomerBidingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(CustomerBidingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}

