using System;
using System.Collections.Generic;
using System.Linq;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Page;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PageService(IPageRepository pageRepository,
            IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(PageViewModel pageVm)
        {
            var page =new PageViewModel().Map(pageVm);
            _pageRepository.Add(page);
        }

        public void Delete(int id)
        {
            _pageRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<PageViewModel> GetAll()
        {
            return new PageViewModel().Map(_pageRepository.FindAll()).ToList();
        }

        public BaseReponse<ModelListResult<PageViewModel>> GetAllPaging(PageRequest request)
        {
            var query = _pageRepository.FindAll();
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.Where(x => x.Name.Contains(request.SearchText));

            int totalRow = query.Count();

            var data = query.OrderByDescending(x => x.Alias)
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize);

            var items = new PageViewModel().Map(data).ToList();

            return new BaseReponse<ModelListResult<PageViewModel>>()
            {
                Data = new ModelListResult<PageViewModel>()
                {
                    Items = items,
                    Message = Message.Success,
                    RowCount = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex
                },
                Message = Message.Success,
                Status = (int)QueryStatus.Success
            };
        }

        public PageViewModel GetByAlias(string alias)
        {
            return new PageViewModel().Map(_pageRepository.FindSingle(x => x.Alias == alias));
        }

        public PageViewModel GetById(int id)
        {
            return new PageViewModel().Map(_pageRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(PageViewModel pageVm)
        {
            var page =new  PageViewModel().Map(pageVm);
            _pageRepository.Update(page);
        }
    }
}
