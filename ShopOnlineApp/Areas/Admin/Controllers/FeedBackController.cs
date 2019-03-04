using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Feedback;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class FeedBackController : BaseController
    {
        private readonly IFeedbackService _feedbackService;
        public FeedBackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAllPaging(FeedbackRequest request)
        {
            var model = _feedbackService.GetAllPaging(request);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            _feedbackService.Delete(id);
            _feedbackService.SaveChanges();
            return new OkObjectResult(id);
        }

    }
}