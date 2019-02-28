using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class BlogCategoryService:IBlogCategoryService
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlogCategoryService(IBlogCategoryRepository blogCategoryRepository,
            IUnitOfWork unitOfWork, IBlogRepository blogRepository)
        {
            _blogCategoryRepository = blogCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public BlogCategoryViewModel Add(BlogCategoryViewModel blogCategoryVm)
        {
            var blogCategory = new BlogCategoryViewModel().Map(blogCategoryVm);
            blogCategory.DateCreated=DateTime.Now;
            blogCategory.DateModified=DateTime.Now;
            _blogCategoryRepository.Add(blogCategory);
            return blogCategoryVm;
        }

        public void Delete(int id)
        {
            _blogCategoryRepository.Remove(id);
        }

        public async Task<List<BlogCategoryViewModel>> GetAll()
        {
            try
            {
                var dataReturn = await _blogCategoryRepository.FindAll().OrderBy(x => x.ParentId).AsNoTracking().ToListAsync();
                var items = new BlogCategoryViewModel().Map(dataReturn).ToList();
                return items;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<BlogCategoryViewModel>> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _blogCategoryRepository.FindAll(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId).AsNoTracking().ProjectTo<BlogCategoryViewModel>().ToList();

            return _blogCategoryRepository.FindAll().AsNoTracking().OrderBy(x => x.ParentId)
                .ProjectTo<BlogCategoryViewModel>()
                .ToList();
        }


        public async Task<List<BlogCategoryViewModel>> GetAllByParentId(int parentId)
        {
            var dataReturn = await _blogCategoryRepository.FindAll(x => x.Status == Status.Active
              && x.ParentId == parentId).ToListAsync();
            return new BlogCategoryViewModel().Map(dataReturn).ToList();
        }

        public BlogCategoryViewModel GetById(int id)
        {
            return new BlogCategoryViewModel().Map(_blogCategoryRepository.FindById(id));
        }

        //public async Task<List<BlogCategoryViewModel>> GetHomeCategories(int top)
        //{
        //    var categories = new BlogCategoryViewModel().Map(_blogCategoryRepository
        //        .FindAll(x => x.HomeFlag == true, c => c.Blogs)
        //        .OrderBy(x => x.HomeOrder)
        //        .Take(top)).ToList();

        //    foreach (var category in categories)
        //    {
        //        var item = new BlogViewModel().Map(_blogCategoryRepository.FindAll(x => x.HomeFlag.Value && x.CategoryId == category.Id)
        //            .OrderByDescending(x => x.DateCreated)
        //            .Take(5)).ToList();
        //    }
        //    return categories;
        //}

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _blogCategoryRepository.FindById(sourceId);
            var target = _blogCategoryRepository.FindById(targetId);

            var temp = 0;
            if (source != null && target != null)
            {
                temp = source.SortOrder;
                source.SortOrder = target.SortOrder;
                target.SortOrder = temp;
            }
            _blogCategoryRepository.Update(source);
            _blogCategoryRepository.Update(target);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        //public List<BlogCategoryViewModel> Unflatern()
        //{
        //    var listCategory = new BlogCategoryViewModel().Map(_blogCategoryRepository.FindAll());
        //    List<BlogCategoryViewModel> lstBlogCategoryViewModels= new List<BlogCategoryViewModel>();

        //    var BlogCategoryViewModels = listCategory as BlogCategoryViewModel[] ?? listCategory.ToArray();
        //    foreach (var item in BlogCategoryViewModels)
        //    {
        //        if (item.ParentId == null)
        //        {
        //            item.Children = BlogCategoryViewModels.Where(x => x.ParentId == item.Id).ToList();

        //        }
        //        else
        //        {

        //        }

        //    }

        //    return lstBlogCategoryViewModels;

        //}

        public void Update(BlogCategoryViewModel blogCategoryVm)
        {
            var blogCategory = new BlogCategoryViewModel().Map(blogCategoryVm);
            blogCategory.DateCreated = DateTime.Now;
            blogCategory.DateModified = DateTime.Now;
            _blogCategoryRepository.Update(blogCategory);

        }
        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _blogCategoryRepository.FindById(sourceId);

            sourceCategory.ParentId = targetId;

            _blogCategoryRepository.Update(sourceCategory);

            var sibling = _blogCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];

                _blogCategoryRepository.Update(child);
            }

        }
    }

}
