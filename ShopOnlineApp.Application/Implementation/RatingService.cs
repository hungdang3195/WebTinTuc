using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Rating;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RatingService(IRatingRepository ratingRepository, IUnitOfWork unitOfWork)
        {
            _ratingRepository = ratingRepository;
            _unitOfWork = unitOfWork;
        }

        public RatingViewModel Add(RatingViewModel ratingVm)
        {
            var page = new RatingViewModel().Map(ratingVm);
            _ratingRepository.Add(page);
            _unitOfWork.Commit();
            return ratingVm;
        }

        public BaseReponse<ModelListResult<RatingViewModel>> GetAllPaging(RateRequest request)
        {
            var query = _ratingRepository.FindAll().AsNoTracking().AsParallel();

            if (request.ProductId >0)
            {
                query = query.AsParallel().AsOrdered().WithDegreeOfParallelism(2).Where(x => x.ProductId == request.ProductId);
            }

            int totalRow = query.AsParallel().AsOrdered().WithDegreeOfParallelism(2).Count();
            query = query.AsParallel().AsOrdered().WithDegreeOfParallelism(2).OrderByDescending(x => x.DateCreated)
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize);

            var items = new RatingViewModel().Map(query).ToList();

            return new BaseReponse<ModelListResult<RatingViewModel>>
            {
                Data = new ModelListResult<RatingViewModel>()
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
    }
}
