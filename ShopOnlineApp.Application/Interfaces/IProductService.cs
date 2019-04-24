using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Application.ViewModels.Tag;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {
        Task<List<ProductViewModel>> GetAll();
        Task<BaseReponse<ModelListResult<ProductViewModel>>> GetAllPaging(ProductRequest request);
        ModelListResult<ProductFullViewModel> FilterProducts(ProductRequest request);

        Task<ProductViewModel> Add(ProductViewModel product);
        ProductViewModel GetById(int id);
        void Update(ProductViewModel product);
        void Save();
        void Delete(int id);
        void ImportExcel(string filePath, int categoryId);
        void AddQuantity(int productId, List<ProductQuantityViewModel> quantities);
        List<ProductQuantityViewModel> GetQuantities(int productId);
        void AddImages(int productId, string[] images);
        List<ProductImageViewModel> GetImages(int productId);
        void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);
        List<WholePriceViewModel> GetWholePrices(int productId);
        List<ProductViewModel> GetLastest(int top);
        List<ProductViewModel> GetHotProduct(int top);
        List<ProductViewModel> GetRelatedProducts(int id, int top);
        List<ProductViewModel> GetRatingProducts(int top);
        List<ProductViewModel> GetUpsellProducts(int top);

        List<TagViewModel> GetProductTags(int productId);

        bool CheckAvailability(int productId, int size, int color);
    }
}
