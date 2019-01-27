using System.Collections.Generic;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Feedback;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IFeedbackService
    {
        void Add(FeedbackViewModel feedbackVm);

        void Update(FeedbackViewModel feedbackVm);

        void Delete(int id);

        List<FeedbackViewModel> GetAll();

        BaseReponse<ModelListResult<FeedbackViewModel>> GetAllPaging(FeedbackRequest request);

        FeedbackViewModel GetById(int id);

        void SaveChanges();
    }
}
