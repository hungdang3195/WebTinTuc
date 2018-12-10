using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Product;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory =new ProductCategoryViewModel().Map(productCategoryVm);
            _productCategoryRepository.Add(productCategory);
            return productCategoryVm;
        }

        public void Delete(int id)
        {
            _productCategoryRepository.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return new ProductCategoryViewModel().Map(_productCategoryRepository.FindAll(x=>x.Name !="test").OrderBy(x => x.ParentId).AsNoTracking()).ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productCategoryRepository.FindAll(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId).ProjectTo<ProductCategoryViewModel>().ToList();
            else
                return _productCategoryRepository.FindAll().OrderBy(x => x.ParentId)
                    .ProjectTo<ProductCategoryViewModel>()
                    .ToList();
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _productCategoryRepository.FindAll(x => x.Status == Status.Active
            && x.ParentId == parentId)
             .ProjectTo<ProductCategoryViewModel>()
             .ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return new ProductCategoryViewModel().Map(_productCategoryRepository.FindById(id));
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _productCategoryRepository
                .FindAll(x => x.HomeFlag == true, c => c.Products)
                  .OrderBy(x => x.HomeOrder)
                  .Take(top).ProjectTo<ProductCategoryViewModel>();

            var categories = query.ToList();
            foreach (var category in categories)
            {
                //category.Products = _productCategoryRepository.FindAll(x => x.HotFlag == true && x.CategoryId == category.Id)
                //    .OrderByDescending(x => x.DateCreated)
                //    .Take(5)
                //    .ProjectTo<ProductViewModel>().ToList();
            }
            return categories;
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            var target = _productCategoryRepository.FindById(targetId);

            var temp = 0;
            if (source != null && target != null)
            {
                temp = source.SortOrder;
                source.SortOrder = target.SortOrder;
                target.SortOrder = temp;
            }
            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<ProductCategoryViewModel> Unflatern()
        {
            var listCategory = new ProductCategoryViewModel().Map(_productCategoryRepository.FindAll());
            List<ProductCategoryViewModel> lstProductCategoryViewModels= new List<ProductCategoryViewModel>();

            var productCategoryViewModels = listCategory as ProductCategoryViewModel[] ?? listCategory.ToArray();
            foreach (var item in productCategoryViewModels)
            {
                if (item.ParentId == null)
                {
                    item.Children = productCategoryViewModels.Where(x => x.ParentId == item.Id).ToList();
                    
                }
                else
                {
                    
                }

            }

            return lstProductCategoryViewModels;

        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = new ProductCategoryViewModel().Map(productCategoryVm);

             _productCategoryRepository.Update(productCategory);
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _productCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;

            _productCategoryRepository.Update(sourceCategory);

            var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));

            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];

                _productCategoryRepository.Update(child);
            }
        }
    }
}
