using System.Collections.Generic;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Feedback;

namespace ShopOnlineApp.Application.Implementation
{
    public interface IFeedbackService
    {
        void Add(FeedbackViewModel feedbackVm);

        void Update(FeedbackViewModel feedbackVm);

        void Delete(int id);

        List<FeedbackViewModel> GetAll();

        PagedResult<FeedbackViewModel> GetAllPaging(FeedbackRequest request);

        FeedbackViewModel GetById(int id);

        void SaveChanges();
    }
}
