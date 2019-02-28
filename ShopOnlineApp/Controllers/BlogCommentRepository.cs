using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.BlogComment;
using ShopOnlineApp.Application.ViewModels.Rating;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers
{
    public class BlogCommentController:Controller
    {
        private readonly IBlogCommentService _blogCommentService;
        public BlogCommentController(IBlogCommentService blogCommentService)
        {
            _blogCommentService = blogCommentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll(BlogCommentRequest request)
        {
            var allRates = _blogCommentService.GetAllPaging(request);

            return new OkObjectResult(allRates);
        }
        [HttpPost]
        public IActionResult Comment(BlogCommentViewModel rate)
        {
            rate.BlogId = HttpContext.Session.Get<int>(CommonConstants.BlogId);
            _blogCommentService.Add(rate);
            return new OkObjectResult(rate);
        }
    }
}