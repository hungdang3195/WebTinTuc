using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopOnlineApp.Application.ViewModels.Business;
using ShopOnlineApp.Application.ViewModels.BusinessAction;

namespace ShopOnlineApp.Application.Interfaces
{
    public interface IBusinessActionService
    {
        Task<List<BusinessActionViewModel>> GetAll(BusinessActionRequest request);
        List<BusinessActionViewModel>  GetByBusinessIds(string businessId);
        BusinessActionViewModel GetByActionId(int id);
        void Update(BusinessActionViewModel businessVm);


    }
}
