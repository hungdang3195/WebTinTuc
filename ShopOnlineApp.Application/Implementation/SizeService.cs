using System;
using System.Collections.Generic;
using System.Linq;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Size;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class SizeService:ISizeService
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SizeService(ISizeRepository sizeRepository,
            IUnitOfWork unitOfWork)
        {
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(SizeViewModel pageVm)
        {
            var page = new SizeViewModel().Map(pageVm);
            _sizeRepository.Add(page);
        }

        public void Delete(int id)
        {
            _sizeRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<SizeViewModel> GetAll()
        {
            return new SizeViewModel().Map(_sizeRepository.FindAll()).ToList();
        }

        public BaseReponse<ModelListResult<SizeViewModel>> GetAllPaging(SizeRequest request)
        {

            var query = _sizeRepository.FindAll();
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.Where(x => x.Name.Contains(request.SearchText));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((request.PageIndex) * request.PageSize)
                .Take(request.PageSize);

            var items = new SizeViewModel().Map(data).ToList();

            return new BaseReponse<ModelListResult<SizeViewModel>>
            {
                Data = new ModelListResult<SizeViewModel>()
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

        public SizeViewModel GetById(int id)
        {
            return new SizeViewModel().Map(_sizeRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(SizeViewModel pageVm)
        {
            var page = new SizeViewModel().Map(pageVm);
            _sizeRepository.Update(page);
        }
    }
}
