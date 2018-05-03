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
    public class WoodBlankController : ApiController
    { 
   private readonly IWoodBlankService _service;

    public WoodBlankController(IWoodBlankService service)
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
    public void AddElement(WoodBlanksBindingModel model)
    {
        _service.AddElement(model);
    }

    [HttpPost]
    public void UpdElement(WoodBlanksBindingModel model)
    {
        _service.UpdElement(model);
    }

    [HttpPost]
    public void DelElement(WoodBlanksBindingModel model)
    {
        _service.DelElement(model.Id);
    }
}
}
