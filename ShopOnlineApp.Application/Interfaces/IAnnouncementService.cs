using System;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.ViewModels.Annoucement;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IAnnouncementService
    {
         PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize);

        bool MarkAsRead(Guid userId, string id);

        void AddAnnoucement(AnnouncementViewModel announcementVM);
    }
}
