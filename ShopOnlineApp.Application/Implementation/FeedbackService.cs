using System;
using System.Collections.Generic;
using System.Linq;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Feedback;
using ShopOnlineApp.Data.EF.Common;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;
using ShopOnlineApp.Utilities.Enum;

namespace ShopOnlineApp.Application.Implementation
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackService(IFeedbackRepository feedbackRepository,
            IUnitOfWork unitOfWork)
        {
            _feedbackRepository = feedbackRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(FeedbackViewModel feedbackVm)
        {
            var page = new  FeedbackViewModel().Map(feedbackVm);
            _feedbackRepository.Add(page);
        }

        public void Delete(int id)
        {
            _feedbackRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<FeedbackViewModel> GetAll()
        {
            return new FeedbackViewModel().Map(_feedbackRepository.FindAll()).ToList();
        }

        public BaseReponse<ModelListResult<FeedbackViewModel>> GetAllPaging(FeedbackRequest request)
        {

            var query = _feedbackRepository.FindAll();
            if (!string.IsNullOrEmpty(request.SearchText))
                query = query.Where(x => x.Name.Contains(request.SearchText));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((request.PageIndex) * request.PageSize)
                .Take(request.PageSize);

            var items = new FeedbackViewModel().Map(data).ToList();

            return new BaseReponse<ModelListResult<FeedbackViewModel>>
            {
                Data = new ModelListResult<FeedbackViewModel>()
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

        public FeedbackViewModel GetById(int id)
        {
            return new FeedbackViewModel().Map(_feedbackRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(FeedbackViewModel feedbackVm)
        {
            var page =new FeedbackViewModel().Map(feedbackVm);

            _feedbackRepository.Update(page);
        }
    }
}
