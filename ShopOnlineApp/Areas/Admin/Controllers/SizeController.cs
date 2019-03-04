using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Size;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class SizeController : BaseController
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            //var dataReturn = await _productService.GetAll();
            var model = _sizeService.GetAll();
            return new OkObjectResult(model);
        }

        public IActionResult GetAllPaging(SizeRequest request)
        {
            var model = _sizeService.GetAllPaging(request);

            return new OkObjectResult(model);

        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _sizeService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(SizeViewModel sizeVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (sizeVm.Id == 0)
                {
                    _sizeService.Add(sizeVm);
                }
                else
                {
                    _sizeService.Update(sizeVm);
                }
                _sizeService.SaveChanges();
                return new OkObjectResult(sizeVm);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            _sizeService.Delete(id);
            _sizeService.SaveChanges();
            return new OkObjectResult(id);
        }

    }
}