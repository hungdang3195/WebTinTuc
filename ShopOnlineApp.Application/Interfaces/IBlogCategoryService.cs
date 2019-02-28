using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IBlogCategoryService
    {
        BlogCategoryViewModel Add(BlogCategoryViewModel BlogCategoryVm);
        void Update(BlogCategoryViewModel BlogCategoryVm);
        void Delete(int id);
        Task<List<BlogCategoryViewModel>> GetAll();
        Task<List<BlogCategoryViewModel>> GetAll(string keyword);
        Task<List<BlogCategoryViewModel>> GetAllByParentId(int parentId);
        BlogCategoryViewModel GetById(int id);
        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);
        void ReOrder(int sourceId, int targetId);
       // Task<List<BlogCategoryViewModel>> GetHomeCategories(int top);

        // Task<List<BlogCategoryViewModel>> Unflatern();
        void Save();
    }
}
