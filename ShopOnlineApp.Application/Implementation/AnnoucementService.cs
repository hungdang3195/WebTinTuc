using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using ShopOnlineApp.Application.Common;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.Annoucement;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IRepository<Announcement, string> _announcementRepository;
        private readonly IRepository<AnnouncementUser, int> _announcementUserRepository;
        

        public AnnouncementService(IRepository<Announcement, string> announcementRepository,
            IRepository<AnnouncementUser, int> announcementUserRepository,
            IUnitOfWork unitOfWork)
        {
            _announcementUserRepository = announcementUserRepository;
            this._announcementRepository = announcementRepository;
        }

        public PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize)
        {
            var query = from x in _announcementRepository.FindAll()
                        join y in _announcementUserRepository.FindAll()
                            on x.Id equals y.AnnouncementId
                            into xy
                        from annonUser in xy.DefaultIfEmpty()
                        where annonUser.HasRead == false && (annonUser.UserId == null || annonUser.UserId == userId)
                        select x;
            int totalRow = query.Count();

            var model = new AnnouncementViewModel().Map(query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize)).ToList();

            var paginationSet = new PagedResult<AnnouncementViewModel>
            {
                Results = model,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public bool MarkAsRead(Guid userId, string id)
        {
            bool result = false;
            var announ = _announcementUserRepository.FindSingle(x => x.AnnouncementId == id
                                                                               && x.UserId == userId);
            if (announ == null)
            {
                _announcementUserRepository.Add(new AnnouncementUser
                {
                    AnnouncementId = id,
                    UserId = userId,
                    HasRead = true
                });
                result = true;
            }
            else
            {
                if (announ.HasRead == false)
                {
                    announ.HasRead = true;
                    _announcementUserRepository.SaveChanges();
                    result = true;
                }

            }
            return result;
        }

        public void AddAnnoucement(AnnouncementViewModel announcementVm)
        {
            var annoucement = new AnnouncementViewModel().Map(announcementVm);

           _announcementRepository.Add(annoucement);
            _announcementUserRepository.Add(new AnnouncementUser
            {
                AnnouncementId = annoucement.Id,
                HasRead = false,
                UserId = annoucement.UserId
            });
            _announcementRepository.SaveChanges();
            _announcementUserRepository.SaveChanges();
        }
    }
}
