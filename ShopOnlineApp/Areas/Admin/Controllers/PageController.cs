using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Page;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class PageController : BaseController
    {
        public readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var model = _pageService.GetAll();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _pageService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(PageRequest request)
        {
            var model = _pageService.GetAllPaging(request);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(PageViewModel pageVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            if (pageVm.Id == 0)
            {
                _pageService.Add(pageVm);
            }
            else
            {
                _pageService.Update(pageVm);
            }
            _pageService.SaveChanges();
            return new OkObjectResult(pageVm);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            _pageService.Delete(id);
            _pageService.SaveChanges();

            return new OkObjectResult(id);
        }
    }
}