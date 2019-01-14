using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Product;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IProductCategoryService
    {
        ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm);
        void Update(ProductCategoryViewModel productCategoryVm);
        void Delete(int id);
        Task<List<ProductCategoryViewModel>> GetAll();
        Task<List<ProductCategoryViewModel>> GetAll(string keyword);
        Task<List<ProductCategoryViewModel>> GetAllByParentId(int parentId);
        ProductCategoryViewModel GetById(int id);
        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);
        void ReOrder(int sourceId, int targetId);
        Task<List<ProductCategoryViewModel>> GetHomeCategories(int top);

       // Task<List<ProductCategoryViewModel>> Unflatern();
        void Save();
    }
}
