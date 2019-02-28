using ShopOnlineApp.Application.ViewModels.BlogComment;
using ShopOnlineApp.Data.EF.Common;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IBlogCommentService
    {
        BlogCommentViewModel Add(BlogCommentViewModel product);
        BaseReponse<ModelListResult<BlogCommentViewModel>> GetAllPaging(BlogCommentRequest request);
    }
}
