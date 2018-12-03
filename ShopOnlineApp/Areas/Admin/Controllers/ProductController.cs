using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        #region public  property 
        private readonly IProductService _productService;
        #endregion
        #region constructer
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async  Task<IActionResult>  GetAll()
        {
            //var dataReturn = await _productService.GetAll();
            var model = await _productService.GetAll();
            return new OkObjectResult(model);

        }
    }
}