using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Models.ProductViewModels;

namespace ShopOnlineApp.Controllers
{
    public class ProductController : Controller
    {
       private readonly IProductService _productService;
        private readonly IBillService _billService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IConfiguration _configuration;
        public ProductController(IProductService productService, IConfiguration configuration,
            IBillService billService,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
            _billService = billService;
        }
        [Route("products.html")]
        public async Task<IActionResult>  Index()
        {
            var categories =await _productCategoryService.GetAll();
            return View(categories);
        }

        [Route("{alias}-c.{id}.html")]
        public async Task<IActionResult>  Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            var request= new ProductRequest();

            var catalog = new CatalogViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";

            
            request.PageSize = pageSize ?? _configuration.GetValue<int>("PageSize");
            request.CategoryId = id;
            catalog.PageSize = request.PageSize;
            request.SortBy = sortBy;
            request.PageIndex = page;
            catalog.SortType = sortBy;

            var data = await _productService.GetAllPaging(request);
            catalog.Data = data.Data;
            catalog.Category = _productCategoryService.GetById(id);
            return View(catalog);
        }


        [Route("search.html")]
        public async Task<IActionResult>  Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new CatalogViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            catalog.PageSize = pageSize ?? _configuration.GetValue<int>("PageSize");
            catalog.SortType = sortBy;
            var request = new ProductRequest {PageSize = pageSize ?? _configuration.GetValue<int>("PageSize")};
            request.SortBy = sortBy;
            request.SearchText = keyword;
            request.PageIndex = page;
            var data =await _productService.GetAllPaging(request);
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

    }
}