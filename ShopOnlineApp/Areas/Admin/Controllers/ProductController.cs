using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Models;
using ShopOnlineApp.Utilities.Helpers;

namespace ShopOnlineApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        #region public  property 
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        private readonly IOptions<CloudinaryImage> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        #endregion
        #region constructer
        public ProductController(IProductService productService, IConfiguration configuration, IOptions<CloudinaryImage> cloudinaryConfig)
        {
            _productService = productService;
            _configuration = configuration;
            _cloudinaryConfig = cloudinaryConfig;
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadImage()
        {
            string url = "";
            DateTime now = DateTime.Now;

            try
            {
                var files = Request.Form.Files;
                if (files.Count == 0)
                {
                    return new BadRequestObjectResult(files);
                }
                else
                {
                     
                    var file = files[0];
                    using (var stream = file.OpenReadStream())
                    {
                      
                        ImageUploadParams uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.Name, stream),
                            Transformation = new Transformation()
                                .Width(200).Height(200).Crop("fill").Gravity("face"),
                            PublicId = "Product/" + file.Name,
                            Overwrite = true,
                            NotificationUrl = "http://mysite/my_notification_endpoint"
                        };

                        ImageUploadResult uploadResult = _cloudinary.Upload(uploadParams);

                        if (uploadResult != null)
                        {
                            url = uploadResult.Uri.ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return new OkObjectResult(url);
        }
        public async  Task<IActionResult>  GetAll()
        {
            //var dataReturn = await _productService.GetAll();
            var model = await _productService.GetAll();
            return new OkObjectResult(model);
        }

        public async Task<IActionResult> GetAllPaging(ProductRequest request)
        {
            var model = await _productService.GetAllPaging(request);
            
            return  new OkObjectResult(model);

        }
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductViewModel productVm)
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
                    _productService.Add(productVm);
                }
                else
                {
                    _productService.Update(productVm);
                }
                _productService.Save();
                return new OkObjectResult(productVm);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _productService.Delete(id);
                _productService.Save();

                return new OkObjectResult(id);
            }
        }

    }
}