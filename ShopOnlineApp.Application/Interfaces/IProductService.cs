using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IProductService:IDisposable
    {
        Task<List<ProductViewModel>> GetAll();
        Task<BaseReponse<ModelListResult<ProductViewModel>>> GetAllPaging(ProductRequest request);
        Task<ProductViewModel> Add(ProductViewModel product);
         ProductViewModel  GetById(int id);
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
    }
}
