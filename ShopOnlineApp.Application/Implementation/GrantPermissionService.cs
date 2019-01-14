using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ShopOnlineApp.Application.Interfaces;
using ShopOnlineApp.Application.ViewModels.GranPermission;
using ShopOnlineApp.Data.Entities;
using ShopOnlineApp.Data.IRepositories;
using ShopOnlineApp.Infrastructure.Interfaces;

namespace ShopOnlineApp.Application.Implementation
{
    public class GrantPermissionService : IGrantPermissionService
    {

        private readonly IGrantPermissionRepository _grantPermissionRepository;
        private readonly IBusinessActionRepository _businessActionService;
        private readonly IUnitOfWork _unitOfWork;

        public GrantPermissionService(IGrantPermissionRepository grantPermissionRepository, IBusinessActionRepository businessActionService, IUnitOfWork unitOfWork)
        {
            _grantPermissionRepository = grantPermissionRepository;
            _businessActionService = businessActionService;
            _unitOfWork = unitOfWork;
        }
        public  List<PermissionActionViewModel> GetPermissons(string businessId, Guid userId)
        {

            var grantItems =(from g in _grantPermissionRepository.FindAll()
                 join p in _businessActionService.FindAll() on g.BusinessActionId equals p.Id
                 where g.UserId == userId && p.BusinessId == businessId
                 select new PermissionActionViewModel
                 {
                     Description = p.Description,
                     IsGranted = true,
                     PermissionID = p.Id,
                     PermissionName = p.Name
                 }).ToList();

            var allPermissons = from p in _businessActionService.FindAll().GroupBy(x=>x.Name).Select(x=>x.FirstOrDefault())
                               where p.BusinessId == businessId
                               select new PermissionActionViewModel
                               { PermissionID = p.Id, PermissionName = p.Name, Description = p.Description, IsGranted = false };

            var listpermissionId = grantItems.Select(p => p.PermissionID);

            foreach (var item in allPermissons)
            {
                if (!listpermissionId.Contains(item.PermissionID))
                    grantItems.Add(item);
            }

            return grantItems;
        }
        public bool UpdatePermisson(int id, Guid userId)
        {
            var grant = _grantPermissionRepository.FindAll(x=>x.BusinessActionId==id).SingleOrDefault();
            if (grant == null)
            {
                var shopGrant = new GrantPermission
                {
                  BusinessActionId = id,
                    UserId = userId
                };

                _grantPermissionRepository.Add(shopGrant);
                _unitOfWork.Commit();

                return true;
            }
            else
            {
                _grantPermissionRepository.Remove(grant);
                _unitOfWork.Commit();
                return false;
            }
        }

        public IEnumerable<string> GetRoleNameByUserId(Guid userId)
        {
            var permissions = from p in _businessActionService.FindAll().ToList()
                join g in _grantPermissionRepository.FindAll()
                    on p.Id equals g.BusinessActionId
                where g.UserId == userId
                select p.Name.ToLower();
            return permissions;

        }
    }
}
