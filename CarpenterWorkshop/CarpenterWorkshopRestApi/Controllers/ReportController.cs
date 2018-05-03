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
    public class ReportController : ApiController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetStocksLoad()
        {
            var list = _service.GetStoragesLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetClientOrders(ReportBindingModel model)
        {
            var list = _service.GetCustomerOrders(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveProductPrice(ReportBindingModel model)
        {
            _service.SaveProductPrice(model);
        }

        [HttpPost]
        public void SaveStocksLoad(ReportBindingModel model)
        {
            _service.SaveStoragesLoad(model);
        }

        [HttpPost]
        public void SaveClientOrders(ReportBindingModel model)
        {
            _service.SaveCustomerOrders(model);
        }
    }
}
