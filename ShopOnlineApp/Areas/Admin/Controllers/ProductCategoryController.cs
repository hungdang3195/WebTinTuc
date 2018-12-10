using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Utilities.Helpers;

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
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var items = _productCategoryService.GetById(id);
            if (items != null)
            {
                return new OkObjectResult(items);
            }
            else
            {
                return new OkResult();
            }

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                _productCategoryService.Delete(id);
                _productCategoryService.Save();
                return new OkObjectResult(id);
            }
        }


        [HttpPost]
        public IActionResult SaveEntity(ProductCategoryViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
                if (productVm.Id == 0)
                {
                    _productCategoryService.Add(productVm);
                }
                else
                {
                    _productCategoryService.Update(productVm);
                }
                _productCategoryService.Save();
                return new OkObjectResult(productVm);

            }
        }
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestObjectResult(ModelState);
                }
                else
                {
                    _productCategoryService.UpdateParentId(sourceId, targetId,items);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        public IActionResult ReOrder(int sourceId,int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else 
            {
                if (sourceId == targetId)
                {
                    return new BadRequestObjectResult(ModelState);
                }
                else
                {
                   _productCategoryService.ReOrder(sourceId,targetId);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
            
        }

       


        public IActionResult GetAll()
        {
            var items = _productCategoryService.GetAll();
            return new OkObjectResult(items);
        }

        #endregion

    }
}