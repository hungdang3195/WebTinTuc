using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Slide;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class SlideService : ISlideService
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SlideService(ISlideRepository slideRepository,
            IUnitOfWork unitOfWork)
        {
            _slideRepository = slideRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(SlideViewModel pageVm)
        {
            var page = new SlideViewModel().Map(pageVm);
            _slideRepository.Add(page);
        }

        public void Delete(int id)
        {
            _slideRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<SlideViewModel> GetAll()
        {
            return new SlideViewModel().Map(_slideRepository.FindAll().AsNoTracking().AsParallel().AsOrdered().WithDegreeOfParallelism(2)).ToList();
        }

        public BaseReponse<ModelListResult<SlideViewModel>> GetAllPaging(SlideRequest request)
        {

            var query = _slideRepository.FindAll().AsNoTracking().AsParallel();
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.AsParallel().AsOrdered().WithDegreeOfParallelism(3).Where(x => x.Name.Contains(request.SearchText));


            int totalRow = query.AsParallel().AsOrdered().WithDegreeOfParallelism(3).Count();
            var data = query.AsParallel().AsOrdered().WithDegreeOfParallelism(3).OrderByDescending(x => x.Id)
                .Skip((request.PageIndex-1) * request.PageSize)
                .Take(request.PageSize);

            var items = new SlideViewModel().Map(data).ToList();

            return new BaseReponse<ModelListResult<SlideViewModel>>
            {
                Data = new ModelListResult<SlideViewModel>()
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

        public SlideViewModel GetById(int id)
        {
            return new SlideViewModel().Map(_slideRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(SlideViewModel pageVm)
        {
            var page = new SlideViewModel().Map(pageVm);
            _slideRepository.Update(page);
        }
    }

}
