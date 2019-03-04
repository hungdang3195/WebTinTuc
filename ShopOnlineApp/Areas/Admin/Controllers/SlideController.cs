using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Slide;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        private readonly ISlideService _slideService;
        public SlideController(ISlideService slideService)
        {
            _slideService = slideService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAllPaging(SlideRequest request)
        {
            var model = _slideService.GetAllPaging(request);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _slideService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(SlideViewModel slideVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (slideVm.Id == 0)
            {
                _slideService.Add(slideVm);
            }
            else
            {
                _slideService.Update(slideVm);
            }

            _slideService.SaveChanges();

            return new OkObjectResult(slideVm);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            _slideService.Delete(id);
            _slideService.SaveChanges();
            return new OkObjectResult(id);
        }
    }
}