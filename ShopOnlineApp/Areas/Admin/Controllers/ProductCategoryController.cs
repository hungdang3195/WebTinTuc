using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        #region private property

        private readonly IProductCategoryService _productCategoryService;

        #endregion

        #region constructor
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        #endregion

        #region method

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var items = _productCategoryService.GetAll();
            return new OkObjectResult(items);
        }

        #endregion

    }
}