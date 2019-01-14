using System;
using System.Collections.Generic;
using ShopOnlineApp.Application.ViewModels.GranPermission;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IGrantPermissionService
    {
        List<PermissionActionViewModel> GetPermissons(string businessId,Guid userId);

        bool UpdatePermisson(int id, Guid userId);
        IEnumerable<string> GetRoleNameByUserId(Guid userId);
    }
}
