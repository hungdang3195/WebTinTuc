using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Extensions;
using ShopOnlineApp.Models;
using ShopOnlineApp.Models.ProductCategoryViewModels;
using ShopOnlineApp.Models.ProductViewModels;
using ShopOnlineApp.Utilities.Constants;

namespace ShopOnlineApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBillService _billService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IConfiguration _configuration;
        private readonly ISizeService _sizeService;
        private readonly IColorService _colorService;
        public ProductController(IProductService productService, IConfiguration configuration,
            IBillService billService,
            IProductCategoryService productCategoryService, ISizeService sizeService, IColorService colorService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _sizeService = sizeService;
            _colorService = colorService;
            _configuration = configuration;
            _billService = billService;
        }
        [Route("products.html")]
        public async Task<IActionResult> Index()
        {
            var categories = await _productCategoryService.GetAll();
            return View(categories);
        }
        [Route("quick-view.{id}.html")]
        public IActionResult QuickView(int id)
        {
            ViewData["BodyClass"] = "product-page";
            var model = new DetailViewModel();
            model.Product = _productService.GetById(id);
            model.Category = _productCategoryService.GetById(model.Product.CategoryId);
            // model.RelatedProducts = _productService.GetRelatedProducts(id, 9);
            //model.UpsellProducts = _productService.GetUpsellProducts(6);
            model.ProductImages = _productService.GetImages(id);
            model.ProductImages.Add(new ProductImageViewModel
            {
                Path = model.Product.Image
            });
            model.Tags = _productService.GetProductTags(id);
            model.Colors = _billService.GetColors().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            model.Sizes = _billService.GetSizes().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View(model);
        }

        //[Route("{alias}-c.{id}.html")]
        //public async Task<IActionResult>  Catalog(int id, int? pageSize, string sortBy, int page = 1)
        //{
        //    var request= new ProductRequest();
        //    var catalog = new CatalogViewModel();
        //    ViewData["BodyClass"] = "shop_grid_full_width_page";
        //    request.PageSize = pageSize ?? _configuration.GetValue<int>("PageSize");
        //    request.CategoryId = id;
        //    catalog.PageSize = request.PageSize;
        //    request.SortBy = sortBy;
        //    request.PageIndex = page;
        //    catalog.SortType = sortBy;
        //    var data = await _productService.GetAllPaging(request);
        //    catalog.Data = data.Data;
        //    catalog.Category = _productCategoryService.GetById(id);
        //    return View(catalog);
        //}

        [Route("{alias}-c.{id}.html")]
        public async Task<IActionResult> ProductCatalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            var productCategory = new ProductCategoryListViewModel();

            var categories = new List<LookupItem>();

            var category = _productCategoryService.GetById(id);

            if (category.ParentId == null)
            {
                // lấy tất cả danh sách các sản phẩm cùng cha
                var dataReturn = await _productCategoryService.GetAllByParentId(category.Id);
                categories = dataReturn.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Select(x => new LookupItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Key= x.SeoAlias
                }).ToList();
            }
            else
            {
                var dataReturn = await _productCategoryService.GetAllByParentId(category.ParentId.Value);
                categories = dataReturn.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism).Select(x => new LookupItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Key = x.SeoAlias
                }).ToList();
            }

            var sizeVMs = _sizeService.GetAll();
            productCategory.Sizes = sizeVMs.Select(x => new LookupItem
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            var colorVMs = _colorService.GetAll();
            productCategory.Colors = colorVMs.Select(x => new LookupItem
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            var categoryCurrent= _productCategoryService.GetById(id);


            productCategory.CurrentCategory = new LookupItem
            {
                Id = categoryCurrent.Id,
                Name = categoryCurrent.SeoAlias
            };
            //get cart model

           productCategory.CartViewModels = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);

            productCategory.Categories = categories;
            return View(productCategory);
        }

        [Route("loadData.html")]
        public IActionResult GetDataCategory(int id, int? pageSize, string sortBy, int colorId, int sizeId,
            int page = 1)
        {
            sortBy = sortBy ?? "";

            var request = new ProductRequest();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            request.PageSize = pageSize ?? _configuration.GetValue<int>("PageSize");
            request.CategoryId = id;
            request.SortBy = sortBy;
            request.PageIndex = page;
            request.ColorId = colorId;
            request.SizeId = sizeId;

            var dataReturn = _productService.FilterProducts(request);

            return new OkObjectResult(dataReturn);
        }

        [Route("search.html")]
        public async Task<IActionResult> Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new CatalogViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            catalog.PageSize = pageSize ?? _configuration.GetValue<int>("PageSize");
            catalog.SortType = sortBy;
            var request = new ProductRequest { PageSize = pageSize ?? _configuration.GetValue<int>("PageSize") };
            request.SortBy = sortBy;
            request.SearchText = keyword;
            request.PageIndex = page;
            var data = await _productService.GetAllPaging(request);
            catalog.Data = data.Data;
            catalog.Keyword = keyword;
            return View(catalog);
        }

        [Route("{alias}-p.{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = "product-page";
            var model = new DetailViewModel();
            model.Product = _productService.GetById(id);
            model.Category = _productCategoryService.GetById(model.Product.CategoryId);
            model.RelatedProducts = _productService.GetRelatedProducts(id, 9);
            model.UpsellProducts = _productService.GetUpsellProducts(6);
            model.ProductImages = _productService.GetImages(id);
            model.ProductImages.Add(new ProductImageViewModel
            {
                Path = model.Product.Image
            });
            model.Tags = _productService.GetProductTags(id);
            model.Colors = _billService.GetColors().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            model.Sizes = _billService.GetSizes().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            HttpContext.Session.Set(CommonConstants.ProductId, id);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SuggestSearch(string keyword)
        {
            var model = await _productService.GetAllPaging(new ProductRequest()
            {
                SearchText = keyword,
                PageSize = 5,
                PageIndex = 1
            });
            return new OkObjectResult(model.Data.Items.Select(x => new { x.Name, x.Image, x.Price }));
        }

    }
}