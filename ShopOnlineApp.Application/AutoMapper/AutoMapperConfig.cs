using System;
using System.Runtime.CompilerServices;
using AutoMapper;
using ShopOnlineApp.Application.ViewModels.Function;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Data.Entities;

namespace ShopOnlineApp.Application.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Config = new MapperConfiguration(cfg =>
        {
            try
            {
                #region Product
                cfg.CreateMap<Product, ProductViewModel>();
                cfg.CreateMap<ProductViewModel, Product>()
                    .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice,
                        c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit, c.Status,
                        c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));
                #endregion

                #region Function
                cfg.CreateMap<Function, FunctionViewModel>();
                cfg.CreateMap<FunctionViewModel, Function>();

                #endregion

                #region ProductCategory
                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>().ForMember(x=>x.Children,opt=>opt.Ignore());
                cfg.CreateMap<ProductCategoryViewModel, ProductCategory>(); ;

                #endregion
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        });
    }

}
