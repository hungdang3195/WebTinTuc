using ShopOnlineApp.Application.ViewModels.Color;
using ShopOnlineApp.Application.ViewModels.Rating;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IRatingService
    {
        RatingViewModel Add(RatingViewModel product);
        BaseReponse<ModelListResult<RatingViewModel>> GetAllPaging(RateRequest request);
    }
}
