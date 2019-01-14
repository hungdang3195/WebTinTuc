using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.BusinessAction;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class BusinessActionController:BaseController
    {
        private readonly IBusinessActionService _actionService;

        public BusinessActionController(IBusinessActionService actionService)
        {
            _actionService = actionService;
        }

        public IActionResult GetAll(BusinessActionRequest request)
        {
            return new OkObjectResult(_actionService.GetAll(request));
        }

        public IActionResult Index([FromQuery]string businessId)
        {
            var items = _actionService.GetByBusinessIds(businessId);
            return View(items);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var currentAction = _actionService.GetByActionId(id);
            return new OkObjectResult(currentAction);
        }


        [HttpPost]
        public IActionResult SaveEntity(BusinessActionViewModel businessVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (businessVm.Id == null)
            {
                //await _businessService.AddAsync(businessVm);
            }
            else
            {
                _actionService.Update(businessVm);
            }
            return new OkObjectResult(businessVm);
        }



    }
}
