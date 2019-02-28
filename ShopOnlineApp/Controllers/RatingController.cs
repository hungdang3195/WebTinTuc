using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Rating;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers
{
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll(RateRequest request)
        {
            var allRates = _ratingService.GetAllPaging(request);

            return new OkObjectResult(allRates);
        }
        [HttpPost]
        public IActionResult Rating(RatingViewModel rate)
        {
            rate.ProductId = HttpContext.Session.Get<int>(CommonConstants.ProductId);
            _ratingService.Add(rate);
            return new OkObjectResult(rate);
        }
    }
}